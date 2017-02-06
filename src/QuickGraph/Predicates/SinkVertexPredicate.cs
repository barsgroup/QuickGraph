namespace QuickGraph.Predicates
{
    using System.Diagnostics.Contracts;

    public sealed class SinkVertexPredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly IIncidenceGraph<TVertex, TEdge> _visitedGraph;

        public SinkVertexPredicate(IIncidenceGraph<TVertex, TEdge> visitedGraph)
        {
            Contract.Requires(visitedGraph != null);

            _visitedGraph = visitedGraph;
        }

        public bool Test(TVertex v)
        {
            return _visitedGraph.IsOutEdgesEmpty(v);
        }
    }
}