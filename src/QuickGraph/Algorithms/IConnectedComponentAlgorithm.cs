namespace QuickGraph.Algorithms
{
    using System.Collections.Generic;

    public interface IConnectedComponentAlgorithm<TVertex, TEdge, TGraph> : IAlgorithm<TGraph>
        where TGraph : IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        int ComponentCount { get; }

        IDictionary<TVertex, int> Components { get; }
    }
}