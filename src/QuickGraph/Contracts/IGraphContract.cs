namespace QuickGraph.Contracts
{
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IGraph<,>))]
    internal abstract class IGraphContract<TVertex, TEdge>
        : IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        bool IGraph<TVertex, TEdge>.IsDirected => default(bool);

        bool IGraph<TVertex, TEdge>.AllowParallelEdges => default(bool);
    }
}