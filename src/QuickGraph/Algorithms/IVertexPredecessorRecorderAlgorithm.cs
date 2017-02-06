namespace QuickGraph.Algorithms
{
    public interface IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
        : ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        event VertexAction<TVertex> FinishVertex;

        event VertexAction<TVertex> StartVertex;
    }
}