﻿namespace QuickGraph.Collections
{
    using System.Collections.Generic;

    using QuickGraph.Clonable;

    public class EdgeEdgeDictionary<TVertex, TEdge>
        : Dictionary<TEdge, TEdge>
          ,
          ICloneable
        where TEdge : IEdge<TVertex>
    {
        public EdgeEdgeDictionary()
        {
        }

        public EdgeEdgeDictionary(int capacity)
            : base(capacity)
        {
        }

        public EdgeEdgeDictionary<TVertex, TEdge> Clone()
        {
            var clone = new EdgeEdgeDictionary<TVertex, TEdge>(Count);
            foreach (var kv in this)
                clone.Add(kv.Key, kv.Value);
            return clone;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}