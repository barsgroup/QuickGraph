namespace QuickGraph.Algorithms.TopologicalSort
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Search;

    public sealed class TopologicalSortAlgorithm<TVertex, TEdge> :
        AlgorithmBase<IVertexListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        public IList<TVertex> SortedVertices { get; private set; } = new List<TVertex>();

        public bool AllowCyclicGraph { get; } = false;

        public TopologicalSortAlgorithm(IVertexListGraph<TVertex, TEdge> g)
            : this(g, new List<TVertex>())
        {
        }

        public TopologicalSortAlgorithm(
            IVertexListGraph<TVertex, TEdge> g,
            IList<TVertex> vertices)
            : base(g)
        {
            Contract.Requires(vertices != null);

            SortedVertices = vertices;
        }

        public void Compute(IList<TVertex> vertices)
        {
            SortedVertices = vertices;
            SortedVertices.Clear();
            Compute();
        }

        protected override void InternalCompute()
        {
            DepthFirstSearchAlgorithm<TVertex, TEdge> dfs = null;
            try
            {
                dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    new Dictionary<TVertex, GraphColor>(VisitedGraph.VertexCount)
                );
                dfs.BackEdge += BackEdge;
                dfs.FinishVertex += FinishVertex;

                dfs.Compute();
            }
            finally
            {
                if (dfs != null)
                {
                    dfs.BackEdge -= BackEdge;
                    dfs.FinishVertex -= FinishVertex;
                }
            }
        }

        private void BackEdge(TEdge args)
        {
            if (!AllowCyclicGraph)
            {
                throw new NonAcyclicGraphException();
            }
        }

        private void FinishVertex(TVertex v)
        {
            SortedVertices.Insert(0, v);
        }
    }
}