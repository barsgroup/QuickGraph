namespace QuickGraph
{
    using System.Diagnostics.Contracts;

    using QuickGraph.Contracts;

    /// <summary>An undirected edge.</summary>
    /// <remarks>Invariant: source must be less or equal to target (using the default comparer)</remarks>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    [ContractClass(typeof(UndirectedEdgeContract<>))]
    public interface IUndirectedEdge<TVertex>
        : IEdge<TVertex>
    {
    }
}