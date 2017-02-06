﻿namespace QuickGraph.Predicates
{
    using System.Collections.Generic;

    public class FilteredIncidenceGraph<TVertex, TEdge, TGraph>
        : FilteredImplicitGraph<TVertex, TEdge, TGraph>
          ,
          IIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IIncidenceGraph<TVertex, TEdge>
    {
        public FilteredIncidenceGraph(
            TGraph baseGraph,
            VertexPredicate<TVertex> vertexPredicate,
            EdgePredicate<TVertex, TEdge> edgePredicate
        )
            : base(baseGraph, vertexPredicate, edgePredicate)
        {
        }

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            if (!VertexPredicate(source))
            {
                return false;
            }
            if (!VertexPredicate(target))
            {
                return false;
            }

            foreach (var edge in BaseGraph.OutEdges(source))
                if (edge.Target.Equals(target) && EdgePredicate(edge))
                {
                    return true;
                }
            return false;
        }

        public bool TryGetEdge(
            TVertex source,
            TVertex target,
            out TEdge edge)
        {
            IEnumerable<TEdge> unfilteredEdges;
            if (VertexPredicate(source) &&
                VertexPredicate(target) &&
                BaseGraph.TryGetEdges(source, target, out unfilteredEdges))
            {
                foreach (var ufe in unfilteredEdges)
                    if (EdgePredicate(ufe))
                    {
                        edge = ufe;
                        return true;
                    }
            }
            edge = default(TEdge);
            return false;
        }

        public bool TryGetEdges(
            TVertex source,
            TVertex target,
            out IEnumerable<TEdge> edges)
        {
            edges = null;
            if (!VertexPredicate(source))
            {
                return false;
            }
            if (!VertexPredicate(target))
            {
                return false;
            }

            IEnumerable<TEdge> unfilteredEdges;
            if (BaseGraph.TryGetEdges(source, target, out unfilteredEdges))
            {
                var filtered = new List<TEdge>();
                foreach (var edge in unfilteredEdges)
                    if (EdgePredicate(edge))
                    {
                        filtered.Add(edge);
                    }
                edges = filtered;
                return true;
            }

            return false;
        }
    }
}