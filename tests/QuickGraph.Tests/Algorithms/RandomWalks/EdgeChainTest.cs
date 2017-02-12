namespace QuickGraph.Algorithms.RandomWalks
{
    using System.Threading.Tasks;

    using QuickGraph.Algorithms.Observers;
    using QuickGraph.Serialization;

    using Xunit;

    public class EdgeChainTest
    {
        public void Generate<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            foreach (var v in g.Vertices)
            {
                var walker = new RandomWalkAlgorithm<TVertex, TEdge>(g);
                var vis = new EdgeRecorderObserver<TVertex, TEdge>();
                using (vis.Attach(walker))
                {
                    walker.Generate(v);
                }
            }
        }

        [Fact]
        public void GenerateAll()
        {
            Parallel.ForEach(
                TestGraphFactory.GetAdjacencyGraphs(),
                Generate);
        }
    }
}