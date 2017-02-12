//namespace QuickGraph
//{
//    using Xunit;

//    public class UndirectedGraphTest
//    {
//        public static void IsAdjacentEdgesEmpty<T, TE>([PexAssumeUnderTest] IUndirectedGraph<T, TE> g)
//            where TE : IEdge<T>
//        {
//            foreach (var v in g.Vertices)
//                Assert.Equal(
//                    g.IsAdjacentEdgesEmpty(v),
//                    g.AdjacentDegree(v) == 0);
//        }
//    }
//}

