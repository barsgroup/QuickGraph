namespace QuickGraph
{
    using System.Diagnostics.Contracts;

    [Pure]
    public delegate bool EdgePredicate<TVertex, TEdge>(TEdge e)
        where TEdge : IEdge<TVertex>;
}