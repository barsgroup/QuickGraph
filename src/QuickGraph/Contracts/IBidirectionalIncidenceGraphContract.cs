﻿namespace QuickGraph.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    [ContractClassFor(typeof(IBidirectionalIncidenceGraph<,>))]
    internal abstract class BidirectionalIncidenceGraphContract<TVertex, TEdge>
        : IBidirectionalIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IImplicitVertexSet<TVertex> Members

        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IBidirectionalImplicitGraph<TVertex,TEdge> Members

        [Pure]
        bool IBidirectionalIncidenceGraph<TVertex, TEdge>.IsInEdgesEmpty(TVertex v)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> ithis = this;
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<bool>() == (ithis.InDegree(v) == 0));

            return default(bool);
        }

        [Pure]
        int IBidirectionalIncidenceGraph<TVertex, TEdge>.InDegree(TVertex v)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> ithis = this;
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<int>() == ithis.InEdges(v).Count());

            return default(int);
        }

        [Pure]
        IEnumerable<TEdge> IBidirectionalIncidenceGraph<TVertex, TEdge>.InEdges(TVertex v)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> ithis = this;
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<IEnumerable<TEdge>>() != null);
            Contract.Ensures(
                Contract.Result<IEnumerable<TEdge>>().All(
                    edge => edge != null && edge.Target.Equals(v)
                )
            );

            return default(IEnumerable<TEdge>);
        }

        [Pure]
        bool IBidirectionalIncidenceGraph<TVertex, TEdge>.TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> ithis = this;
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<bool>() == ithis.ContainsVertex(v));
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out edges) != null);
            Contract.Ensures(
                !Contract.Result<bool>() ||
                Contract.ValueAtReturn(out edges).All(
                    edge => edge != null && edge.Target.Equals(v)
                )
            );

            edges = null;
            return default(bool);
        }

        [Pure]
        TEdge IBidirectionalIncidenceGraph<TVertex, TEdge>.InEdge(TVertex v, int index)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> ithis = this;
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Requires(index >= 0 && index < ithis.InDegree(v));
            Contract.Ensures(Contract.Result<TEdge>().Equals(ithis.InEdges(v).ElementAt(index)));

            return default(TEdge);
        }

        [Pure]
        int IBidirectionalIncidenceGraph<TVertex, TEdge>.Degree(TVertex v)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> ithis = this;
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<int>() == ithis.InDegree(v) + ithis.OutDegree(v));

            return default(int);
        }

        #endregion

        #region IImplicitGraph<TVertex,TEdge> Members

        bool IImplicitGraph<TVertex, TEdge>.IsOutEdgesEmpty(TVertex v)
        {
            throw new NotImplementedException();
        }

        int IImplicitGraph<TVertex, TEdge>.OutDegree(TVertex v)
        {
            throw new NotImplementedException();
        }

        IEnumerable<TEdge> IImplicitGraph<TVertex, TEdge>.OutEdges(TVertex v)
        {
            throw new NotImplementedException();
        }

        bool IImplicitGraph<TVertex, TEdge>.TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        TEdge IImplicitGraph<TVertex, TEdge>.OutEdge(TVertex v, int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IGraph<TVertex,TEdge> Members

        bool IGraph<TVertex, TEdge>.IsDirected
        {
            get { throw new NotImplementedException(); }
        }

        bool IGraph<TVertex, TEdge>.AllowParallelEdges
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IIncidenceGraph<TVertex,TEdge> Members

        bool IIncidenceGraph<TVertex, TEdge>.ContainsEdge(TVertex source, TVertex target)
        {
            throw new NotImplementedException();
        }

        bool IIncidenceGraph<TVertex, TEdge>.TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        bool IIncidenceGraph<TVertex, TEdge>.TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}