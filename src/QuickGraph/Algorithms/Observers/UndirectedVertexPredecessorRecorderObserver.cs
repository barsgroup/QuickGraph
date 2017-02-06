namespace QuickGraph.Algorithms.Observers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary></summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    /// <reference-ref
    ///     idref="boost" />
    public sealed class UndirectedVertexPredecessorRecorderObserver<TVertex, TEdge> :
        IObserver<IUndirectedTreeBuilderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        public IDictionary<TVertex, TEdge> VertexPredecessors { get; }

        public UndirectedVertexPredecessorRecorderObserver()
            : this(new Dictionary<TVertex, TEdge>())
        {
        }

        public UndirectedVertexPredecessorRecorderObserver(
            IDictionary<TVertex, TEdge> vertexPredecessors)
        {
            Contract.Requires(vertexPredecessors != null);

            VertexPredecessors = vertexPredecessors;
        }

        public IDisposable Attach(IUndirectedTreeBuilderAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.TreeEdge += TreeEdge;
            return new DisposableAction(
                () => algorithm.TreeEdge -= TreeEdge
            );
        }

        public bool TryGetPath(TVertex vertex, out IEnumerable<TEdge> path)
        {
            return VertexPredecessors.TryGetPath(vertex, out path);
        }

        private void TreeEdge(object sender, UndirectedEdgeEventArgs<TVertex, TEdge> e)
        {
            VertexPredecessors[e.Target] = e.Edge;
        }
    }
}