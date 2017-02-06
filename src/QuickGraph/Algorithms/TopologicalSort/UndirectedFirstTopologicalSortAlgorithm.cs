namespace QuickGraph.Algorithms.TopologicalSort
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Collections;

    public sealed class UndirectedFirstTopologicalSortAlgorithm<TVertex, TEdge> :
        AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private IList<TVertex> _sortedVertices = new List<TVertex>();

        public ICollection<TVertex> SortedVertices => _sortedVertices;

        public BinaryQueue<TVertex, int> Heap { get; }

        public IDictionary<TVertex, int> Degrees { get; } = new Dictionary<TVertex, int>();

        public bool AllowCyclicGraph { get; set; } = false;

        public UndirectedFirstTopologicalSortAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph
        )
            : base(visitedGraph)
        {
            Heap = new BinaryQueue<TVertex, int>(e => Degrees[e]);
        }

        public void Compute(IList<TVertex> vertices)
        {
            Contract.Requires(vertices != null);

            _sortedVertices = vertices;
            Compute();
        }

        protected override void InternalCompute()
        {
            InitializeInDegrees();
            var cancelManager = Services.CancelManager;

            while (Heap.Count != 0)
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }

                var v = Heap.Dequeue();
                if (Degrees[v] != 0 && !AllowCyclicGraph)
                {
                    throw new NonAcyclicGraphException();
                }

                _sortedVertices.Add(v);
                OnAddVertex(v);

                // update the count of it's adjacent vertices
                foreach (var e in VisitedGraph.AdjacentEdges(v))
                {
                    if (e.Source.Equals(e.Target))
                    {
                        continue;
                    }

                    Degrees[e.Target]--;
                    if (Degrees[e.Target] < 0 && !AllowCyclicGraph)
                    {
                        throw new InvalidOperationException("Degree is negative, and cannot be");
                    }
                    if (Heap.Contains(e.Target))
                    {
                        Heap.Update(e.Target);
                    }
                }
            }
        }

        private void InitializeInDegrees()
        {
            foreach (var v in VisitedGraph.Vertices)
            {
                Degrees.Add(v, VisitedGraph.AdjacentDegree(v));
                Heap.Enqueue(v);
            }
        }

        private void OnAddVertex(TVertex v)
        {
            if (AddVertex != null)
            {
                AddVertex(v);
            }
        }

        public event VertexAction<TVertex> AddVertex;
    }
}