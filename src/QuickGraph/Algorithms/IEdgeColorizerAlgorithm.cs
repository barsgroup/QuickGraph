namespace QuickGraph.Algorithms
{
    using System.Collections.Generic;

    public interface IEdgeColorizerAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        IDictionary<TEdge, GraphColor> EdgeColors { get; }
    }
}