namespace QuickGraph.Tests.Serialization
{
    using System.Xml;
    using System.Xml.XPath;

    using QuickGraph.Serialization;

    using Xunit;

    public class XmlSerializationTest
    {
        [Fact]
        public void DeserializeFromXml()
        {
            var doc = new XPathDocument("netcoreapp1.1/GraphML/repro12273.xml");
            var ug = doc.DeserializeFromXml(
                "graph",
                "node",
                "edge",
                nav => new UndirectedGraph<string, TaggedEdge<string, double>>(),
                nav => nav.GetAttribute("id", ""),
                nav => new TaggedEdge<string, double>(
                    nav.GetAttribute("source", ""),
                    nav.GetAttribute("target", ""),
                    int.Parse(nav.GetAttribute("weight", ""))
                )
            );

            var ug2 = XmlReader.Create("netcoreapp1.1/GraphML/repro12273.xml").DeserializeFromXml(
                "graph",
                "node",
                "edge",
                "",
                reader => new UndirectedGraph<string, TaggedEdge<string, double>>(),
                reader => reader.GetAttribute("id"),
                reader => new TaggedEdge<string, double>(
                    reader.GetAttribute("source"),
                    reader.GetAttribute("target"),
                    int.Parse(reader.GetAttribute("weight"))
                )
            );

            Assert.Equal(ug.VertexCount, ug2.VertexCount);
            Assert.Equal(ug.EdgeCount, ug2.EdgeCount);
        }
    }
}