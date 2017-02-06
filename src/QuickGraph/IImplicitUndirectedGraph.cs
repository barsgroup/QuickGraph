namespace QuickGraph
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Contracts;

    [ContractClass(typeof(ImplicitUndirectedGraphContract<,>))]
    public interface IImplicitUndirectedGraph<TVertex, TEdge>
        : IImplicitVertexSet<TVertex>
          ,
          IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        [Pure]
        EdgeEqualityComparer<TVertex, TEdge> EdgeEqualityComparer { get; }

        [Pure]
        int AdjacentDegree(TVertex v);

        [Pure]
        TEdge AdjacentEdge(TVertex v, int index);

        [Pure]
        IEnumerable<TEdge> AdjacentEdges(TVertex v);

        [Pure]
        bool ContainsEdge(TVertex source, TVertex target);

        [Pure]
        bool IsAdjacentEdgesEmpty(TVertex v);

        [Pure]
        bool TryGetEdge(TVertex source, TVertex target, out TEdge edge);
    }
}