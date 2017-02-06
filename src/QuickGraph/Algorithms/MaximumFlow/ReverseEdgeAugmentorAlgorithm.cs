﻿namespace QuickGraph.Algorithms.MaximumFlow
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Services;

    public sealed class ReversedEdgeAugmentorAlgorithm<TVertex, TEdge>
        : IDisposable
        where TEdge : IEdge<TVertex>
    {
        private readonly IList<TEdge> _augmentedEgdes = new List<TEdge>();

        public IMutableVertexAndEdgeListGraph<TVertex, TEdge> VisitedGraph { get; }

        public EdgeFactory<TVertex, TEdge> EdgeFactory { get; }

        public ICollection<TEdge> AugmentedEdges => _augmentedEgdes;

        public Dictionary<TEdge, TEdge> ReversedEdges { get; } = new Dictionary<TEdge, TEdge>();

        public bool Augmented { get; private set; }

        public ReversedEdgeAugmentorAlgorithm(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            EdgeFactory<TVertex, TEdge> edgeFactory)
            : this(null, visitedGraph, edgeFactory)
        {
        }

        public ReversedEdgeAugmentorAlgorithm(
            IAlgorithmComponent host,
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            EdgeFactory<TVertex, TEdge> edgeFactory)
        {
            Contract.Requires(visitedGraph != null);
            Contract.Requires(edgeFactory != null);

            VisitedGraph = visitedGraph;
            EdgeFactory = edgeFactory;
        }

        public void AddReversedEdges()
        {
            if (Augmented)
            {
                throw new InvalidOperationException("Graph already augmented");
            }

            // step 1, find edges that need reversing
            IList<TEdge> notReversedEdges = new List<TEdge>();
            foreach (var edge in VisitedGraph.Edges)
            {
                // if reversed already found, continue
                if (ReversedEdges.ContainsKey(edge))
                {
                    continue;
                }

                var reversedEdge = FindReversedEdge(edge);
                if (reversedEdge != null)
                {
                    // setup edge
                    ReversedEdges[edge] = reversedEdge;

                    // setup reversed if needed
                    if (!ReversedEdges.ContainsKey(reversedEdge))
                    {
                        ReversedEdges[reversedEdge] = edge;
                    }
                    continue;
                }

                // this edge has no reverse
                notReversedEdges.Add(edge);
            }

            // step 2, go over each not reversed edge, add reverse
            foreach (var edge in notReversedEdges)
            {
                if (ReversedEdges.ContainsKey(edge))
                {
                    continue;
                }

                // already been added
                var reversedEdge = FindReversedEdge(edge);
                if (reversedEdge != null)
                {
                    ReversedEdges[edge] = reversedEdge;
                    continue;
                }

                // need to create one
                reversedEdge = EdgeFactory(edge.Target, edge.Source);
                if (!VisitedGraph.AddEdge(reversedEdge))
                {
                    throw new InvalidOperationException("We should not be here");
                }
                _augmentedEgdes.Add(reversedEdge);
                ReversedEdges[edge] = reversedEdge;
                ReversedEdges[reversedEdge] = edge;
                OnReservedEdgeAdded(reversedEdge);
            }

            Augmented = true;
        }

        public void RemoveReversedEdges()
        {
            if (!Augmented)
            {
                throw new InvalidOperationException("Graph is not yet augmented");
            }

            foreach (var edge in _augmentedEgdes)
                VisitedGraph.RemoveEdge(edge);

            _augmentedEgdes.Clear();
            ReversedEdges.Clear();

            Augmented = false;
        }

        void IDisposable.Dispose()
        {
            if (Augmented)
            {
                RemoveReversedEdges();
            }
        }

        private TEdge FindReversedEdge(TEdge edge)
        {
            foreach (var redge in VisitedGraph.OutEdges(edge.Target))
                if (redge.Target.Equals(edge.Source))
                {
                    return redge;
                }
            return default(TEdge);
        }

        private void OnReservedEdgeAdded(TEdge e)
        {
            var eh = ReversedEdgeAdded;
            if (eh != null)
            {
                eh(e);
            }
        }

        public event EdgeAction<TVertex, TEdge> ReversedEdgeAdded;
    }
}