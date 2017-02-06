namespace QuickGraph.Predicates
{
    using System.Diagnostics.Contracts;

    /// <summary>A vertex predicate that detects vertex with no in or out edges.</summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    public sealed class IsolatedVertexPredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly IBidirectionalGraph<TVertex, TEdge> _visitedGraph;

        public IsolatedVertexPredicate(IBidirectionalGraph<TVertex, TEdge> visitedGraph)
        {
            Contract.Requires(visitedGraph != null);

            _visitedGraph = visitedGraph;
        }

        [Pure]
        public bool Test(TVertex v)
        {
            Contract.Requires(v != null);

            return _visitedGraph.IsInEdgesEmpty(v)
                   && _visitedGraph.IsOutEdgesEmpty(v);
        }
    }
}