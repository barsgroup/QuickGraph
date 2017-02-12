namespace QuickGraph.Serialization
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    using QuickGraph.Algorithms;
    using QuickGraph.Serialization.DirectedGraphML;

    /// <summary>Directed Graph Markup Language extensions</summary>
    public static class DirectedGraphMlExtensions
    {
        private static XmlSerializer directedGraphSerializer;

        /// <summary>Gets the DirectedGraph xml serializer</summary>
        public static XmlSerializer DirectedGraphSerializer
        {
            get
            {
                if (directedGraphSerializer == null)
                {
                    directedGraphSerializer = new XmlSerializer(typeof(DirectedGraph));
                }
                return directedGraphSerializer;
            }
        }

        public static void OpenAsDgml<TVertex, TEdge>(
#if !NET20
            this
#endif
                IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            string filename)
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(graph != null);

            if (filename == null)
            {
                filename = "graph.dgml";
            }

            WriteXml(ToDirectedGraphMl(graph), filename);
        }

        /// <summary>Populates a DGML graph from a graph</summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="visitedGraph"></param>
        /// <returns></returns>
        public static DirectedGraph ToDirectedGraphMl<TVertex, TEdge>(
#if !NET20
            this
#endif
                IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(visitedGraph != null);
            Contract.Ensures(Contract.Result<DirectedGraph>() != null);

            return ToDirectedGraphMl(
                visitedGraph,
                AlgorithmExtensions.GetVertexIdentity(visitedGraph),
                AlgorithmExtensions.GetEdgeIdentity(visitedGraph)
            );
        }

        /// <summary>Populates a DGML graph from a graph</summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="visitedGraph"></param>
        /// <param name="vertexColors"></param>
        /// <returns></returns>
        public static DirectedGraph ToDirectedGraphMl<TVertex, TEdge>(
#if !NET20
            this
#endif
                IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TVertex, GraphColor> vertexColors)
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(visitedGraph != null);
            Contract.Requires(vertexColors != null);
            Contract.Ensures(Contract.Result<DirectedGraph>() != null);

            return ToDirectedGraphMl(
                visitedGraph,
                AlgorithmExtensions.GetVertexIdentity(visitedGraph),
                AlgorithmExtensions.GetEdgeIdentity(visitedGraph),
                (v, n) =>
                {
                    var color = vertexColors(v);
                    switch (color)
                    {
                        case GraphColor.Black:
                            n.Background = "Black";
                            break;
                        case GraphColor.Gray:
                            n.Background = "LightGray";
                            break;
                        case GraphColor.White:
                            n.Background = "White";
                            break;
                    }
                },
                null
            );
        }

        /// <summary>Populates a DGML graph from a graph</summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="visitedGraph"></param>
        /// <param name="vertexIdentities"></param>
        /// <param name="edgeIdentities"></param>
        /// <returns></returns>
        public static DirectedGraph ToDirectedGraphMl<TVertex, TEdge>(
#if !NET20
            this
#endif
                IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            VertexIdentity<TVertex> vertexIdentities,
            EdgeIdentity<TVertex, TEdge> edgeIdentities)
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(visitedGraph != null);
            Contract.Requires(vertexIdentities != null);
            Contract.Requires(edgeIdentities != null);
            Contract.Ensures(Contract.Result<DirectedGraph>() != null);

            return ToDirectedGraphMl(
                visitedGraph,
                vertexIdentities,
                edgeIdentities,
                null,
                null);
        }

        /// <summary>Populates a DGML graph from a graph</summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="visitedGraph"></param>
        /// <param name="vertexIdentities"></param>
        /// <param name="edgeIdentities"></param>
        /// <param name="formatNode"></param>
        /// <param name="formatEdge"></param>
        /// <returns></returns>
        public static DirectedGraph ToDirectedGraphMl<TVertex, TEdge>(
#if !NET20
            this
#endif
                IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            VertexIdentity<TVertex> vertexIdentities,
            EdgeIdentity<TVertex, TEdge> edgeIdentities,
            Action<TVertex, DirectedGraphNode> formatNode,
            Action<TEdge, DirectedGraphLink> formatEdge)
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(visitedGraph != null);
            Contract.Requires(vertexIdentities != null);
            Contract.Requires(edgeIdentities != null);
            Contract.Ensures(Contract.Result<DirectedGraph>() != null);

            var algorithm = new DirectedGraphMlAlgorithm<TVertex, TEdge>(
                visitedGraph,
                vertexIdentities,
                edgeIdentities
            );
            if (formatNode != null)
            {
                algorithm.FormatNode += formatNode;
            }
            if (formatEdge != null)
            {
                algorithm.FormatEdge += formatEdge;
            }
            algorithm.Compute();

            return algorithm.DirectedGraph;
        }

        /// <summary>Writes the dgml data structure to the xml writer</summary>
        /// <param name="fileName" />
        /// <param name="graph"></param>
        public static void WriteXml(
#if !NET20
            this
#endif
                DirectedGraph graph,
            string fileName)
        {
            Contract.Requires(graph != null);
            Contract.Requires(!string.IsNullOrEmpty(fileName));
            using (var stream = File.CreateText(fileName))
            {
                WriteXml(graph, stream);
            }
        }

        /// <summary>Writes the dgml data structure to the xml writer</summary>
        /// <param name="graph"></param>
        /// <param name="writer"></param>
        public static void WriteXml(
#if !NET20
            this
#endif
                DirectedGraph graph,
            XmlWriter writer)
        {
            Contract.Requires(graph != null);
            Contract.Requires(writer != null);

            DirectedGraphSerializer.Serialize(writer, graph);
        }

        /// <summary>Writes the dgml data structure to the xml writer</summary>
        /// <param name="graph"></param>
        /// <param name="stream"></param>
        public static void WriteXml(
#if !NET20
            this
#endif
                DirectedGraph graph,
            Stream stream)
        {
            Contract.Requires(graph != null);
            Contract.Requires(stream != null);

            DirectedGraphSerializer.Serialize(stream, graph);
        }

        /// <summary>Writes the dgml data structure to the xml writer</summary>
        /// <param name="graph"></param>
        /// <param name="writer"></param>
        public static void WriteXml(
#if !NET20
            this
#endif
                DirectedGraph graph,
            TextWriter writer)
        {
            Contract.Requires(graph != null);
            Contract.Requires(writer != null);

            DirectedGraphSerializer.Serialize(writer, graph);
        }
    }
}