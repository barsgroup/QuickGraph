﻿namespace QuickGraph.Tests.Algorithms.ShortestPath
{
    using System.Collections.Generic;
    using System.Linq;

    using QuickGraph.Algorithms;
    using QuickGraph.Serialization;

    using Xunit;

    public class BellmanFordShortestPathTest
    {
        [Fact]
        public void AllSamples()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
            {
                if (g.IsVerticesEmpty)
                {
                    continue;
                }

                var testPath = g.ShortestPathsBellmanFord(e => e.Source.Length - e.Target.Length, g.Vertices.First());
                foreach (var i in g.Vertices)
                {
                    IEnumerable<Edge<string>> es;
                    if (testPath(i, out es))
                    {
                        TestConsole.WriteLine("{0}: {1}", i, es.Count());
                    }
                }
            }
        }

        [Fact]
        public void Sample()
        {
            var testGraph = new AdjacencyGraph<int, Edge<int>>();
            testGraph.AddVerticesAndEdge(new Edge<int>(1, 2));
            testGraph.AddVerticesAndEdge(new Edge<int>(1, 3));
            testGraph.AddVerticesAndEdge(new Edge<int>(3, 4));
            testGraph.AddVerticesAndEdge(new Edge<int>(1, 4));
            var testPath = testGraph.ShortestPathsBellmanFord(e => e.Source - e.Target, 1);
            foreach (var i in testGraph.Vertices)
            {
                IEnumerable<Edge<int>> es;
                if (testPath(i, out es))
                {
                    TestConsole.WriteLine("{0}: {1}", i, es.Count());
                }
            }
        }
    }
}