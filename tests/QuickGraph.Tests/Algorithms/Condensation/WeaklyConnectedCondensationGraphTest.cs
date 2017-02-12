namespace QuickGraph.Algorithms.Condensation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using QuickGraph.Serialization;

    using Xunit;

    public class WeaklyConnectedCondensationGraphAlgorithmTest
    {
        [Fact]
        public void WeaklyConnectedCondensatAll()
        {
            Parallel.ForEach(TestGraphFactory.GetAdjacencyGraphs(), WeaklyConnectedCondensate);
        }

        public void WeaklyConnectedCondensate<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var algo = new CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>(g);
            algo.StronglyConnected = false;
            algo.Compute();
            CheckVertexCount(g, algo);
            CheckEdgeCount(g, algo);
            CheckComponentCount(g, algo);
        }

        private void CheckComponentCount<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g,
                                                         CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> algo)
            where TEdge : IEdge<TVertex>
        {
            // check number of vertices = number of storngly connected components
            var components = g.WeaklyConnectedComponents(new Dictionary<TVertex, int>());
            Assert.Equal(components, algo.CondensedGraph.VertexCount);
        }

        private void CheckEdgeCount<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g,
                                                    CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> algo)
            where TEdge : IEdge<TVertex>
        {
            // check edge count
            var count = 0;
            foreach (var edges in algo.CondensedGraph.Edges)
                count += edges.Edges.Count;
            foreach (var vertices in algo.CondensedGraph.Vertices)
                count += vertices.EdgeCount;
            Assert.Equal(g.EdgeCount, count);
        }

        private void CheckVertexCount<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g,
                                                      CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> algo)
            where TEdge : IEdge<TVertex>
        {
            var count = 0;
            foreach (var vertices in algo.CondensedGraph.Vertices)
                count += vertices.VertexCount;
            Assert.Equal(g.VertexCount, count);
        }
    }
}