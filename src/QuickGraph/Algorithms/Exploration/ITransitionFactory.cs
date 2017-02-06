namespace QuickGraph.Algorithms.Exploration
{
    using System.Collections.Generic;

    using QuickGraph.Clonable;

    public interface ITransitionFactory<TVertex, TEdge>
        where TVertex : ICloneable
        where TEdge : IEdge<TVertex>
    {
        IEnumerable<TEdge> Apply(TVertex source);

        bool IsValid(TVertex v);
    }
}