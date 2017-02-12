namespace QuickGraph.Algorithms.ConnectedComponents
{
    using System.Threading.Tasks;

    using QuickGraph.Serialization;

    using Xunit;

    public class WeaklyConnectedComponentsAlgorithmTest
    {
        public void Compute<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var dfs =
                new WeaklyConnectedComponentsAlgorithm<TVertex, TEdge>(g);
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
            foreach (var edge in g.OutEdges(vertex))
                Assert.Equal(dfs.Components[edge.Source], dfs.Components[edge.Target]);
        }

        [Fact]
        public void WeaklyConnectedComponentsAll()
        {
            Parallel.ForEach(TestGraphFactory.GetAdjacencyGraphs(), Compute);
        }
    }
}