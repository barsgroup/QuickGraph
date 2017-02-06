namespace QuickGraph
{
    using System.Diagnostics.Contracts;

    using QuickGraph.Contracts;

    /// <summary>A directed graph datastructure that is efficient to traverse both in and out edges.</summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    [ContractClass(typeof(BidirectionalGraphContract<,>))]
    public interface IBidirectionalGraph<TVertex, TEdge>
        : IVertexAndEdgeListGraph<TVertex, TEdge>
          ,
          IBidirectionalIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}