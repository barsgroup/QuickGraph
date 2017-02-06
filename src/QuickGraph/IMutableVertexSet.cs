namespace QuickGraph
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Contracts;

    /// <summary>A mutable vertex set</summary>
    /// <typeparam name="TVertex"></typeparam>
    [ContractClass(typeof(MutableVertexSetContract<>))]
    public interface IMutableVertexSet<TVertex>
        : IVertexSet<TVertex>
    {
        bool AddVertex(TVertex v);

        int AddVertexRange(IEnumerable<TVertex> vertices);

        bool RemoveVertex(TVertex v);

        int RemoveVertexIf(VertexPredicate<TVertex> pred);

        event VertexAction<TVertex> VertexAdded;

        event VertexAction<TVertex> VertexRemoved;
    }
}