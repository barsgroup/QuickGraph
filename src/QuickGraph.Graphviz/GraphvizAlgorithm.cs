namespace QuickGraph.Graphviz
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Text.RegularExpressions;

    using QuickGraph.Graphviz.Dot;

    /// <summary>An algorithm that renders a graph to the Graphviz DOT format.</summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    public class GraphvizAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private static readonly Regex WriteLineReplace = new Regex("\n", RegexOptions.Multiline);

        private readonly Dictionary<TVertex, int> _vertexIds = new Dictionary<TVertex, int>();

        private IEdgeListGraph<TVertex, TEdge> _visitedGraph;

        public GraphvizGraph GraphFormat { get; }

        public GraphvizVertex CommonVertexFormat { get; }

        public GraphvizEdge CommonEdgeFormat { get; }

        public IEdgeListGraph<TVertex, TEdge> VisitedGraph
        {
            get { return _visitedGraph; }
            set
            {
                Contract.Requires(value != null);

                _visitedGraph = value;
            }
        }

        /// <summary>Dot output stream.</summary>
        public StringWriter Output { get; private set; }

        /// <summary>Current image output type</summary>
        public GraphvizImageType ImageType { get; set; }

        public GraphvizAlgorithm(IEdgeListGraph<TVertex, TEdge> g)
            : this(g, ".", GraphvizImageType.Png)
        {
        }

        public GraphvizAlgorithm(
            IEdgeListGraph<TVertex, TEdge> g,
            string path,
            GraphvizImageType imageType
        )
        {
            Contract.Requires(g != null);
            Contract.Requires(!string.IsNullOrEmpty(path));

            _visitedGraph = g;
            ImageType = imageType;
            GraphFormat = new GraphvizGraph();
            CommonVertexFormat = new GraphvizVertex();
            CommonEdgeFormat = new GraphvizEdge();
        }

        public string Escape(string value)
        {
            return WriteLineReplace.Replace(value, "\\n");
        }

        public string Generate()
        {
            _vertexIds.Clear();
            Output = new StringWriter();

            // build vertex id map
            var i = 0;
            foreach (var v in VisitedGraph.Vertices)
                _vertexIds.Add(v, i++);

            if (VisitedGraph.IsDirected)
            {
                Output.Write("digraph ");
            }
            else
            {
                Output.Write("graph ");
            }
            Output.Write(GraphFormat.Name);
            Output.WriteLine(" {");

            var gf = GraphFormat.ToDot();
            if (gf.Length > 0)
            {
                Output.WriteLine(gf);
            }
            var vf = CommonVertexFormat.ToDot();
            if (vf.Length > 0)
            {
                Output.WriteLine("node [{0}];", vf);
            }
            var ef = CommonEdgeFormat.ToDot();
            if (ef.Length > 0)
            {
                Output.WriteLine("edge [{0}];", ef);
            }

            // initialize vertex map
            var colors = new Dictionary<TVertex, GraphColor>();
            foreach (var v in VisitedGraph.Vertices)
                colors[v] = GraphColor.White;
            var edgeColors = new Dictionary<TEdge, GraphColor>();
            foreach (var e in VisitedGraph.Edges)
                edgeColors[e] = GraphColor.White;

            WriteVertices(colors, VisitedGraph.Vertices);
            WriteEdges(edgeColors, VisitedGraph.Edges);

            Output.WriteLine("}");
            return Output.ToString();
        }

        public string Generate(IDotEngine dot, string outputFileName)
        {
            Contract.Requires(dot != null);
            Contract.Requires(!string.IsNullOrEmpty(outputFileName));

            var output = Generate();
            return dot.Run(ImageType, Output.ToString(), outputFileName);
        }

        private void OnFormatEdge(TEdge e)
        {
            if (FormatEdge != null)
            {
                var ev = new GraphvizEdge();
                FormatEdge(this, new FormatEdgeEventArgs<TVertex, TEdge>(e, ev));
                Output.Write(" {0}", ev.ToDot());
            }
        }

        private void OnFormatVertex(TVertex v)
        {
            Output.Write("{0} ", _vertexIds[v]);
            if (FormatVertex != null)
            {
                var gv = new GraphvizVertex
                         {
                             Label = v.ToString()
                         };
                FormatVertex(this, new FormatVertexEventArgs<TVertex>(v, gv));

                var s = gv.ToDot();
                if (s.Length != 0)
                {
                    Output.Write("[{0}]", s);
                }
            }
            Output.WriteLine(";");
        }

        private void WriteEdges(
            IDictionary<TEdge, GraphColor> edgeColors,
            IEnumerable<TEdge> edges)
        {
            Contract.Requires(edgeColors != null);
            Contract.Requires(edges != null);

            foreach (var e in edges)
            {
                if (edgeColors[e] != GraphColor.White)
                {
                    continue;
                }

                Output.Write(
                    "{0} -> {1} [",
                    _vertexIds[e.Source],
                    _vertexIds[e.Target]
                );

                OnFormatEdge(e);
                Output.WriteLine("];");

                edgeColors[e] = GraphColor.Black;
            }
        }

        private void WriteVertices(
            IDictionary<TVertex, GraphColor> colors,
            IEnumerable<TVertex> vertices)
        {
            Contract.Requires(colors != null);
            Contract.Requires(vertices != null);

            foreach (var v in vertices)
                if (colors[v] == GraphColor.White)
                {
                    OnFormatVertex(v);
                    colors[v] = GraphColor.Black;
                }
        }

        public event FormatEdgeAction<TVertex, TEdge> FormatEdge;

        /*
                /// <summary>
                /// Event raised while drawing a cluster
                /// </summary>
                public event FormatClusterEventHandler<Vertex,Edge> FormatCluster;
                private void OnFormatCluster(IVertexAndEdgeListGraph<Vertex,Edge> cluster)
                {
                    if (FormatCluster != null)
                    {
                        FormatClusterEventArgs<Vertex,Edge> args =
                            new FormatClusterEventArgs<Vertex,Edge>(cluster, new GraphvizGraph());
                        FormatCluster(this, args);
                        string s = args.GraphFormat.ToDot();
                        if (s.Length != 0)
                            Output.WriteLine(s);
                    }
                }
        */

        public event FormatVertexEventHandler<TVertex> FormatVertex;
    }
}