﻿using System;
using System.Collections.Generic;
using System.Text;
using QuickGraph.Clonable;

namespace QuickGraph.Collections
{
    public sealed class VertexEdgeDictionary<TVertex,TEdge>
        : Dictionary<TVertex, IEdgeList<TVertex, TEdge>>
        , IVertexEdgeDictionary<TVertex, TEdge>
        , ICloneable
        where TEdge : IEdge<TVertex>
    {
        public VertexEdgeDictionary() { }
        public VertexEdgeDictionary(int capacity)
            : base(capacity)
        { }
        public VertexEdgeDictionary(IEqualityComparer<TVertex> vertexComparer)
            : base(vertexComparer)
        { }
        public VertexEdgeDictionary(int capacity, IEqualityComparer<TVertex> vertexComparer)
            : base(capacity, vertexComparer)
        { }

        public VertexEdgeDictionary<TVertex, TEdge> Clone()
        {
            var clone = new VertexEdgeDictionary<TVertex, TEdge>(this.Count);
            foreach (var kv in this)
                clone.Add(kv.Key, kv.Value.Clone());
            return clone;
        }

        IVertexEdgeDictionary<TVertex, TEdge> IVertexEdgeDictionary<TVertex,TEdge>.Clone()
        {
            return this.Clone();
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}
