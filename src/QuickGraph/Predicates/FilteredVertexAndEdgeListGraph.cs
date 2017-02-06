namespace QuickGraph.Predicates
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class FilteredVertexAndEdgeListGraph<TVertex, TEdge, TGraph> :
        FilteredVertexListGraph<TVertex, TEdge, TGraph>,
        IVertexAndEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IVertexAndEdgeListGraph<TVertex, TEdge>
    {
        public bool IsEdgesEmpty => EdgeCount == 0;

        public int EdgeCount
        {
            get
            {
                var count = 0;
                foreach (var edge in BaseGraph.Edges)
                    if (
                        VertexPredicate(edge.Source)
                        && VertexPredicate(edge.Target)
                        && EdgePredicate(edge))
                    {
                        count++;
                    }
                return count;
            }
        }

        public IEnumerable<TEdge> Edges
        {
            get
            {
                foreach (var edge in BaseGraph.Edges)
                    if (
                        VertexPredicate(edge.Source)
                        && VertexPredicate(edge.Target)
                        && EdgePredicate(edge))
                    {
                        yield return edge;
                    }
            }
        }

        public FilteredVertexAndEdgeListGraph(
            TGraph baseGraph,
            VertexPredicate<TVertex> vertexPredicate,
            EdgePredicate<TVertex, TEdge> edgePredicate
        )
            : base(baseGraph, vertexPredicate, edgePredicate)
        {
        }

        [Pure]
        public bool ContainsEdge(TEdge edge)
        {
            foreach (var e in Edges)
                if (Comparer<TEdge>.Default.Compare(edge, e) == 0)
                {
                    return true;
                }
            return false;
        }
    }
}