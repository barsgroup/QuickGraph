namespace QuickGraph.Algorithms.RandomWalks
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public abstract class WeightedMarkovEdgeChainBase<TVertex, TEdge> :
        MarkovEdgeChainBase<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        public IDictionary<TEdge, double> Weights { get; set; }

        public WeightedMarkovEdgeChainBase(IDictionary<TEdge, double> weights)
        {
            Contract.Requires(weights != null);

            Weights = weights;
        }

        protected double GetOutWeight(IImplicitGraph<TVertex, TEdge> g, TVertex u)
        {
            var edges = g.OutEdges(u);
            return GetWeights(edges);
        }

        protected double GetWeights(IEnumerable<TEdge> edges)
        {
            double outWeight = 0;
            foreach (var e in edges)
                outWeight += Weights[e];
            return outWeight;
        }

        protected bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> g, TVertex u, double position, out TEdge successor)
        {
            Contract.Requires(g != null);
            Contract.Requires(u != null);

            var edges = g.OutEdges(u);
            return TryGetSuccessor(edges, position, out successor);
        }

        protected bool TryGetSuccessor(IEnumerable<TEdge> edges, double position, out TEdge successor)
        {
            Contract.Requires(edges != null);

            double pos = 0;
            double nextPos = 0;
            foreach (var e in edges)
            {
                nextPos = pos + Weights[e];
                if (position >= pos && position <= nextPos)
                {
                    successor = e;
                    return true;
                }
                pos = nextPos;
            }

            successor = default(TEdge);
            return false;
        }
    }
}