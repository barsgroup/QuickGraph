namespace QuickGraph.Serialization
{
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Xml;

    using QuickGraph.Algorithms;

    public static class GraphMlExtensions
    {
        public static void DeserializeAndValidateFromGraphMl<TVertex, TEdge, TGraph>(this TGraph graph,
                                                                                     TextReader reader,
                                                                                     IdentifiableVertexFactory<TVertex> vertexFactory,
                                                                                     IdentifiableEdgeFactory<TVertex, TEdge> edgeFactory)
            where TEdge : IEdge<TVertex>
            where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>
        {
            Contract.Requires(graph != null);
            Contract.Requires(reader != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);

            var serializer = new GraphMlDeserializer<TVertex, TEdge, TGraph>();
            var settings = new XmlReaderSettings();

            // add graphxml schema
            //AddGraphMlSchema<TVertex, TEdge, TGraph>(settings);

            // reader and validating
            using (var xreader = XmlReader.Create(reader, settings))
            {
                serializer.Deserialize(xreader, graph, vertexFactory, edgeFactory);
            }
        }

        public static void DeserializeFromGraphMl<TVertex, TEdge, TGraph>(this TGraph graph,
                                                                          TextReader reader,
                                                                          IdentifiableVertexFactory<TVertex> vertexFactory,
                                                                          IdentifiableEdgeFactory<TVertex, TEdge> edgeFactory)
            where TEdge : IEdge<TVertex>
            where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>
        {
            Contract.Requires(graph != null);
            Contract.Requires(reader != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);

            var settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;

            using (var xreader = XmlReader.Create(reader, settings))
            {
                DeserializeFromGraphMl(graph, xreader, vertexFactory, edgeFactory);
            }
        }

        public static void DeserializeFromGraphMl<TVertex, TEdge, TGraph>(this TGraph graph,
                                                                          XmlReader reader,
                                                                          IdentifiableVertexFactory<TVertex> vertexFactory,
                                                                          IdentifiableEdgeFactory<TVertex, TEdge> edgeFactory)
            where TEdge : IEdge<TVertex>
            where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>
        {
            Contract.Requires(graph != null);
            Contract.Requires(reader != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);

            var serializer = new GraphMlDeserializer<TVertex, TEdge, TGraph>();
            serializer.Deserialize(reader, graph, vertexFactory, edgeFactory);
        }

        public static void SerializeToGraphMl<TVertex, TEdge, TGraph>(this TGraph graph,
                                                                      XmlWriter writer,
                                                                      VertexIdentity<TVertex> vertexIdentities,
                                                                      EdgeIdentity<TVertex, TEdge> edgeIdentities)
            where TEdge : IEdge<TVertex>
            where TGraph : IEdgeListGraph<TVertex, TEdge>
        {
            Contract.Requires(graph != null);
            Contract.Requires(writer != null);

            var serializer = new GraphMlSerializer<TVertex, TEdge, TGraph>();
            serializer.Serialize(writer, graph, vertexIdentities, edgeIdentities);
        }

        public static void SerializeToGraphMl<TVertex, TEdge, TGraph>(this TGraph graph, XmlWriter writer)
            where TEdge : IEdge<TVertex>
            where TGraph : IEdgeListGraph<TVertex, TEdge>
        {
            Contract.Requires(graph != null);
            Contract.Requires(writer != null);

            var vertexIdentity = AlgorithmExtensions.GetVertexIdentity(graph);
            var edgeIdentity = AlgorithmExtensions.GetEdgeIdentity(graph);

            SerializeToGraphMl(
                graph,
                writer,
                vertexIdentity,
                edgeIdentity
            );
        }
    }
}