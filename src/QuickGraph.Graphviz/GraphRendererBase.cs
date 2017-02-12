namespace QuickGraph.Graphviz
{
    using QuickGraph.Graphviz.Dot;

    public abstract class GraphRendererBase<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        public GraphvizAlgorithm<TVertex, TEdge> Graphviz { get; }

        public IEdgeListGraph<TVertex, TEdge> VisitedGraph => Graphviz.VisitedGraph;

        public GraphRendererBase(
            IEdgeListGraph<TVertex, TEdge> visitedGraph)
        {
            Graphviz = new GraphvizAlgorithm<TVertex, TEdge>(visitedGraph);
            Initialize();
        }

        public string Generate(IDotEngine dot, string fileName)
        {
            return Graphviz.Generate(dot, fileName);
        }

        protected virtual void Initialize()
        {
            Graphviz.CommonVertexFormat.Style = GraphvizVertexStyle.Filled;
            Graphviz.CommonVertexFormat.FillColor = GraphvizColor.LightYellow;
            Graphviz.CommonVertexFormat.Font = new GraphvizFont("Tahoma", 8.25F);
            Graphviz.CommonVertexFormat.Shape = GraphvizVertexShape.Box;

            Graphviz.CommonEdgeFormat.Font = new GraphvizFont("Tahoma", 8.25F);
        }
    }
}