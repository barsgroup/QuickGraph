//// <copyright file="GraphExtensionsTest.cs" company="Jonathan de Halleux">Copyright http://quickgraph.codeplex.com/</copyright>

//namespace QuickGraph
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Collections.ObjectModel;
//    using System.Linq;

//    using QuickGraph.Tests;

//    using Xunit;

//    public class TestVertexType
//    {
//        public string Name { get; set; }

//        public TestVertexType(string name)
//        {
//            Name = name;
//        }
//    }

//    /// <summary>This class contains parameterized unit tests for GraphExtensions</summary>
//    //[PexClass(typeof(GraphExtensions))]
//    //[PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
//    //[PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
//    public partial class GraphExtensionsTest
//    {
//        [Fact]
//        public void CollectionOfEdgesToAdjacencyGraph()
//        {
//            var numVertices = 4;
//            var vertices = new Collection<TestVertexType>();
//            for (var i = 0; i < numVertices; i++)
//                vertices.Add(new TestVertexType(i.ToString()));

//            var edges = TestHelper.CreateAllPairwiseEdges(
//                vertices,
//                vertices,
//                (source, target) => new Edge<TestVertexType>(source, target));

//            var adjGraph = edges.ToAdjacencyGraph<TestVertexType, Edge<TestVertexType>>();

//            //Verify that the right number of vertices were created
//            Assert.Equal(numVertices, adjGraph.Vertices.ToList().Count);
//        }

//        [Fact]
//        public void DictionaryToVertexAndEdgeListGraph()
//        {
//            var dic = new Dictionary<int, int[]>();
//            dic.Add(0, new[] { 1, 2 });
//            dic.Add(1, new[] { 2 });
//            dic.Add(2, new int[] { });

//            var graph = dic.ToVertexAndEdgeListGraph(kv => kv.Value.Select(v => new SEquatableEdge<int>(kv.Key, v)));

//            foreach (var kv in dic)
//            {
//                Assert.True(graph.ContainsVertex(kv.Key));
//                foreach (var i in kv.Value)
//                {
//                    Assert.True(graph.ContainsVertex(i));
//                    Assert.True(graph.ContainsEdge(kv.Key, i));
//                }
//            }
//        }

//        /// <summary>Test stub for ToAdjacencyGraph(IEnumerable`1&lt;!!1&gt;, Boolean)</summary>
//        public AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph<TVertex, TEdge>(IEnumerable<TEdge> edges, bool allowParallelEdges)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method GraphExtensionsTest.ToAdjacencyGraph(IEnumerable`1<!!1>, Boolean)
//            var result
//                = edges.ToAdjacencyGraph<TVertex, TEdge>(allowParallelEdges);
//            return result;
//        }

//        /// <summary>Test stub for ToAdjacencyGraph(IEnumerable`1&lt;!!1&gt;)</summary>
//        public AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph01<TVertex, TEdge>(IEnumerable<TEdge> edges)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method GraphExtensionsTest.ToAdjacencyGraph01(IEnumerable`1<!!1>)
//            var result
//                = edges.ToAdjacencyGraph<TVertex, TEdge>();
//            return result;
//        }

//        /// <summary>Test stub for ToAdjacencyGraph(IEnumerable`1&lt;!!0&gt;, Func`2&lt;!!0,IEnumerable`1&lt;!!1&gt;&gt;, Boolean)</summary>
//        public AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph02<TVertex, TEdge>(
//            IEnumerable<TVertex> vertices,
//            Func<TVertex, IEnumerable<TEdge>> outEdgesFactory,
//            bool allowParallelEdges
//        )
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method GraphExtensionsTest.ToAdjacencyGraph02(IEnumerable`1<!!0>, Func`2<!!0,IEnumerable`1<!!1>>, Boolean)
//            var result
//                = vertices.ToAdjacencyGraph
//                    (outEdgesFactory, allowParallelEdges);
//            return result;
//        }

//        /// <summary>Test stub for ToAdjacencyGraph(IEnumerable`1&lt;!!0&gt;, Func`2&lt;!!0,IEnumerable`1&lt;!!1&gt;&gt;)</summary>
//        public AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph03<TVertex, TEdge>(
//            IEnumerable<TVertex> vertices,
//            Func<TVertex, IEnumerable<TEdge>> outEdgesFactory
//        )
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method GraphExtensionsTest.ToAdjacencyGraph03(IEnumerable`1<!!0>, Func`2<!!0,IEnumerable`1<!!1>>)
//            var result
//                = vertices.ToAdjacencyGraph(outEdgesFactory);
//            return result;
//        }

//        ///// <summary>Test stub for ToAdjacencyGraph(IEnumerable`1&lt;SEquatableEdge`1&lt;!!0&gt;&gt;)</summary>
//        //[PexGenericArguments(typeof(int))]
//        //public AdjacencyGraph<TVertex, SEquatableEdge<TVertex>> ToAdjacencyGraph04<TVertex>(IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
//        //{
//        //    // TODO: add assertions to method GraphExtensionsTest.ToAdjacencyGraph04(IEnumerable`1<SEquatableEdge`1<!!0>>)
//        //    var result
//        //        = vertexPairs.ToAdjacencyGraph();
//        //    return result;
//        //}

//        /// <summary>Test stub for ToBidirectionalGraph(IVertexAndEdgeListGraph`2&lt;!!0,!!1&gt;)</summary>
//        public IBidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> graph)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method GraphExtensionsTest.ToBidirectionalGraph(IVertexAndEdgeListGraph`2<!!0,!!1>)
//            var result
//                = graph.ToBidirectionalGraph();
//            return result;
//        }

//        /// <summary>Test stub for ToBidirectionalGraph(IEnumerable`1&lt;!!1&gt;, Boolean)</summary>
//        public BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph01<TVertex, TEdge>(IEnumerable<TEdge> edges, bool allowParallelEdges)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method GraphExtensionsTest.ToBidirectionalGraph01(IEnumerable`1<!!1>, Boolean)
//            var result
//                = edges.ToBidirectionalGraph<TVertex, TEdge>
//                    (allowParallelEdges);
//            return result;
//        }

//        /// <summary>Test stub for ToBidirectionalGraph(IEnumerable`1&lt;!!1&gt;)</summary>
//        public BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph02<TVertex, TEdge>(IEnumerable<TEdge> edges)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method GraphExtensionsTest.ToBidirectionalGraph02(IEnumerable`1<!!1>)
//            var result
//                = edges.ToBidirectionalGraph<TVertex, TEdge>();
//            return result;
//        }

//        /// <summary>
//        ///     Test stub for ToBidirectionalGraph(IEnumerable`1&lt;!!0&gt;, Func`2&lt;!!0,IEnumerable`1&lt;!!1&gt;&gt;,
//        ///     Boolean)
//        /// </summary>
//        public BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph03<TVertex, TEdge>(
//            IEnumerable<TVertex> vertices,
//            Func<TVertex, IEnumerable<TEdge>> outEdgesFactory,
//            bool allowParallelEdges
//        )
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method GraphExtensionsTest.ToBidirectionalGraph03(IEnumerable`1<!!0>, Func`2<!!0,IEnumerable`1<!!1>>, Boolean)
//            var result
//                = vertices.ToBidirectionalGraph
//                    (outEdgesFactory, allowParallelEdges);
//            return result;
//        }

//        /// <summary>Test stub for ToBidirectionalGraph(IEnumerable`1&lt;!!0&gt;, Func`2&lt;!!0,IEnumerable`1&lt;!!1&gt;&gt;)</summary>
//        public BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph04<TVertex, TEdge>(
//            IEnumerable<TVertex> vertices,
//            Func<TVertex, IEnumerable<TEdge>> outEdgesFactory
//        )
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method GraphExtensionsTest.ToBidirectionalGraph04(IEnumerable`1<!!0>, Func`2<!!0,IEnumerable`1<!!1>>)
//            var result
//                = vertices.ToBidirectionalGraph
//                    (outEdgesFactory);
//            return result;
//        }

//        /// <summary>Test stub for ToBidirectionalGraph(IEnumerable`1&lt;SEquatableEdge`1&lt;!!0&gt;&gt;)</summary>
//        [PexGenericArguments(typeof(int))]
//        public BidirectionalGraph<TVertex, SEquatableEdge<TVertex>> ToBidirectionalGraph05<TVertex>(IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
//        {
//            // TODO: add assertions to method GraphExtensionsTest.ToBidirectionalGraph05(IEnumerable`1<SEquatableEdge`1<!!0>>)
//            var result
//                = vertexPairs.ToBidirectionalGraph();
//            return result;
//        }

//        /// <summary>Test stub for ToUndirectedGraph(IEnumerable`1&lt;!!1&gt;)</summary>
//        public UndirectedGraph<TVertex, TEdge> ToUndirectedGraph<TVertex, TEdge>(IEnumerable<TEdge> edges)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method GraphExtensionsTest.ToUndirectedGraph(IEnumerable`1<!!1>)
//            var result
//                = edges.ToUndirectedGraph<TVertex, TEdge>();
//            return result;
//        }

//        /// <summary>Test stub for ToUndirectedGraph(IEnumerable`1&lt;!!1&gt;, Boolean)</summary>
//        public UndirectedGraph<TVertex, TEdge> ToUndirectedGraph01<TVertex, TEdge>(IEnumerable<TEdge> edges, bool allowParralelEdges)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method GraphExtensionsTest.ToUndirectedGraph01(IEnumerable`1<!!1>, Boolean)
//            var result =
//                edges.ToUndirectedGraph<TVertex, TEdge>(allowParralelEdges);
//            return result;
//        }

//        /// <summary>Test stub for ToUndirectedGraph(IEnumerable`1&lt;SEquatableEdge`1&lt;!!0&gt;&gt;)</summary>
//        [PexGenericArguments(typeof(int))]
//        public UndirectedGraph<TVertex, SEquatableEdge<TVertex>> ToUndirectedGraph02<TVertex>(IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
//        {
//            // TODO: add assertions to method GraphExtensionsTest.ToUndirectedGraph02(IEnumerable`1<SEquatableEdge`1<!!0>>)
//            var result
//                = vertexPairs.ToUndirectedGraph();
//            return result;
//        }
//    }
//}

