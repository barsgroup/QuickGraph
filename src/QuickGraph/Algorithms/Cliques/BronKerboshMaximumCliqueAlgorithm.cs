﻿namespace QuickGraph.Algorithms.Cliques
{
    using System.Collections.Generic;

    using QuickGraph.Algorithms.Services;

    // under construction
    internal class BronKerboshMaximumCliqueAlgorithm<TVertex, TEdge>
        : MaximumCliqueAlgorithmBase<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        protected BronKerboshMaximumCliqueAlgorithm(
            IAlgorithmComponent host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

        protected BronKerboshMaximumCliqueAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
        }

        protected override void InternalCompute()
        {
            // the currently growing clique;
            var r = new List<TVertex>();

            // prospective nodes which are connected to all nodes in R 
            // and using which R can be expanded
            var p = new List<TVertex>();

            // nodes already processed i.e. nodes which were previously in P 
            // and hence all maximal cliques containing them have already been reported
            var x = new List<TVertex>();

            // An important invariant is that all nodes which are connected to every node 
            // of R are either in P or X.
        }
    }
}