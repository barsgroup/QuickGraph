//namespace QuickGraph
//{
//    using Xunit;

//    public static class EdgeListGraphTest
//    {
//        public static void Count<T, TE>([PexAssumeUnderTest] IEdgeListGraph<T, TE> g)
//            where TE : IEdge<T>
//        {
//            var n = g.EdgeCount;
//            if (n == 0)
//            {
//                Assert.True(g.IsEdgesEmpty);
//            }

//            var i = 0;
//            foreach (var e in g.Edges)
//            {
//                e.ToString();
//                ++i;
//            }
//            Assert.Equal(n, i);
//        }

//        public static void Iteration<T, TE>([PexAssumeUnderTest] IEdgeListGraph<T, TE> g)
//            where TE : IEdge<T>
//        {
//            var n = g.EdgeCount;
//            var i = 0;
//            foreach (var e in g.Edges)
//                ++i;
//        }
//    }
//}