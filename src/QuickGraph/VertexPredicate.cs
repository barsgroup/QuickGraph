namespace QuickGraph
{
    using System.Diagnostics.Contracts;

    [Pure]
    public delegate bool VertexPredicate<TVertex>(TVertex v);
}