namespace QuickGraph.Graphviz
{
    using System.IO;

    using QuickGraph.Algorithms.Condensation;

    public class EdgeMergeCondensatedGraphRenderer<TVertex, TEdge> :
        GraphRendererBase<TVertex, MergedEdge<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        public EdgeMergeCondensatedGraphRenderer(
            IVertexAndEdgeListGraph<TVertex, MergedEdge<TVertex, TEdge>> visitedGraph)
            : base(visitedGraph)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            Graphviz.FormatVertex += GraphvizFormatVertex;
            Graphviz.FormatEdge += GraphvizFormatEdge;
        }

        private void GraphvizFormatEdge(object sender, FormatEdgeEventArgs<TVertex, MergedEdge<TVertex, TEdge>> e)
        {
            var sw = new StringWriter();
            sw.WriteLine("{0}", e.Edge.Edges.Count);
            foreach (var edge in e.Edge.Edges)
                sw.WriteLine("  {0}", edge);
            e.EdgeFormatter.Label.Value = Graphviz.Escape(sw.ToString());
        }

        private void GraphvizFormatVertex(object sender, FormatVertexEventArgs<TVertex> e)
        {
            e.VertexFormatter.Label = e.Vertex.ToString();
        }
    }
}