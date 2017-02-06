namespace QuickGraph
{
    using System.Diagnostics.Contracts;

    using QuickGraph.Contracts;

    /// <summary>A mutable vertex list graph</summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    [ContractClass(typeof(MutableVertexListGraphContract<,>))]
    public interface IMutableVertexListGraph<TVertex, TEdge> :
        IMutableIncidenceGraph<TVertex, TEdge>,
        IMutableVertexSet<TVertex>
        where TEdge : IEdge<TVertex>
    {
    }
}