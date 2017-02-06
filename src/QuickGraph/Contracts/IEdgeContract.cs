namespace QuickGraph.Contracts
{
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IEdge<>))]
    internal abstract class EdgeContract<TVertex>
        : IEdge<TVertex>
    {
        TVertex IEdge<TVertex>.Source
        {
            get
            {
                Contract.Ensures(Contract.Result<TVertex>() != null);
                return default(TVertex);
            }
        }

        TVertex IEdge<TVertex>.Target
        {
            get
            {
                Contract.Ensures(Contract.Result<TVertex>() != null);
                return default(TVertex);
            }
        }

        [ContractInvariantMethod]
        private void EdgeInvariant()
        {
            IEdge<TVertex> ithis = this;
            Contract.Invariant(ithis.Source != null);
            Contract.Invariant(ithis.Target != null);
        }
    }
}