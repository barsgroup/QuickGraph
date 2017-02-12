//// <copyright file="ForestDisjointSetTTest.cs" company="Jonathan de Halleux">Copyright http://quickgraph.codeplex.com/</copyright>

//namespace QuickGraph.Collections
//{
//    using System;
//    using System.Collections.Generic;

//    using Xunit;

//    /// <summary>This class contains parameterized unit tests for ForestDisjointSet`1</summary>
//    [PexClass(typeof(ForestDisjointSet<>))]
//    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
//    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
//    public partial class ForestDisjointSetTTest
//    {
//        [PexMethod(MaxConstraintSolverTime = 2)]
//        public void Unions(int elementCount, KeyValuePair<int, int>[] unions)
//        {
//            PexAssume.True(0 < elementCount);
//            PexSymbolicValue.Minimize(elementCount);
//            PexAssume.TrueForAll(
//                unions,
//                u => 0 <= u.Key && u.Key < elementCount &&
//                     0 <= u.Value && u.Value < elementCount
//            );

//            var target = new ForestDisjointSet<int>();

//            // fill up with 0..elementCount - 1
//            for (var i = 0; i < elementCount; i++)
//            {
//                target.MakeSet(i);
//                Assert.True(target.Contains(i));
//                Assert.Equal(i + 1, target.ElementCount);
//                Assert.Equal(i + 1, target.SetCount);
//            }

//            // apply Union for pairs unions[i], unions[i+1]
//            for (var i = 0; i < unions.Length; i++)
//            {
//                var left = unions[i].Key;
//                var right = unions[i].Value;

//                var setCount = target.SetCount;
//                var unioned = target.Union(left, right);

//                // should be in the same set now
//                Assert.True(target.AreInSameSet(left, right));

//                // if unioned, the count decreased by 1
//                PexAssert.ImpliesIsTrue(unioned, () => setCount - 1 == target.SetCount);
//            }
//        }
//    }
//}

