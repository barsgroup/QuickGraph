namespace QuickGraph.Algorithms.RandomWalks
{
    using System.Threading.Tasks;

    using QuickGraph.Serialization;

    using Xunit;

    public class CyclePoppingRandomTreeAlgorithmTest
    {
        [Fact]
        public void CyclePoppingRandomTreeAll()
        {
            Parallel.ForEach(
                TestGraphFactory.GetAdjacencyGraphs(),
                g =>
                {
                    foreach (var v in g.Vertices)
                    {
                        var target = new CyclePoppingRandomTreeAlgorithm<string, Edge<string>>(g);
                        target.Compute(v);
                    }
                });
        }

        [Fact]
        public void IsolatedVertices()
        {
            var g = new AdjacencyGraph<int, Edge<int>>(true);
            g.AddVertex(0);
            g.AddVertex(1);

            var target = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(g);
            target.RandomTree();
        }

        [Fact]
        public void IsolatedVerticesWithRoot()
        {
            var g = new AdjacencyGraph<int, Edge<int>>(true);
            g.AddVertex(0);
            g.AddVertex(1);

            var target = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(g);
            target.RandomTreeWithRoot(0);
        }

        [Fact]
        public void Repro13160()
        {
            // create a new graph			
            var graph = new BidirectionalGraph<int, SEquatableEdge<int>>(false);

            // adding vertices		    
            for (var i = 0; i < 3; ++i)
            for (var j = 0; j < 3; ++j)
                graph.AddVertex(i * 3 + j);

            // adding Width edges			    
            for (var i = 0; i < 3; ++i)
            for (var j = 0; j < 2; ++j)
                graph.AddEdge(new SEquatableEdge<int>(i * 3 + j, i * 3 + j + 1));

            // adding Length edges			    
            for (var i = 0; i < 2; ++i)
            for (var j = 0; j < 3; ++j)
                graph.AddEdge(new SEquatableEdge<int>(i * 3 + j, (i + 1) * 3 + j));

            // create cross edges 
            foreach (var e in graph.Edges)
                graph.AddEdge(new SEquatableEdge<int>(e.Target, e.Source));

            // breaking graph apart
            for (var i = 0; i < 3; ++i)
            for (var j = 0; j < 3; ++j)
                if (i == 1)
                {
                    graph.RemoveVertex(i * 3 + j);
                }

            var target = new CyclePoppingRandomTreeAlgorithm<int, SEquatableEdge<int>>(graph);
            target.Compute(2);
            foreach (var kv in target.Successors)
                TestConsole.WriteLine("{0}: {1}", kv.Key, kv.Value);
        }

        [Fact]
        public void RootIsNotAccessible()
        {
            var g = new AdjacencyGraph<int, Edge<int>>(true);
            g.AddVertex(0);
            g.AddVertex(1);
            g.AddEdge(new Edge<int>(0, 1));

            var target = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(g);
            target.RandomTreeWithRoot(0);
        }
    }
}