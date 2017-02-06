using System;
using System.Collections.Generic;
using System.Text;
using QuickGraph.Clonable;

namespace QuickGraph.Collections
{
    public class EdgeEdgeDictionary<TVertex, TEdge>
        : Dictionary<TEdge, TEdge>
        , ICloneable
        where TEdge : IEdge<TVertex>
    {
        public EdgeEdgeDictionary()
        { }

        public EdgeEdgeDictionary(int capacity)
            : base(capacity)
        { }

        public EdgeEdgeDictionary<TVertex, TEdge> Clone()
        {
            var clone = new EdgeEdgeDictionary<TVertex, TEdge>(this.Count);
            foreach (var kv in this)
                clone.Add(kv.Key, kv.Value);
            return clone;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}
