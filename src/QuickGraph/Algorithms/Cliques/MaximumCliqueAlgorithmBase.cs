namespace QuickGraph.Algorithms.Cliques
{
    using QuickGraph.Algorithms.Services;

    public abstract class MaximumCliqueAlgorithmBase<TVertex, TEdge>
        : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        protected MaximumCliqueAlgorithmBase(IAlgorithmComponent host, IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

        protected MaximumCliqueAlgorithmBase(IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
        }
    }
}