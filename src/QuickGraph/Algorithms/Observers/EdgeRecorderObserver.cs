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
    public sealed class EdgeRecorderObserver<TVertex, TEdge> :
        IObserver<ITreeBuilderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        public IList<TEdge> Edges { get; }

        public EdgeRecorderObserver()
            : this(new List<TEdge>())
        {
        }

        public EdgeRecorderObserver(IList<TEdge> edges)
        {
            Contract.Requires(edges != null);

            Edges = edges;
        }

        public IDisposable Attach(ITreeBuilderAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.TreeEdge += RecordEdge;
            return new DisposableAction(
                () => algorithm.TreeEdge -= RecordEdge
            );
        }

        private void RecordEdge(TEdge args)
        {
            Contract.Requires(args != null);

            Edges.Add(args);
        }
    }
}