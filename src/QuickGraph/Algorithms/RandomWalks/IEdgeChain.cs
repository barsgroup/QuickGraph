namespace QuickGraph.Algorithms.RandomWalks
{
    using System.Collections.Generic;

    public interface IEdgeChain<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> g, TVertex u, out TEdge successor);

        bool TryGetSuccessor(IEnumerable<TEdge> edges, TVertex u, out TEdge successor);
    }
}