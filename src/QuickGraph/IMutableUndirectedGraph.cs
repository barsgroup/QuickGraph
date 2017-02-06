namespace QuickGraph
{
    using System.Diagnostics.Contracts;

    using QuickGraph.Contracts;

    /// <summary>A mutable indirect graph</summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    [ContractClass(typeof(MutableUndirectedGraphContract<,>))]
    public interface IMutableUndirectedGraph<TVertex, TEdge>
        : IMutableEdgeListGraph<TVertex, TEdge>
          ,
          IMutableVertexSet<TVertex>
          ,
          IUndirectedGraph<TVertex, TEdge>
          ,
          IMutableVertexAndEdgeSet<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        void ClearAdjacentEdges(TVertex vertex);

        int RemoveAdjacentEdgeIf(TVertex vertex, EdgePredicate<TVertex, TEdge> predicate);
    }
}