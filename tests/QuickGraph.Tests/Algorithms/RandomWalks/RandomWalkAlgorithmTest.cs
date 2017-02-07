namespace QuickGraph.Algorithms.RandomWalks
{
    using System.Threading.Tasks;

    using QuickGraph.Algorithms.Observers;

    using Xunit;

    public class RandomWalkAlgorithmTest
    {
        //[Fact]
        //public void RoundRobinAll()
        //{
        //    Parallel.ForEach(
        //        TestGraphFactory.GetAdjacencyGraphs(),
        //        g =>
        //            RoundRobinTest(g));
        //}

        public void RoundRobinTest<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            if (g.VertexCount == 0)
            {
                return;
            }

            foreach (var root in g.Vertices)
            {
                var walker =
                    new RandomWalkAlgorithm<TVertex, TEdge>(g);
                walker.EdgeChain = new NormalizedMarkovEdgeChain<TVertex, TEdge>();
                walker.Generate(root);
            }
        }

        public void RoundRobinTestWithVisitor<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            if (g.VertexCount == 0)
            {
                return;
            }

            foreach (var root in g.Vertices)
            {
                var walker =
                    new RandomWalkAlgorithm<TVertex, TEdge>(g);
                walker.EdgeChain = new NormalizedMarkovEdgeChain<TVertex, TEdge>();

                var vis = new EdgeRecorderObserver<TVertex, TEdge>();
                using (vis.Attach(walker))
                {
                    walker.Generate(root);
                }
            }
        }
    }
}