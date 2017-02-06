namespace QuickGraph.Algorithms.RandomWalks
{
    using System;

    public interface IMarkovEdgeChain<TVertex, TEdge>
        : IEdgeChain<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        Random Rand { get; set; }
    }
}