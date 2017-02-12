namespace QuickGraph.Serialization
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Xml.Serialization;

    public class XmlSerializableEdge<TVertex>
        : IEdge<TVertex>
    {
        [XmlElement]
        public TVertex Source { get; set; }

        [XmlElement]
        public TVertex Target { get; set; }
    }

    /// <summary>A base class that creates a proxy to a graph that is xml serializable</summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    /// <typeparam name="TGraph"></typeparam>
    [XmlRoot("graph")]
    public class XmlSerializableGraph<TVertex, TEdge, TGraph>
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>, new()
    {
        private XmlEdgeList _edges;

        [XmlElement("graph-traits")]
        public TGraph Graph { get; }

        [XmlArray("edges")]
        [XmlArrayItem("edge")]
        public IEnumerable<TEdge> Edges
        {
            get
            {
                if (_edges == null)
                {
                    _edges = new XmlEdgeList(Graph);
                }
                return _edges;
            }
        }

        public XmlSerializableGraph(TGraph graph)
        {
            Contract.Requires(graph != null);

            Graph = graph;
        }

        public XmlSerializableGraph()
            : this(new TGraph())
        {
        }

        public class XmlEdgeList
            : IEnumerable<TEdge>
        {
            private readonly TGraph _graph;

            internal XmlEdgeList(TGraph graph)
            {
                Contract.Requires(graph != null);

                _graph = graph;
            }

            public void Add(TEdge edge)
            {
                Contract.Requires(edge != null);

                _graph.AddVerticesAndEdge(edge);
            }

            public IEnumerator<TEdge> GetEnumerator()
            {
                return _graph.Edges.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}