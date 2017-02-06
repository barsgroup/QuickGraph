namespace QuickGraph
{
    public class UndirectedEdgeEventArgs<TVertex, TEdge>
        : EdgeEventArgs<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        public bool Reversed { get; }

        public TVertex Source => Reversed
                                     ? Edge.Target
                                     : Edge.Source;

        public TVertex Target => Reversed
                                     ? Edge.Source
                                     : Edge.Target;

        public UndirectedEdgeEventArgs(TEdge edge, bool reversed)
            : base(edge)
        {
            Reversed = reversed;
        }
    }

    public delegate void UndirectedEdgeAction<TVertex, TEdge>(
        object sender,
        UndirectedEdgeEventArgs<TVertex, TEdge> e)
        where TEdge : IEdge<TVertex>;
}