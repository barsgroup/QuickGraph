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
    public sealed class VertexPredecessorPathRecorderObserver<TVertex, TEdge> :
        IObserver<IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly List<TVertex> _endPathVertices = new List<TVertex>();

        public IDictionary<TVertex, TEdge> VertexPredecessors { get; }

        public ICollection<TVertex> EndPathVertices => _endPathVertices;

        public VertexPredecessorPathRecorderObserver()
            : this(new Dictionary<TVertex, TEdge>())
        {
        }

        public VertexPredecessorPathRecorderObserver(
            IDictionary<TVertex, TEdge> vertexPredecessors)
        {
            Contract.Requires(vertexPredecessors != null);
            VertexPredecessors = vertexPredecessors;
        }

        public IEnumerable<IEnumerable<TEdge>> AllPaths()
        {
            var es = new List<IEnumerable<TEdge>>();
            foreach (var v in EndPathVertices)
            {
                IEnumerable<TEdge> path;
                if (VertexPredecessors.TryGetPath(v, out path))
                {
                    es.Add(path);
                }
            }
            return es;
        }

        public IDisposable Attach(IVertexPredecessorRecorderAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.TreeEdge += TreeEdge;
            algorithm.FinishVertex += FinishVertex;
            return new DisposableAction(
                () =>
                {
                    algorithm.TreeEdge -= TreeEdge;
                    algorithm.FinishVertex -= FinishVertex;
                });
        }

        private void FinishVertex(TVertex v)
        {
            foreach (var edge in VertexPredecessors.Values)
                if (edge.Source.Equals(v))
                {
                    return;
                }
            _endPathVertices.Add(v);
        }

        private void TreeEdge(TEdge e)
        {
            VertexPredecessors[e.Target] = e;
        }
    }
}