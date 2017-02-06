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
    public sealed class VertexPredecessorRecorderObserver<TVertex, TEdge> :
        IObserver<ITreeBuilderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<TVertex, TEdge> _vertexPredecessors;

        public IDictionary<TVertex, TEdge> VertexPredecessors => _vertexPredecessors;

        public VertexPredecessorRecorderObserver()
            : this(new Dictionary<TVertex, TEdge>())
        {
        }

        public VertexPredecessorRecorderObserver(
            Dictionary<TVertex, TEdge> vertexPredecessors)
        {
            Contract.Requires(vertexPredecessors != null);

            _vertexPredecessors = vertexPredecessors;
        }

        public IDisposable Attach(ITreeBuilderAlgorithm<TVertex, TEdge> algorithm)
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

        private void TreeEdge(TEdge e)
        {
            _vertexPredecessors[e.Target] = e;
        }
    }
}