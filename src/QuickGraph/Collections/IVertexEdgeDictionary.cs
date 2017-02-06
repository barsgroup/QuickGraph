using System;
using System.Collections.Generic;
#if !SILVERLIGHT
#endif
using System.Diagnostics.Contracts;
using QuickGraph.Clonable;

namespace QuickGraph.Collections
{
    /// <summary>
    /// A dictionary of vertices to a list of edges
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    [ContractClass(typeof(IVertexEdgeDictionaryContract<,>))]
    public interface IVertexEdgeDictionary<TVertex, TEdge>
        : IDictionary<TVertex, IEdgeList<TVertex, TEdge>>
        , ICloneable
     where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Gets a clone of the dictionary. The vertices and edges are not cloned.
        /// </summary>
        /// <returns></returns>
        new 
        IVertexEdgeDictionary<TVertex, TEdge> Clone();
    }
}
