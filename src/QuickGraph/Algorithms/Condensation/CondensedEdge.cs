namespace QuickGraph.Algorithms.Condensation
{
    using System.Collections.Generic;

    public sealed class CondensedEdge<TVertex, TEdge, TGraph> : Edge<TGraph>
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>, new()
    {
        private readonly List<TEdge> _edges = new List<TEdge>();

        public IList<TEdge> Edges => _edges;

        public CondensedEdge(TGraph source, TGraph target)
            : base(source, target)
        {
        }
    }
}