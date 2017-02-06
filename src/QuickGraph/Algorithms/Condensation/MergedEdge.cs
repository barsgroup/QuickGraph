namespace QuickGraph.Algorithms.Condensation
{
    using System.Collections.Generic;

    public sealed class MergedEdge<TVertex, TEdge> : Edge<TVertex>
        where TEdge : IEdge<TVertex>
    {
        private List<TEdge> _edges = new List<TEdge>();

        public IList<TEdge> Edges => _edges;

        public MergedEdge(TVertex source, TVertex target)
            : base(source, target)
        {
        }

        public static MergedEdge<TVertex, TEdge> Merge(
            MergedEdge<TVertex, TEdge> inEdge,
            MergedEdge<TVertex, TEdge> outEdge
        )
        {
            var newEdge = new MergedEdge<TVertex, TEdge>(
                inEdge.Source,
                outEdge.Target);
            newEdge._edges = new List<TEdge>(inEdge.Edges.Count + outEdge.Edges.Count);
            newEdge._edges.AddRange(inEdge.Edges);
            newEdge._edges.AddRange(outEdge._edges);

            return newEdge;
        }
    }
}