namespace QuickGraph
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Contracts;

    [ContractClass(typeof(IncidenceGraphContract<,>))]
    public interface IIncidenceGraph<TVertex, TEdge>
        : IImplicitGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        bool ContainsEdge(TVertex source, TVertex target);

        bool TryGetEdge(
            TVertex source,
            TVertex target,
            out TEdge edge);

        bool TryGetEdges(
            TVertex source,
            TVertex target,
            out IEnumerable<TEdge> edges);
    }
}