namespace QuickGraph.Tests.Regression
{
    using System.Collections.Generic;

    using QuickGraph.Algorithms;

    using Xunit;

    public class ShortestPathBellmanFordTest
    {
        [Fact]
        public void Repro12901()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var vertex = 1;
            graph.AddVerticesAndEdge(new Edge<int>(vertex, vertex));
            var pathFinder = graph.ShortestPathsBellmanFord(edge => -1.0, vertex);
            IEnumerable<Edge<int>> path;
            pathFinder(vertex, out path);
        }
    }
}