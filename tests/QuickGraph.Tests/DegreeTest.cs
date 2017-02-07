namespace QuickGraph.Tests
{

    using Xunit;

    public class DegreeTest
    {
        public void DegreeSumEqualsTwiceEdgeCount<TVertex, TEdge>(
            IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var edgeCount = graph.EdgeCount;
            var degCount = 0;
            foreach (var v in graph.Vertices)
                degCount += graph.Degree(v);

            Assert.Equal(edgeCount * 2, degCount);
        }

        //[Fact]
        //public void DegreeSumEqualsTwiceEdgeCountAll()
        //{
        //    foreach (var g in TestGraphFactory.GetBidirectionalGraphs())
        //        DegreeSumEqualsTwiceEdgeCount(g);
        //}

        public void InDegreeSumEqualsEdgeCount<TVertex, TEdge>(
            IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var edgeCount = graph.EdgeCount;
            var degCount = 0;
            foreach (var v in graph.Vertices)
                degCount += graph.InDegree(v);

            Assert.Equal(edgeCount, degCount);
        }

        //[Fact]
        //public void InDegreeSumEqualsEdgeCountAll()
        //{
        //    foreach (var g in TestGraphFactory.GetBidirectionalGraphs())
        //        InDegreeSumEqualsEdgeCount(g);
        //}

        public void OutDegreeSumEqualsEdgeCount<TVertex, TEdge>(
            IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var edgeCount = graph.EdgeCount;
            var degCount = 0;
            foreach (var v in graph.Vertices)
                degCount += graph.OutDegree(v);

            Assert.Equal(edgeCount, degCount);
        }

        //[Fact]
        //public void OutDegreeSumEqualsEdgeCountAll()
        //{
        //    foreach (var g in TestGraphFactory.GetBidirectionalGraphs())
        //        OutDegreeSumEqualsEdgeCount(g);
        //}
    }
}