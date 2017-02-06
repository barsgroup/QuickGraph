namespace QuickGraph.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Services;

    public abstract class RootedAlgorithmBase<TVertex, TGraph>
        : AlgorithmBase<TGraph>
    {
        private bool _hasRootVertex;

        private TVertex _rootVertex;

        protected RootedAlgorithmBase(
            IAlgorithmComponent host,
            TGraph visitedGraph)
            : base(host, visitedGraph)
        {
        }

        public void ClearRootVertex()
        {
            _rootVertex = default(TVertex);
            _hasRootVertex = false;
        }

        public void Compute(TVertex rootVertex)
        {
            Contract.Requires(rootVertex != null);

            SetRootVertex(rootVertex);
            Compute();
        }

        public void SetRootVertex(TVertex rootVertex)
        {
            Contract.Requires(rootVertex != null);

            var changed = Comparer<TVertex>.Default.Compare(_rootVertex, rootVertex) != 0;
            _rootVertex = rootVertex;
            if (changed)
            {
                OnRootVertexChanged(EventArgs.Empty);
            }
            _hasRootVertex = true;
        }

        public bool TryGetRootVertex(out TVertex rootVertex)
        {
            if (_hasRootVertex)
            {
                rootVertex = _rootVertex;
                return true;
            }
            rootVertex = default(TVertex);
            return false;
        }

        protected virtual void OnRootVertexChanged(EventArgs e)
        {
            Contract.Requires(e != null);

            var eh = RootVertexChanged;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        public event EventHandler RootVertexChanged;
    }
}