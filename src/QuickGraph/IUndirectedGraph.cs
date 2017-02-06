namespace QuickGraph
{
    using System.Diagnostics.Contracts;

    using QuickGraph.Contracts;

    /// <summary>An undirected graph</summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    [ContractClass(typeof(UndirectedGraphContract<,>))]
    public interface IUndirectedGraph<TVertex, TEdge>
        : IImplicitUndirectedGraph<TVertex, TEdge>
          ,
          IEdgeListGraph<TVertex, TEdge>
          ,
          IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}