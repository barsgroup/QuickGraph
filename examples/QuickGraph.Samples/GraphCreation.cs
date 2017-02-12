namespace QuickGraph.Samples
{
    using System;
    using System.Linq;

    using QuickGraph.Algorithms;

    using Xunit;

    public class GraphCreation
    {
        [Fact]
        public void DelegateGraph()
        {
            // a simple adjacency graph representation
            var graph = new int[5][];
            graph[0] = new[] { 1 };
            graph[1] = new[] { 2, 3 };
            graph[2] = new[] { 3, 4 };
            graph[3] = new[] { 4 };
            graph[4] = new int[] { };

            // interoping with quickgraph
            var g = Enumerable.Range(0, graph.Length).ToDelegateVertexAndEdgeListGraph(v => graph[v].Select(w => new SEquatableEdge<int>(v, w)));

            // it's ready to use!
            foreach (var v in g.TopologicalSort())
                Console.WriteLine(v);
        }

        [Fact]
        public void EdgeArrayToAdjacencyGraph()
        {
            var edges = new[]
                        {
                            new SEdge<int>(1, 2),
                            new SEdge<int>(0, 1)
                        };
            var graph = edges.ToAdjacencyGraph<int, SEdge<int>>();
        }
    }
}