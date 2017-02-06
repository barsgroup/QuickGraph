namespace QuickGraph.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Clonable;

    [ContractClassFor(typeof(IVertexEdgeDictionary<,>))]
    internal abstract class VertexEdgeDictionaryContract<TVertex, TEdge>
        : IVertexEdgeDictionary<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        IVertexEdgeDictionary<TVertex, TEdge> IVertexEdgeDictionary<TVertex, TEdge>.Clone()
        {
            Contract.Ensures(Contract.Result<IVertexEdgeDictionary<TVertex, TEdge>>() != null);
            throw new NotImplementedException();
        }

        #region others

        void IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.Add(TVertex key, IEdgeList<TVertex, TEdge> value)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.ContainsKey(TVertex key)
        {
            throw new NotImplementedException();
        }

        ICollection<TVertex> IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.Keys
        {
            get { throw new NotImplementedException(); }
        }

        bool IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.Remove(TVertex key)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.TryGetValue(TVertex key,
                                                                         out IEdgeList<TVertex, TEdge> value)
        {
            throw new NotImplementedException();
        }

        ICollection<IEdgeList<TVertex, TEdge>> IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.Values
        {
            get { throw new NotImplementedException(); }
        }

        IEdgeList<TVertex, TEdge> IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.this[TVertex key]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        void ICollection<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Add(
            KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Contains(
            KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.CopyTo(
            KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>[] array,
            int arrayIndex)
        {
            throw new NotImplementedException();
        }

        int ICollection<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Count
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Remove(
            KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>> item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>
            IEnumerable<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}