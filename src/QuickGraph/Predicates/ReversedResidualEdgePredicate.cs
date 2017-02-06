namespace QuickGraph.Predicates
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public sealed class ReversedResidualEdgePredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>Residual capacities map</summary>
        public IDictionary<TEdge, double> ResidualCapacities { get; }

        /// <summary>Reversed edges map</summary>
        public IDictionary<TEdge, TEdge> ReversedEdges { get; }

        public ReversedResidualEdgePredicate(
            IDictionary<TEdge, double> residualCapacities,
            IDictionary<TEdge, TEdge> reversedEdges)
        {
            Contract.Requires(residualCapacities != null);
            Contract.Requires(reversedEdges != null);

            ResidualCapacities = residualCapacities;
            ReversedEdges = reversedEdges;
        }

        public bool Test(TEdge e)
        {
            Contract.Requires(e != null);
            return 0 < ResidualCapacities[ReversedEdges[e]];
        }
    }
}