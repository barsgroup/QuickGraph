//// <copyright file="ArrayAdjacencyGraphTVertexTEdgeTest.cs" company="Jonathan de Halleux">Copyright http://quickgraph.codeplex.com/</copyright>

//namespace QuickGraph
//{
//    using System;
//    using System.Collections.Generic;

//    using QuickGraph.Serialization;

//    using Xunit;

//    /// <summary>This class contains parameterized unit tests for ArrayAdjacencyGraph`2</summary>
//    [PexClass(typeof(ArrayAdjacencyGraph<,>))]
//    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
//    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
//    [PexGenericArguments(typeof(int), typeof(SEdge<int>))]
//    public class ArrayAdjacencyGraphTVertexTEdgeTest
//    {
//        /// <summary>Test stub for AllowParallelEdges</summary>
//        public void AllowParallelEdgesGet<TVertex, TEdge>([PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.AllowParallelEdgesGet(ArrayAdjacencyGraph`2<!!0,!!1>)
//            var result = target.AllowParallelEdges;
//        }

//        /// <summary>Test stub for ContainsEdge(!0, !0)</summary>
//        public bool ContainsEdge<TVertex, TEdge>(
//            [PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target01,
//            TVertex source,
//            TVertex target
//        )
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.ContainsEdge(ArrayAdjacencyGraph`2<!!0,!!1>, !!0, !!0)
//            var result = target01.ContainsEdge(source, target);
//            return result;
//        }

//        /// <summary>Test stub for ContainsEdge(!1)</summary>
//        public bool ContainsEdge01<TVertex, TEdge>(
//            [PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target,
//            TEdge edge
//        )
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.ContainsEdge01(ArrayAdjacencyGraph`2<!!0,!!1>, !!1)
//            var result = target.ContainsEdge(edge);
//            return result;
//        }

//        /// <summary>Test stub for ContainsVertex(!0)</summary>
//        public bool ContainsVertex<TVertex, TEdge>(
//            [PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target,
//            TVertex vertex
//        )
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.ContainsVertex(ArrayAdjacencyGraph`2<!!0,!!1>, !!0)
//            var result = target.ContainsVertex(vertex);
//            return result;
//        }

//        /// <summary>Test stub for EdgeCount</summary>
//        public void EdgeCountGet<TVertex, TEdge>([PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.EdgeCountGet(ArrayAdjacencyGraph`2<!!0,!!1>)
//            var result = target.EdgeCount;
//        }

//        /// <summary>Test stub for Edges</summary>
//        public void EdgesGet<TVertex, TEdge>([PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.EdgesGet(ArrayAdjacencyGraph`2<!!0,!!1>)
//            var result = target.Edges;
//        }

//        /// <summary>Test stub for IsDirected</summary>
//        public void IsDirectedGet<TVertex, TEdge>([PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.IsDirectedGet(ArrayAdjacencyGraph`2<!!0,!!1>)
//            var result = target.IsDirected;
//        }

//        /// <summary>Test stub for IsEdgesEmpty</summary>
//        public void IsEdgesEmptyGet<TVertex, TEdge>([PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.IsEdgesEmptyGet(ArrayAdjacencyGraph`2<!!0,!!1>)
//            var result = target.IsEdgesEmpty;
//        }

//        /// <summary>Test stub for IsOutEdgesEmpty(!0)</summary>
//        public bool IsOutEdgesEmpty<TVertex, TEdge>(
//            [PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target,
//            TVertex v
//        )
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.IsOutEdgesEmpty(ArrayAdjacencyGraph`2<!!0,!!1>, !!0)
//            var result = target.IsOutEdgesEmpty(v);
//            return result;
//        }

//        /// <summary>Test stub for IsVerticesEmpty</summary>
//        public void IsVerticesEmptyGet<TVertex, TEdge>([PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.IsVerticesEmptyGet(ArrayAdjacencyGraph`2<!!0,!!1>)
//            var result = target.IsVerticesEmpty;
//        }

//        /// <summary>Test stub for OutDegree(!0)</summary>
//        public int OutDegree<TVertex, TEdge>(
//            [PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target,
//            TVertex v
//        )
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.OutDegree(ArrayAdjacencyGraph`2<!!0,!!1>, !!0)
//            var result = target.OutDegree(v);
//            return result;
//        }

//        /// <summary>Test stub for OutEdge(!0, Int32)</summary>
//        public TEdge OutEdge<TVertex, TEdge>(
//            [PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target,
//            TVertex v,
//            int index
//        )
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.OutEdge(ArrayAdjacencyGraph`2<!!0,!!1>, !!0, Int32)
//            var result = target.OutEdge(v, index);
//            return result;
//        }

//        /// <summary>Test stub for OutEdges(!0)</summary>
//        public IEnumerable<TEdge> OutEdges<TVertex, TEdge>(
//            [PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target,
//            TVertex v
//        )
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.OutEdges(ArrayAdjacencyGraph`2<!!0,!!1>, !!0)
//            var result = target.OutEdges(v);
//            return result;
//        }

//        public void SameEdgeCount<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g)
//            where TEdge : IEdge<TVertex>
//        {
//            var ag = g.ToArrayAdjacencyGraph();
//            Assert.Equal(g.EdgeCount, ag.EdgeCount);
//        }

//        [Fact]
//        public void SameEdgeCountAll()
//        {
//            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
//                SameEdgeCount(g);
//        }

//        public void SameEdges<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g)
//            where TEdge : IEdge<TVertex>
//        {
//            var ag = g.ToArrayAdjacencyGraph();
//            PexAssert.AreElementsEqual(g.Edges, ag.Edges, (l, r) => l.Equals(r));
//        }

//        [Fact]
//        public void SameEdgesAll()
//        {
//            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
//                SameEdges(g);
//        }

//        public void SameOutEdges<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g)
//            where TEdge : IEdge<TVertex>
//        {
//            var ag = g.ToArrayAdjacencyGraph();
//            foreach (var v in g.Vertices)
//                PexAssert.AreElementsEqual(g.OutEdges(v), ag.OutEdges(v), (l, r) => l.Equals(r));
//        }

//        [Fact]
//        public void SameOutEdgesAll()
//        {
//            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
//                SameOutEdges(g);
//        }

//        public void SameVertexCount<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g)
//            where TEdge : IEdge<TVertex>
//        {
//            var ag = g.ToArrayAdjacencyGraph();
//            Assert.Equal(g.VertexCount, ag.VertexCount);
//        }

//        [Fact]
//        public void SameVertexCountAll()
//        {
//            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
//                SameVertexCount(g);
//        }

//        public void SameVertices<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g)
//            where TEdge : IEdge<TVertex>
//        {
//            var ag = g.ToArrayAdjacencyGraph();
//            PexAssert.AreElementsEqual(g.Vertices, ag.Vertices, (l, r) => l.Equals(r));
//        }

//        [Fact]
//        public void SameVerticesAll()
//        {
//            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
//                SameVertices(g);
//        }

//        /// <summary>Test stub for TryGetEdge(!0, !0, !1&amp;)</summary>
//        public bool TryGetEdge<TVertex, TEdge>(
//            [PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target01,
//            TVertex source,
//            TVertex target,
//            out TEdge edge
//        )
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.TryGetEdge(ArrayAdjacencyGraph`2<!!0,!!1>, !!0, !!0, !!1&)
//            var result = target01.TryGetEdge(source, target, out edge);
//            return result;
//        }

//        /// <summary>Test stub for TryGetEdges(!0, !0, IEnumerable`1&lt;!1&gt;&amp;)</summary>
//        public bool TryGetEdges<TVertex, TEdge>(
//            [PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target01,
//            TVertex source,
//            TVertex target,
//            out IEnumerable<TEdge> edges
//        )
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.TryGetEdges(ArrayAdjacencyGraph`2<!!0,!!1>, !!0, !!0, IEnumerable`1<!!1>&)
//            var result = target01.TryGetEdges(source, target, out edges);
//            return result;
//        }

//        /// <summary>Test stub for TryGetOutEdges(!0, IEnumerable`1&lt;!1&gt;&amp;)</summary>
//        public bool TryGetOutEdges<TVertex, TEdge>(
//            [PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target,
//            TVertex v,
//            out IEnumerable<TEdge> edges
//        )
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.TryGetOutEdges(ArrayAdjacencyGraph`2<!!0,!!1>, !!0, IEnumerable`1<!!1>&)
//            var result = target.TryGetOutEdges(v, out edges);
//            return result;
//        }

//        /// <summary>Test stub for VertexCount</summary>
//        public void VertexCountGet<TVertex, TEdge>([PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.VertexCountGet(ArrayAdjacencyGraph`2<!!0,!!1>)
//            var result = target.VertexCount;
//        }

//        /// <summary>Test stub for Vertices</summary>
//        public void VerticesGet<TVertex, TEdge>([PexAssumeUnderTest] ArrayAdjacencyGraph<TVertex, TEdge> target)
//            where TEdge : IEdge<TVertex>
//        {
//            // TODO: add assertions to method ArrayAdjacencyGraphTVertexTEdgeTest.VerticesGet(ArrayAdjacencyGraph`2<!!0,!!1>)
//            var result = target.Vertices;
//        }
//    }
//}

