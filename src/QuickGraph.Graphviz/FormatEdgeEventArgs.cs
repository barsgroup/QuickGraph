namespace QuickGraph.Graphviz
{
    using QuickGraph.Graphviz.Dot;

    public sealed class FormatEdgeEventArgs<TVertex, TEdge>
        : EdgeEventArgs<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>Edge formatter</summary>
        public GraphvizEdge EdgeFormatter { get; }

        internal FormatEdgeEventArgs(TEdge e, GraphvizEdge edgeFormatter)
            : base(e)
        {
#if CONTRACTS_BUG
            Contract.Requires(edgeFormatter != null);
#endif
            EdgeFormatter = edgeFormatter;
        }
    }

    public delegate void FormatEdgeAction<TVertex, TEdge>(
        object sender,
        FormatEdgeEventArgs<TVertex, TEdge> e)
        where TEdge : IEdge<TVertex>;
}