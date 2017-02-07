//// <copyright file="SoftHeapTKeyTValueTest.cs" company="MSIT">Copyright © MSIT 2008</copyright>

//namespace QuickGraph.Collections
//{
//    using System;

//    using Xunit;

//    /// <summary>This class contains parameterized unit tests for SoftHeap`2</summary>
//    [PexClass(typeof(SoftHeap<,>))]
//    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
//    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
//    public partial class SoftHeapTKeyTValueTest
//    {
//        [PexMethod(MaxBranches = 160000)]
//        public void Add(int[] keys)
//        {
//            PexAssume.TrueForAll(keys, k => k < int.MaxValue);
//            PexAssume.True(keys.Length > 0);

//            var target = new SoftHeap<int, int>(1 / 4.0, int.MaxValue);
//            TestConsole.WriteLine("expected error rate: {0}", target.ErrorRate);
//            foreach (var key in keys)
//            {
//                var count = target.Count;
//                target.Add(key, key + 1);
//                Assert.Equal(count + 1, target.Count);
//            }

//            var lastMin = int.MaxValue;
//            var error = 0;
//            while (target.Count > 0)
//            {
//                var kv = target.DeleteMin();
//                if (lastMin < kv.Key)
//                {
//                    error++;
//                }
//                lastMin = kv.Key;
//                Assert.Equal(kv.Key + 1, kv.Value);
//            }

//            TestConsole.WriteLine("error rate: {0}", error / (double)keys.Length);
//            Assert.True(error / (double)keys.Length <= target.ErrorRate);
//        }
//    }
//}