namespace QuickGraph.Tests.Algorithms
{
    using System.Collections.Generic;
    using System.Linq;

    using QuickGraph.Algorithms;

    using Xunit;

    public class AlgorithmExtensionsTest
    {
        [Fact]
        public void AdjacencyGraphRoots()
        {
            var g = new AdjacencyGraph<string, Edge<string>>();
            g.AddVertex("A");
            g.AddVertex("B");
            g.AddVertex("C");

            g.AddEdge(new Edge<string>("A", "B"));
            g.AddEdge(new Edge<string>("B", "C"));

            var roots = g.Roots().ToList();
            Assert.Equal(1, roots.Count);
            Assert.Equal("A", roots[0]);
        }

        //[Fact]
        //public void AllAdjacencyGraphRoots()
        //{
        //    foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
        //        Roots(g);
        //}

        public void Roots<T>(IVertexAndEdgeListGraph<T, Edge<T>> g)
        {
            var roots = new HashSet<T>(g.Roots());
            foreach (var edge in g.Edges)
                Assert.False(roots.Contains(edge.Target));
        }
    }
}