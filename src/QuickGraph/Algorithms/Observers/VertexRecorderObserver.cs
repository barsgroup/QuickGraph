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
    public sealed class VertexRecorderObserver<TVertex, TEdge> :
        IObserver<IVertexTimeStamperAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly IList<TVertex> _vertices;

        public IEnumerable<TVertex> Vertices => _vertices;

        public VertexRecorderObserver()
            : this(new List<TVertex>())
        {
        }

        public VertexRecorderObserver(IList<TVertex> vertices)
        {
            Contract.Requires(vertices != null);

            _vertices = vertices;
        }

        public IDisposable Attach(IVertexTimeStamperAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.DiscoverVertex += algorithm_DiscoverVertex;
            return new DisposableAction(
                () => algorithm.DiscoverVertex -= algorithm_DiscoverVertex
            );
        }

        private void algorithm_DiscoverVertex(TVertex v)
        {
            _vertices.Add(v);
        }
    }
}