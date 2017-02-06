﻿namespace QuickGraph.Algorithms.MaximumFlow
{
    using System;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Observers;
    using QuickGraph.Algorithms.Search;
    using QuickGraph.Algorithms.Services;
    using QuickGraph.Collections;
    using QuickGraph.Predicates;

    /// <summary>Edmond and Karp maximum flow algorithm for directed graph with positive capacities and flows.</summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    public sealed class EdmondsKarpMaximumFlowAlgorithm<TVertex, TEdge>
        : MaximumFlowAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private IVertexListGraph<TVertex, TEdge> ResidualGraph
        {
            get
            {
                return new FilteredVertexListGraph<
                    TVertex,
                    TEdge,
                    IVertexListGraph<TVertex, TEdge>
                >(
                    VisitedGraph,
                    v => true,
                    new ResidualEdgePredicate<TVertex, TEdge>(ResidualCapacities).Test
                );
            }
        }

        public EdmondsKarpMaximumFlowAlgorithm(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            Func<TEdge, double> capacities,
            EdgeFactory<TVertex, TEdge> edgeFactory
        )
            : this(null, g, capacities, edgeFactory)
        {
        }

        public EdmondsKarpMaximumFlowAlgorithm(
            IAlgorithmComponent host,
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            Func<TEdge, double> capacities,
            EdgeFactory<TVertex, TEdge> edgeFactory
        )
            : base(host, g, capacities, edgeFactory)
        {
        }

        /// <summary>Computes the maximum flow between Source and Sink.</summary>
        /// <returns></returns>
        protected override void InternalCompute()
        {
            if (Source == null)
            {
                throw new InvalidOperationException("Source is not specified");
            }
            if (Sink == null)
            {
                throw new InvalidOperationException("Sink is not specified");
            }

            if (Services.CancelManager.IsCancelling)
            {
                return;
            }

            var g = VisitedGraph;
            foreach (var u in g.Vertices)
            foreach (var e in g.OutEdges(u))
            {
                var capacity = Capacities(e);
                if (capacity < 0)
                {
                    throw new InvalidOperationException("negative edge capacity");
                }
                ResidualCapacities[e] = capacity;
            }

            VertexColors[Sink] = GraphColor.Gray;
            while (VertexColors[Sink] != GraphColor.White)
            {
                var vis = new VertexPredecessorRecorderObserver<TVertex, TEdge>(
                    Predecessors
                );
                var queue = new Queue<TVertex>();
                var bfs = new BreadthFirstSearchAlgorithm<TVertex, TEdge>(
                    ResidualGraph,
                    queue,
                    VertexColors
                );
                using (vis.Attach(bfs))
                {
                    bfs.Compute(Source);
                }

                if (VertexColors[Sink] != GraphColor.White)
                {
                    Augment(Source, Sink);
                }
            } // while

            MaxFlow = 0;
            foreach (var e in g.OutEdges(Source))
                MaxFlow += Capacities(e) - ResidualCapacities[e];
        }

        private void Augment(
            TVertex source,
            TVertex sink
        )
        {
            Contract.Requires(source != null);
            Contract.Requires(sink != null);

            TEdge e;
            TVertex u;

            // find minimum residual capacity along the augmenting path
            var delta = double.MaxValue;
            u = sink;
            do
            {
                e = Predecessors[u];
                delta = Math.Min(delta, ResidualCapacities[e]);
                u = e.Source;
            }
            while (!u.Equals(source));

            // push delta units of flow along the augmenting path
            u = sink;
            do
            {
                e = Predecessors[u];
                ResidualCapacities[e] -= delta;
                if (ReversedEdges != null && ReversedEdges.ContainsKey(e))
                {
                    ResidualCapacities[ReversedEdges[e]] += delta;
                }
                u = e.Source;
            }
            while (!u.Equals(source));
        }
    }
}