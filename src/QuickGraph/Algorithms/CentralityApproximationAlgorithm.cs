namespace QuickGraph.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Observers;
    using QuickGraph.Algorithms.ShortestPath;

    public sealed class CentralityApproximationAlgorithm<TVertex, TEdge> :
        AlgorithmBase<IVertexListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly IDictionary<TVertex, double> _centralities = new Dictionary<TVertex, double>();

        private readonly DijkstraShortestPathAlgorithm<TVertex, TEdge> _dijkstra;

        private readonly VertexPredecessorRecorderObserver<TVertex, TEdge> _predecessorRecorder;

        public Func<TEdge, double> Distances => _dijkstra.Weights;

        public Random Rand { get; set; } = new Random();

        public int MaxIterationCount { get; set; } = 50;

        public CentralityApproximationAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> distances
        )
            : base(visitedGraph)
        {
            Contract.Requires(distances != null);

            _dijkstra = new DijkstraShortestPathAlgorithm<TVertex, TEdge>(
                VisitedGraph,
                distances,
                DistanceRelaxers.ShortestDistance
            );
            _predecessorRecorder = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            _predecessorRecorder.Attach(_dijkstra);
        }

        protected override void Initialize()
        {
            base.Initialize();
            _centralities.Clear();
            foreach (var v in VisitedGraph.Vertices)
                _centralities.Add(v, 0);
        }

        protected override void InternalCompute()
        {
            if (VisitedGraph.VertexCount == 0)
            {
                return;
            }

            // compute temporary values
            var n = VisitedGraph.VertexCount;
            for (var i = 0; i < MaxIterationCount; ++i)
            {
                var v = RandomGraphFactory.GetVertex(VisitedGraph, Rand);
                _dijkstra.Compute(v);

                foreach (var u in VisitedGraph.Vertices)
                {
                    double d;
                    if (_dijkstra.TryGetDistance(u, out d))
                    {
                        _centralities[u] += n * d / (MaxIterationCount * (n - 1));
                    }
                }
            }

            // update
            foreach (var v in _centralities.Keys)
                _centralities[v] = 1.0 / _centralities[v];
        }
    }
}