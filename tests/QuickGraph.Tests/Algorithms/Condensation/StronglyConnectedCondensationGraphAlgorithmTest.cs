namespace QuickGraph.Algorithms.Condensation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    //using QuickGraph.Serialization;

    using Xunit;

    public class StronglyConnectedCondensationGraphAlgorithmTest
    {
        public void StronglyConnectedCondensate<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var cg = g.CondensateStronglyConnected<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>();

            CheckVertexCount(g, cg);
            CheckEdgeCount(g, cg);
            CheckComponentCount(g, cg);
            CheckDag(g, cg);
        }

        //[Fact]
        //public void StronglyConnectedCondensateAll()
        //{
        //    Parallel.ForEach(
        //        TestGraphFactory.GetAdjacencyGraphs(),
        //        g =>
        //            StronglyConnectedCondensate(g));
        //}

        private void CheckComponentCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            // check number of vertices = number of storngly connected components
            IDictionary<TVertex, int> components;
            var componentCount = g.StronglyConnectedComponents(out components);
            Assert.Equal(componentCount, cg.VertexCount);
        }

        private void CheckDag<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            // check it's a dag
            try
            {
                cg.TopologicalSort();
            }
            catch (NonAcyclicGraphException)
            {
                Assert.False(true, "Graph is not a DAG.");
            }
        }

        private void CheckEdgeCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            // check edge count
            var count = 0;
            foreach (var edges in cg.Edges)
                count += edges.Edges.Count;
            foreach (var vertices in cg.Vertices)
                count += vertices.EdgeCount;
            Assert.Equal(g.EdgeCount, count);
        }

        private void CheckVertexCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            var count = 0;
            foreach (var vertices in cg.Vertices)
                count += vertices.VertexCount;

            Assert.Equal(g.VertexCount, count);
        }
    }
}