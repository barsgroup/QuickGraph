namespace QuickGraph.Algorithms.RandomWalks
{
    using System;
    using System.Collections.Generic;

    public abstract class MarkovEdgeChainBase<TVertex, TEdge> :
        IMarkovEdgeChain<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        public Random Rand { get; set; } = new Random();

        public abstract bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> g, TVertex u, out TEdge successor);

        public abstract bool TryGetSuccessor(IEnumerable<TEdge> edges, TVertex u, out TEdge successor);
    }
}