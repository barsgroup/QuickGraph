namespace QuickGraph.Algorithms.Search
{
    using System.Threading.Tasks;

    using QuickGraph.Serialization;

    using Xunit;

    public class BidirectionalDepthFirstSearchAlgorithmTest
    {
        public void Compute<TVertex, TEdge>(IBidirectionalGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var dfs = new BidirectionalDepthFirstSearchAlgorithm<TVertex, TEdge>(g);
            dfs.Compute();

            // let's make sure
            foreach (var v in g.Vertices)
            {
                Assert.True(dfs.VertexColors.ContainsKey(v));
                Assert.Equal(dfs.VertexColors[v], GraphColor.Black);
            }
        }

        [Fact]
        public void ComputeAll()
        {
            Parallel.ForEach(TestGraphFactory.GetBidirectionalGraphs(), Compute);
        }
    }
}