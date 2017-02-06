namespace QuickGraph.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IUndirectedEdge<>))]
    internal abstract class UndirectedEdgeContract<TVertex>
        : IUndirectedEdge<TVertex>
    {
        [ContractInvariantMethod]
        private void UndirectedEdgeInvariant()
        {
            IUndirectedEdge<TVertex> ithis = this;
            Contract.Invariant(Comparer<TVertex>.Default.Compare(ithis.Source, ithis.Target) <= 0);
        }

        #region IEdge<TVertex> Members

        public TVertex Source
        {
            get { throw new NotImplementedException(); }
        }

        public TVertex Target
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}