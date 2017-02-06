namespace QuickGraph.Contracts
{
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IImplicitVertexSet<>))]
    internal abstract class ImplicitVertexSetContract<TVertex>
        : IImplicitVertexSet<TVertex>
    {
        [Pure]
        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
        {
            IImplicitVertexSet<TVertex> ithis = this;
            Contract.Requires(vertex != null);

            return default(bool);
        }
    }
}