﻿using System;
using System.Diagnostics.Contracts;

namespace QuickGraph.Predicates
{
    public sealed class SinkVertexPredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly IIncidenceGraph<TVertex, TEdge> visitedGraph;

        public SinkVertexPredicate(IIncidenceGraph<TVertex, TEdge> visitedGraph)
        {
            Contract.Requires(visitedGraph != null);

            this.visitedGraph = visitedGraph;
        }

        public bool Test(TVertex v)
        {
            return this.visitedGraph.IsOutEdgesEmpty(v);
        }
    }
}
