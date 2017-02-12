namespace QuickGraph.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.XPath;

    using Xunit;

    public static class TestGraphFactory
    {
        public static IEnumerable<AdjacencyGraph<string, Edge<string>>> GetAdjacencyGraphs()
        {
            yield return new AdjacencyGraph<string, Edge<string>>();
            foreach (var graphmlFile in GetFileNames())
            {
                var g = LoadAdjacencyGraph(graphmlFile);
                yield return g;
            }
        }

        public static IEnumerable<BidirectionalGraph<string, Edge<string>>> GetBidirectionalGraphs()
        {
            yield return new BidirectionalGraph<string, Edge<string>>();
            foreach (var graphmlFile in GetFileNames())
            {
                var g = LoadBidirectionalGraph(graphmlFile);
                yield return g;
            }
        }

        public static IEnumerable<string> GetFileNames()
        {
            var list = new List<string>();
            list.AddRange(Directory.GetFiles("./netcoreapp1.1/GraphML", "g.*.graphml"));
            if (Directory.Exists("graphml"))
            {
                list.AddRange(Directory.GetFiles("graphml", "g.*.graphml"));
            }
            return list;
        }

        public static IEnumerable<UndirectedGraph<string, Edge<string>>> GetUndirectedGraphs()
        {
            yield return new UndirectedGraph<string, Edge<string>>();
            foreach (var g in GetAdjacencyGraphs())
            {
                var ug = new UndirectedGraph<string, Edge<string>>();
                ug.AddVerticesAndEdgeRange(g.Edges);
                yield return ug;
            }
        }

        public static AdjacencyGraph<string, Edge<string>> LoadAdjacencyGraph(string graphmlFile)
        {
            TestConsole.WriteLine(graphmlFile);
            var g = new AdjacencyGraph<string, Edge<string>>();
            LoadGraph(graphmlFile, g);
            return g;
        }

        public static BidirectionalGraph<string, Edge<string>> LoadBidirectionalGraph(string graphmlFile)
        {
            TestConsole.WriteLine(graphmlFile);
            var g = new BidirectionalGraph<string, Edge<string>>();
            LoadGraph(graphmlFile, g);
            return g;
        }

        public static UndirectedGraph<string, Edge<string>> LoadUndirectedGraph(string graphmlFile)
        {
            var g = LoadAdjacencyGraph(graphmlFile);
            var ug = new UndirectedGraph<string, Edge<string>>();
            ug.AddVerticesAndEdgeRange(g.Edges);
            return ug;
        }

        internal static void LoadGraph<TGraph>(string graphmlFile, TGraph g)
            where TGraph : IMutableVertexAndEdgeListGraph<string, Edge<string>>
        {
            using (var stream = new FileStream(graphmlFile, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    g.DeserializeFromGraphMl(reader, id => id, (source, target, id) => new Edge<string>(source, target));
                }
            }
        }
    }

    public class GraphMlSerializerIntegrationTest
    {
        [Fact]
        public void DeserializeFromGraphMlNorth()
        {
            foreach (var graphmlFile in TestGraphFactory.GetFileNames())
            {
                var graph = TestGraphFactory.LoadAdjacencyGraph(graphmlFile);
                Console.Write(": {0} vertices, {1} edges", graph.VertexCount, graph.EdgeCount);

                var vertices = new Dictionary<string, string>();
                foreach (var v in graph.Vertices)
                    vertices.Add(v, v);

                // check all nodes are loaded
                var settings = new XmlReaderSettings
                               {
                                   DtdProcessing = DtdProcessing.Ignore
                               };
                using (var xreader = XmlReader.Create(graphmlFile, settings))
                {
                    var doc = new XPathDocument(xreader);
                    foreach (XPathNavigator node in doc.CreateNavigator().Select("/graphml/graph/node"))
                    {
                        var id = node.GetAttribute("id", "");
                        Assert.True(vertices.ContainsKey(id));
                    }
                    TestConsole.Write(", vertices ok");

                    // check all edges are loaded
                    foreach (XPathNavigator node in doc.CreateNavigator().Select("/graphml/graph/edge"))
                    {
                        var source = node.GetAttribute("source", "");
                        var target = node.GetAttribute("target", "");
                        Assert.True(graph.ContainsEdge(vertices[source], vertices[target]));
                    }
                    TestConsole.Write(", edges ok");
                }
                TestConsole.WriteLine();
            }
        }
    }
}