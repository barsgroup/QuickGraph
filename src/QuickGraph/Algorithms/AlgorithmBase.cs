﻿namespace QuickGraph.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Services;

    public abstract class AlgorithmBase<TGraph> :
        IAlgorithm<TGraph>,
        IAlgorithmComponent
    {
        private readonly AlgorithmServices services;

        private Dictionary<Type, object> _services;

        private volatile ComputationState _state = ComputationState.NotRunning;

        private volatile object _syncRoot = new object();

        public TGraph VisitedGraph { get; }

        public object SyncRoot => _syncRoot;

        public ComputationState State
        {
            get
            {
                lock (_syncRoot)
                {
                    return _state;
                }
            }
        }

        public IAlgorithmServices Services => services;

        /// <summary>Creates a new algorithm with an (optional) host.</summary>
        /// <param name="host">if null, host is set to the this reference</param>
        /// <param name="visitedGraph"></param>
        protected AlgorithmBase(IAlgorithmComponent host, TGraph visitedGraph)
        {
            Contract.Requires(visitedGraph != null);
            if (host == null)
            {
                host = this;
            }
            VisitedGraph = visitedGraph;
            services = new AlgorithmServices(host);
        }

        protected AlgorithmBase(TGraph visitedGraph)
        {
            Contract.Requires(visitedGraph != null);
            VisitedGraph = visitedGraph;
            services = new AlgorithmServices(this);
        }

        public void Abort()
        {
            var raise = false;
            lock (_syncRoot)
            {
                if (_state == ComputationState.Running)
                {
                    _state = ComputationState.PendingAbortion;
                    Services.CancelManager.Cancel();
                    raise = true;
                }
            }
            if (raise)
            {
                OnStateChanged(EventArgs.Empty);
            }
        }

        public void Compute()
        {
            BeginComputation();
            Initialize();
            try
            {
                InternalCompute();
            }
            finally
            {
                Clean();
            }
            EndComputation();
        }

        public T GetService<T>()
            where T : IService
        {
            T service;
            if (!TryGetService(out service))
            {
                throw new InvalidOperationException("service not found");
            }
            return service;
        }

        public bool TryGetService<T>(out T service)
            where T : IService
        {
            object serviceObject;
            if (TryGetService(typeof(T), out serviceObject))
            {
                service = (T)serviceObject;
                return true;
            }

            service = default(T);
            return false;
        }

        protected void BeginComputation()
        {
            Contract.Requires(State == ComputationState.NotRunning);
            lock (_syncRoot)
            {
                _state = ComputationState.Running;
                Services.CancelManager.ResetCancel();
                OnStarted(EventArgs.Empty);
                OnStateChanged(EventArgs.Empty);
            }
        }

        protected virtual void Clean()
        {
        }

        protected void EndComputation()
        {
            Contract.Requires(
                State == ComputationState.Running ||
                State == ComputationState.Aborted);
            lock (_syncRoot)
            {
                switch (_state)
                {
                    case ComputationState.Running:
                        _state = ComputationState.Finished;
                        OnFinished(EventArgs.Empty);
                        break;
                    case ComputationState.PendingAbortion:
                        _state = ComputationState.Aborted;
                        OnAborted(EventArgs.Empty);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
                Services.CancelManager.ResetCancel();
                OnStateChanged(EventArgs.Empty);
            }
        }

        protected virtual void Initialize()
        {
        }

        protected abstract void InternalCompute();

        protected virtual void OnAborted(EventArgs e)
        {
            var eh = Aborted;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        protected virtual void OnFinished(EventArgs e)
        {
            var eh = Finished;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        protected virtual void OnStarted(EventArgs e)
        {
            var eh = Started;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        protected virtual void OnStateChanged(EventArgs e)
        {
            var eh = StateChanged;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        protected virtual bool TryGetService(Type serviceType, out object service)
        {
            Contract.Requires(serviceType != null);
            lock (SyncRoot)
            {
                if (_services == null)
                {
                    _services = new Dictionary<Type, object>();
                }
                if (!_services.TryGetValue(serviceType, out service))
                {
                    if (serviceType == typeof(ICancelManager))
                    {
                        _services[serviceType] = service = new CancelManager();
                    }
                    else
                    {
                        _services[serviceType] = service = null;
                    }
                }

                return service != null;
            }
        }

        public event EventHandler Aborted;

        public event EventHandler Finished;

        public event EventHandler Started;

        public event EventHandler StateChanged;
    }
}