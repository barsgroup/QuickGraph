﻿namespace QuickGraph.Algorithms.MaximumFlow
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Services;

    public abstract class GraphAugmentorAlgorithmBase<TVertex, TEdge, TGraph>
        : AlgorithmBase<TGraph>
          ,
          IDisposable
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>
    {
        private readonly List<TEdge> _augmentedEdges = new List<TEdge>();

        public VertexFactory<TVertex> VertexFactory { get; }

        public EdgeFactory<TVertex, TEdge> EdgeFactory { get; }

        public TVertex SuperSource { get; private set; }

        public TVertex SuperSink { get; private set; }

        public bool Augmented { get; private set; }

        public ICollection<TEdge> AugmentedEdges => _augmentedEdges;

        protected GraphAugmentorAlgorithmBase(
            IAlgorithmComponent host,
            TGraph visitedGraph,
            VertexFactory<TVertex> vertexFactory,
            EdgeFactory<TVertex, TEdge> edgeFactory
        )
            : base(host, visitedGraph)
        {
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);

            VertexFactory = vertexFactory;
            EdgeFactory = edgeFactory;
        }

        public void Dispose()
        {
            Rollback();
        }

        public virtual void Rollback()
        {
            if (!Augmented)
            {
                return;
            }

            Augmented = false;
            VisitedGraph.RemoveVertex(SuperSource);
            VisitedGraph.RemoveVertex(SuperSink);
            SuperSource = default(TVertex);
            SuperSink = default(TVertex);
            _augmentedEdges.Clear();
        }

        protected void AddAugmentedEdge(TVertex source, TVertex target)
        {
            var edge = EdgeFactory(source, target);
            _augmentedEdges.Add(edge);
            VisitedGraph.AddEdge(edge);
            OnEdgeAdded(edge);
        }

        protected abstract void AugmentGraph();

        protected override void InternalCompute()
        {
            if (Augmented)
            {
                throw new InvalidOperationException("Graph already augmented");
            }

            SuperSource = VertexFactory();
            VisitedGraph.AddVertex(SuperSource);
            OnSuperSourceAdded(SuperSource);

            SuperSink = VertexFactory();
            VisitedGraph.AddVertex(SuperSink);
            OnSuperSinkAdded(SuperSink);

            AugmentGraph();
            Augmented = true;
        }

        private void OnEdgeAdded(TEdge e)
        {
            Contract.Requires(e != null);
            var eh = EdgeAdded;
            if (eh != null)
            {
                eh(e);
            }
        }

        private void OnSuperSinkAdded(TVertex v)
        {
            Contract.Requires(v != null);
            var eh = SuperSinkAdded;
            if (eh != null)
            {
                eh(v);
            }
        }

        private void OnSuperSourceAdded(TVertex v)
        {
            Contract.Requires(v != null);
            var eh = SuperSourceAdded;
            if (eh != null)
            {
                eh(v);
            }
        }

        public event EdgeAction<TVertex, TEdge> EdgeAdded;

        public event VertexAction<TVertex> SuperSinkAdded;

        public event VertexAction<TVertex> SuperSourceAdded;
    }
}