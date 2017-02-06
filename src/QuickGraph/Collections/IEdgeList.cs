namespace QuickGraph.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Clonable;

    /// <summary>A cloneable list of edges</summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    [ContractClass(typeof(EdgeListContract<,>))]
    public interface IEdgeList<TVertex, TEdge>
        : IList<TEdge>
          ,
          ICloneable
        where TEdge : IEdge<TVertex>
    {
        /// <summary>Gets a clone of this list</summary>
        /// <returns></returns>
        new IEdgeList<TVertex, TEdge> Clone();

        /// <summary>Trims excess allocated space</summary>
        void TrimExcess();
    }
}