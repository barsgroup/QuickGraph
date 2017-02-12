namespace QuickGraph.Tests.Serialization
{
    using System;
    using System.Diagnostics;

    using QuickGraph.Serialization;

    using Xunit;

    public class DirectedGraphMlExtensionsTest
    {
        [Fact]
        public void SimpleGraph()
        {
            int[][] edges =
            {
                new[] { 1, 2, 3 },
                new[] { 2, 3, 1 }
            };
            edges.ToAdjacencyGraph()
                 .ToDirectedGraphMl()
                 .WriteXml("simple.dgml");

            if (Debugger.IsAttached)
            {
                Process.Start("simple.dgml");
            }

            edges.ToAdjacencyGraph()
                 .ToDirectedGraphMl()
                 .WriteXml(Console.Out);
        }

        [Fact]
        public void ToDirectedGraphMl()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
            {
                var dg = g.ToDirectedGraphMl();
                Assert.NotNull(g);
                Assert.Equal(dg.Nodes.Length, g.VertexCount);
                Assert.Equal(dg.Links.Length, g.EdgeCount);
            }
        }
    }
}