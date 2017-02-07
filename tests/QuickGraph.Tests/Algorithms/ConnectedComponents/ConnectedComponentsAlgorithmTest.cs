namespace QuickGraph.Algorithms.ConnectedComponents
{
    using System.Linq;
    using System.Threading.Tasks;


    using Xunit;

    public class ConnectedComponentsAlgorithmTest
    {
        public void Compute<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var dfs = new ConnectedComponentsAlgorithm<TVertex, TEdge>(g);
            dfs.Compute();
            if (g.VertexCount == 0)
            {
                Assert.True(dfs.ComponentCount == 0);
                return;
            }

            Assert.True(0 < dfs.ComponentCount);
            Assert.True(dfs.ComponentCount <= g.VertexCount);
            foreach (var kv in dfs.Components)
            {
                Assert.True(0 <= kv.Value);
                Assert.True(kv.Value < dfs.ComponentCount, $"{kv.Value} < {dfs.ComponentCount}");
            }

            foreach (var vertex in g.Vertices)
            foreach (var edge in g.AdjacentEdges(vertex))
                Assert.Equal(dfs.Components[edge.Source], dfs.Components[edge.Target]);
        }

        //[Fact]
        //[TestCategories(TestCategories.LongRunning)]
        //public void ConnectedComponentsAll()
        //{
        //    Parallel.ForEach(
        //        TestGraphFactory.GetUndirectedGraphs(),
        //        g =>
        //        {
        //            while (g.EdgeCount > 0)
        //            {
        //                Compute(g);
        //                g.RemoveEdge(g.Edges.First());
        //            }
        //        });
        //}
    }
}