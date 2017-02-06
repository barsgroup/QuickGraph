﻿namespace QuickGraph.Algorithms.RandomWalks
{
    using System.Collections.Generic;

    public sealed class VanishingWeightedMarkovEdgeChain<TVertex, TEdge> :
        WeightedMarkovEdgeChainBase<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        public double Factor { get; set; }

        public VanishingWeightedMarkovEdgeChain(IDictionary<TEdge, double> weights)
            : this(weights, 0.2)
        {
        }

        public VanishingWeightedMarkovEdgeChain(IDictionary<TEdge, double> weights, double factor)
            : base(weights)
        {
            Factor = factor;
        }

        public override bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> g, TVertex u, out TEdge successor)
        {
            if (!g.IsOutEdgesEmpty(u))
            {
                // get outweight
                var outWeight = GetOutWeight(g, u);

                // get succesor
                TEdge s;
                if (TryGetSuccessor(g, u, Rand.NextDouble() * outWeight, out s))
                {
                    // update probabilities
                    Weights[s] *= Factor;

                    // normalize
                    foreach (var e in g.OutEdges(u))
                        Weights[e] /= outWeight;

                    successor = s;
                    return true;
                }
            }

            successor = default(TEdge);
            return false;
        }

        public override bool TryGetSuccessor(IEnumerable<TEdge> edges, TVertex u, out TEdge successor)
        {
            // get outweight
            var outWeight = GetWeights(edges);

            // get succesor
            TEdge s;
            if (TryGetSuccessor(edges, Rand.NextDouble() * outWeight, out s))
            {
                // update probabilities
                Weights[s] *= Factor;

                // normalize
                foreach (var e in edges)
                    Weights[e] /= outWeight;

                successor = s;
                return true;
            }

            successor = default(TEdge);
            return false;
        }
    }
}