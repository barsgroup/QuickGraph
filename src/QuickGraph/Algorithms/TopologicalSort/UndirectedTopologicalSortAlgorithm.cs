﻿namespace QuickGraph.Algorithms.TopologicalSort
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Search;

    public sealed class UndirectedTopologicalSortAlgorithm<TVertex, TEdge>
        : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        public IList<TVertex> SortedVertices { get; private set; }

        public bool AllowCyclicGraph { get; set; } = false;

        public UndirectedTopologicalSortAlgorithm(IUndirectedGraph<TVertex, TEdge> g)
            : this(g, new List<TVertex>())
        {
        }

        public UndirectedTopologicalSortAlgorithm(
            IUndirectedGraph<TVertex, TEdge> g,
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
            UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge> dfs = null;
            try
            {
                dfs = new UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge>(
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

        private void BackEdge(object sender, UndirectedEdgeEventArgs<TVertex, TEdge> a)
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