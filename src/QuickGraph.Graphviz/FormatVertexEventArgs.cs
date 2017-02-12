namespace QuickGraph.Graphviz
{
    using QuickGraph.Graphviz.Dot;

    public sealed class FormatVertexEventArgs<TVertex>
        : VertexEventArgs<TVertex>
    {
        public GraphvizVertex VertexFormatter { get; }

        internal FormatVertexEventArgs(TVertex v, GraphvizVertex vertexFormatter)
            : base(v)
        {
#if CONTRACTS_BUG
            Contract.Requires(vertexFormatter != null);
#endif
            VertexFormatter = vertexFormatter;
        }
    }

    public delegate void FormatVertexEventHandler<TVertex>(
        object sender,
        FormatVertexEventArgs<TVertex> e);
}