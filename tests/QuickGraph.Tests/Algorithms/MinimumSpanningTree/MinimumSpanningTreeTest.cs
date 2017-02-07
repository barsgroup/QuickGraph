﻿namespace QuickGraph.Tests.Algorithms.MinimumSpanningTree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;

    using QuickGraph.Algorithms;
    using QuickGraph.Algorithms.MinimumSpanningTree;
    using QuickGraph.Algorithms.Observers;
    using QuickGraph.Tests.Traits;

    using Xunit;

    public class MinimumSpanningTreeTest
    {
        public double CompareRoot<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (var e in g.Edges)
                distances[e] = g.AdjacentDegree(e.Source) + 1;

            var prim = new List<TEdge>(g.MinimumSpanningTreePrim(e => distances[e]));
            var kruskal = new List<TEdge>(g.MinimumSpanningTreeKruskal(e => distances[e]));

            var primCost = prim.Sum(e => distances[e]);
            var kruskalCost = kruskal.Sum(e => distances[e]);
            TestConsole.WriteLine("prim cost: {0}", primCost);
            TestConsole.WriteLine("kruskal cost: {0}", kruskalCost);
            if (primCost != kruskalCost)
            {
                GraphConsoleSerializer.DisplayGraph(g);
                TestConsole.WriteLine("prim: {0}", string.Join(", ", prim.Select(e => e.ToString() + ':' + distances[e])));
                TestConsole.WriteLine("krus: {0}", string.Join(", ", kruskal.Select(e => e.ToString() + ':' + distances[e])));
                Assert.False(true, "cost do not match");
            }

            return kruskalCost;
        }

        public void Kruskal<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (var e in g.Edges)
                distances[e] = g.AdjacentDegree(e.Source) + 1;

            var kruskal = new KruskalMinimumSpanningTreeAlgorithm<TVertex, TEdge>(g, e => distances[e]);
            AssertMinimumSpanningTree(g, kruskal);
        }

        //[Fact]
        //public void KruskalMinimumSpanningTreeAll()
        //{
        //    Parallel.ForEach(
        //        TestGraphFactory.GetUndirectedGraphs(),
        //        g =>
        //            Kruskal(g));
        //}

        public void Prim<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (var e in g.Edges)
                distances[e] = g.AdjacentDegree(e.Source) + 1;

            var edges = AlgorithmExtensions.MinimumSpanningTreePrim(g, e => distances[e]);
            AssertSpanningTree(g, edges);
        }

        [Fact]
        [WorkItem(12240)]
        public void Prim12240()
        {
            var g = new UndirectedGraph<int, Edge<int>>();

            // (1,2), (3,2),(3,4),(1,4)
            g.AddVerticesAndEdge(new Edge<int>(1, 2));
            g.AddVerticesAndEdge(new Edge<int>(3, 2));
            g.AddVerticesAndEdge(new Edge<int>(3, 4));
            g.AddVerticesAndEdge(new Edge<int>(1, 4));

            var cost = CompareRoot(g);
            Assert.Equal(9, cost);
        }

        [Fact]
        [WorkItem(12240)]
        public void Prim12240WithDelegate()
        {
            var vertices = new[] { 1, 2, 3, 4 };
            var g = vertices.ToDelegateUndirectedGraph(
                delegate(int v, out IEnumerable<EquatableEdge<int>> ov)
                {
                    switch (v)
                    {
                        case 1:
                            ov = new[] { new EquatableEdge<int>(1, 2), new EquatableEdge<int>(1, 4) };
                            break;
                        case 2:
                            ov = new[] { new EquatableEdge<int>(1, 2), new EquatableEdge<int>(3, 2) };
                            break;
                        case 3:
                            ov = new[] { new EquatableEdge<int>(3, 2), new EquatableEdge<int>(3, 4) };
                            break;
                        case 4:
                            ov = new[] { new EquatableEdge<int>(1, 4), new EquatableEdge<int>(3, 4) };
                            break;
                        default:
                            ov = null;
                            break;
                    }
                    return ov != null;
                });
            var cost = CompareRoot(g);
            Assert.Equal(9, cost);
        }

        //[Fact]
        //[WorkItem(12273)]
        //public void Prim12273()
        //{
        //    //  var doc = new XPathDocument("repro12273.xml");

        //    //var ug = doc.DeserializeFromXml(
        //    //    "graph", "node", "edge",
        //    //    nav => new UndirectedGraph<string, TaggedEdge<string, double>>(),
        //    //    nav => nav.GetAttribute("id", ""),
        //    //    nav => new TaggedEdge<string, double>(
        //    //        nav.GetAttribute("source", ""),
        //    //        nav.GetAttribute("target", ""),
        //    //        int.Parse(nav.GetAttribute("weight", ""))
        //    //        )
        //    //    );
        //    var ug = XmlReader.Create("repro12273.xml").DeserializeFromXml(
        //        "graph",
        //        "node",
        //        "edge",
        //        "",
        //        reader => new UndirectedGraph<string, TaggedEdge<string, double>>(),
        //        reader => reader.GetAttribute("id"),
        //        reader => new TaggedEdge<string, double>(
        //            reader.GetAttribute("source"),
        //            reader.GetAttribute("target"),
        //            int.Parse(reader.GetAttribute("weight"))
        //        )
        //    );

        //    //MsaglGraphExtensions.ShowMsaglGraph(ug);
        //    var prim = ug.MinimumSpanningTreePrim(e => e.Tag).ToList();
        //    var pcost = prim.Sum(e => e.Tag);
        //    TestConsole.WriteLine("prim cost {0}", pcost);
        //    foreach (var e in prim)
        //        TestConsole.WriteLine(e);

        //    var kruskal = ug.MinimumSpanningTreeKruskal(e => e.Tag).ToList();
        //    var kcost = kruskal.Sum(e => e.Tag);
        //    TestConsole.WriteLine("kruskal cost {0}", kcost);
        //    foreach (var e in kruskal)
        //        TestConsole.WriteLine(e);

        //    Assert.Equal(pcost, 63);
        //    Assert.Equal(pcost, kcost);
        //}

        //[Fact]
        //public void PrimKruskalMinimumSpanningTreeAll()
        //{
        //    Parallel.ForEach(
        //        TestGraphFactory.GetUndirectedGraphs(),
        //        g =>
        //            CompareRoot(g));
        //}

        //[Fact]
        //public void PrimMinimumSpanningTreeAll()
        //{
        //    Parallel.ForEach(
        //        TestGraphFactory.GetUndirectedGraphs(),
        //        g =>
        //            Prim(g));
        //}

        private static void AssertAreEqual<TVertex, TEdge>(
            IDictionary<TVertex, TEdge> left,
            IDictionary<TVertex, TEdge> right)
            where TEdge : IEdge<TVertex>
        {
            try
            {
                Assert.Equal(left.Count, right.Count);
                foreach (var kv in left)
                    Assert.Equal(kv.Value, right[kv.Key]);
            }
            catch (Exception ex)
            {
                TestConsole.WriteLine("Count: {0} - {1}", left.Count, right.Count);
                foreach (var kv in left)
                {
                    TEdge e;
                    TestConsole.WriteLine(
                        "{0} - {1}",
                        kv.Value,
                        right.TryGetValue(kv.Key, out e)
                            ? e.ToString()
                            : "missing");
                }

                throw new Exception("comparison failed", ex);
            }
        }

        private static void AssertMinimumSpanningTree<TVertex, TEdge>(
            IUndirectedGraph<TVertex, TEdge> g,
            IMinimumSpanningTreeAlgorithm<TVertex, TEdge> algorithm)
            where TEdge : IEdge<TVertex>
        {
            var edgeRecorder = new EdgeRecorderObserver<TVertex, TEdge>();
            using (edgeRecorder.Attach(algorithm))
            {
                algorithm.Compute();
            }

            TestConsole.WriteLine("tree cost: {0}", edgeRecorder.Edges.Count);
            AssertSpanningTree(g, edgeRecorder.Edges);
        }

        private static void AssertSpanningTree<TVertex, TEdge>(
            IUndirectedGraph<TVertex, TEdge> g,
            IEnumerable<TEdge> tree)
            where TEdge : IEdge<TVertex>
        {
            var spanned = new Dictionary<TVertex, TEdge>();
            TestConsole.WriteLine("tree:");
            foreach (var e in tree)
            {
                TestConsole.WriteLine("\t{0}", e);
                spanned[e.Source] = spanned[e.Target] = default(TEdge);
            }

            // find vertices that are connected to some edge
            var treeable = new Dictionary<TVertex, TEdge>();
            foreach (var e in g.Edges)
                treeable[e.Source] = treeable[e.Target] = e;

            // ensure they are in the tree
            foreach (var v in treeable.Keys)
                Assert.True(spanned.ContainsKey(v), $"{v} not in tree");
        }

        private static double Cost<TVertex, TEdge>(IDictionary<TVertex, TEdge> tree)
        {
            return tree.Count;
        }
    }
}