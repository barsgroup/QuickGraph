namespace QuickGraph
{
    /// <summary>A vertex factory delegate.</summary>
    public delegate TVertex CreateVertexDelegate<TVertex, TEdge>(
        IVertexListGraph<TVertex, TEdge> g)
        where TEdge : IEdge<TVertex>;
}