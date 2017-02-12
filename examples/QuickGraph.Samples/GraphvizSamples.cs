namespace QuickGraph.Samples
{
    using System;

    using QuickGraph.Graphviz;

    using Xunit;

    public class GraphvizSamples
    {
        [Fact]
        public void RenderGraphWithGraphviz()
        {
            var edges = new[]
                        {
                            new SEdge<int>(1, 2),
                            new SEdge<int>(0, 1),
                            new SEdge<int>(0, 3),
                            new SEdge<int>(2, 3)
                        };
            var graph = edges.ToAdjacencyGraph<int, SEdge<int>>();
            Console.WriteLine(graph.ToGraphviz());
        }
    }
}