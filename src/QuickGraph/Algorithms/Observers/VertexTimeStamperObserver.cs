namespace QuickGraph.Algorithms.Observers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary></summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    /// <reference-ref
    ///     idref="boost" />
    public sealed class VertexTimeStamperObserver<TVertex, TEdge> :
        IObserver<IVertexTimeStamperAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<TVertex, int> _discoverTimes;

        private readonly Dictionary<TVertex, int> _finishTimes;

        private int _currentTime;

        public IDictionary<TVertex, int> DiscoverTimes => _discoverTimes;

        public IDictionary<TVertex, int> FinishTimes => _finishTimes;

        public VertexTimeStamperObserver()
            : this(new Dictionary<TVertex, int>(), new Dictionary<TVertex, int>())
        {
        }

        public VertexTimeStamperObserver(Dictionary<TVertex, int> discoverTimes)
        {
            Contract.Requires(discoverTimes != null);

            _discoverTimes = discoverTimes;
            _finishTimes = null;
        }

        public VertexTimeStamperObserver(
            Dictionary<TVertex, int> discoverTimes,
            Dictionary<TVertex, int> finishTimes)
        {
            Contract.Requires(discoverTimes != null);
            Contract.Requires(finishTimes != null);

            _discoverTimes = discoverTimes;
            _finishTimes = finishTimes;
        }

        public IDisposable Attach(IVertexTimeStamperAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.DiscoverVertex += DiscoverVertex;
            if (_finishTimes != null)
            {
                algorithm.FinishVertex += FinishVertex;
            }

            return new DisposableAction(
                () =>
                {
                    algorithm.DiscoverVertex -= DiscoverVertex;
                    if (_finishTimes != null)
                    {
                        algorithm.FinishVertex -= FinishVertex;
                    }
                });
        }

        private void DiscoverVertex(TVertex v)
        {
            _discoverTimes[v] = _currentTime++;
        }

        private void FinishVertex(TVertex v)
        {
            _finishTimes[v] = _currentTime++;
        }
    }
}