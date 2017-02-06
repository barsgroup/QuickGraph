using System;
using System.Collections.Generic;
using QuickGraph.Clonable;

namespace QuickGraph.Algorithms.Exploration
{
    public interface ITransitionFactory<TVertex,TEdge>
        where TVertex : ICloneable
        where TEdge : IEdge<TVertex>
    {
        bool IsValid(TVertex v);
        IEnumerable<TEdge> Apply(TVertex source);
    }
}
