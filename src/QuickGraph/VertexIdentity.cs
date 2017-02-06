namespace QuickGraph
{
    using System.Diagnostics.Contracts;

    [Pure]
    public delegate string VertexIdentity<TVertex>(TVertex v);
}