namespace QuickGraph.Predicates
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public sealed class ResidualEdgePredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        public IDictionary<TEdge, double> ResidualCapacities { get; }

        public ResidualEdgePredicate(
            IDictionary<TEdge, double> residualCapacities)
        {
            Contract.Requires(residualCapacities != null);

            ResidualCapacities = residualCapacities;
        }

        public bool Test(TEdge e)
        {
            Contract.Requires(e != null);
            return 0 < ResidualCapacities[e];
        }
    }
}