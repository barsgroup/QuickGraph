namespace QuickGraph.Algorithms.MinimumSpanningTree
{
    using System;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Services;
    using QuickGraph.Collections;

    public sealed class KruskalMinimumSpanningTreeAlgorithm<TVertex, TEdge>
        : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
          ,
          IMinimumSpanningTreeAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly Func<TEdge, double> _edgeWeights;

        public KruskalMinimumSpanningTreeAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> edgeWeights
        )
            : this(null, visitedGraph, edgeWeights)
        {
        }

        public KruskalMinimumSpanningTreeAlgorithm(
            IAlgorithmComponent host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> edgeWeights
        )
            : base(host, visitedGraph)
        {
            Contract.Requires(edgeWeights != null);

            _edgeWeights = edgeWeights;
        }

        protected override void InternalCompute()
        {
            var cancelManager = Services.CancelManager;
            var ds = new ForestDisjointSet<TVertex>(VisitedGraph.VertexCount);
            foreach (var v in VisitedGraph.Vertices)
                ds.MakeSet(v);

            if (cancelManager.IsCancelling)
            {
                return;
            }

            var queue = new BinaryQueue<TEdge, double>(_edgeWeights);
            foreach (var e in VisitedGraph.Edges)
                queue.Enqueue(e);

            if (cancelManager.IsCancelling)
            {
                return;
            }

            while (queue.Count > 0)
            {
                var e = queue.Dequeue();
                OnExamineEdge(e);
                if (!ds.AreInSameSet(e.Source, e.Target))
                {
                    OnTreeEdge(e);
                    ds.Union(e.Source, e.Target);
                }
            }
        }

        private void OnExamineEdge(TEdge edge)
        {
            var eh = ExamineEdge;
            if (eh != null)
            {
                eh(edge);
            }
        }

        private void OnTreeEdge(TEdge edge)
        {
            var eh = TreeEdge;
            if (eh != null)
            {
                eh(edge);
            }
        }

        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        public event EdgeAction<TVertex, TEdge> TreeEdge;
    }
}