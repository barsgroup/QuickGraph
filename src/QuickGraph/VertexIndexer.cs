namespace QuickGraph
{
    using System.Diagnostics.Contracts;

    [Pure]
    public delegate int VertexIndexer<TVertex>(TVertex v);
}