namespace QuickGraph
{
    /// <summary>An edge factory delegate</summary>
    public delegate TEdge CreateEdgeDelegate<TVertex, TEdge>(
        IVertexListGraph<TVertex, TEdge> g,
        TVertex source,
        TVertex target)
        where TEdge : IEdge<TVertex>;
}