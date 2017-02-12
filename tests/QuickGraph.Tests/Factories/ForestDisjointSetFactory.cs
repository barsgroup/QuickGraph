//// <copyright file="ForestDisjointSetFactory.cs" company="MSIT">Copyright © MSIT 2007</copyright>

//namespace QuickGraph.Collections
//{
//    public static class ForestDisjointSetFactory
//    {
//        [PexFactoryMethod(typeof(ForestDisjointSet<int>))]
//        public static ForestDisjointSet<int> Create(int[] elements, int[] unions)
//        {
//            var ds = new ForestDisjointSet<int>();
//            for (var i = 0; i < elements.Length; ++i)
//                ds.MakeSet(i);
//            for (var i = 0; i + 1 < unions.Length; i += 2)
//            {
//                PexAssume.True(ds.Contains(unions[i]));
//                PexAssume.True(ds.Contains(unions[i + 1]));
//                ds.Union(unions[i], unions[i + 1]);
//            }
//            return ds;
//        }
//    }
//}

