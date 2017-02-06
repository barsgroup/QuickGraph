﻿namespace QuickGraph.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Observers;
    using QuickGraph.Algorithms.Search;
    using QuickGraph.Algorithms.Services;

    /// <summary>Computes the dominator map of a directed graph</summary>
    /// <remarks>
    ///     Thomas Lengauer and Robert Endre Tarjan A fast algorithm for finding dominators in a flowgraph ACM
    ///     Transactions on Programming Language and Systems, 1(1):121-141, 1979.
    /// </remarks>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    internal class LengauerTarjanDominatorAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IBidirectionalGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        public LengauerTarjanDominatorAlgorithm(
            IAlgorithmComponent host,
            IBidirectionalGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

        public LengauerTarjanDominatorAlgorithm(
            IBidirectionalGraph<TVertex, TEdge> visitedGraph)
            : this(null, visitedGraph)
        {
        }

        protected override void InternalCompute()
        {
            var cancelManager = Services.CancelManager;
            var vertexCount = VisitedGraph.VertexCount;
            var vertices = VisitedGraph.Vertices;

            var timeStamps = new Dictionary<TVertex, int>(vertexCount);
            var stamps = new List<TVertex>(vertexCount);
            var predecessors = new Dictionary<TVertex, TEdge>(vertexCount);

            // phase 1: DFS over the graph and record vertex indices
            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(this, VisitedGraph);
            using (new TimeStampObserver(stamps).Attach(dfs))
            {
                using (new VertexTimeStamperObserver<TVertex, TEdge>(timeStamps).Attach(dfs))
                {
                    using (new VertexPredecessorRecorderObserver<TVertex, TEdge>(predecessors).Attach(dfs))
                    {
                        dfs.Compute();
                    }
                }
            }

            if (cancelManager.IsCancelling)
            {
                return;
            }

            // phase 2: find semidominators
            var semi = new Dictionary<TVertex, TVertex>(vertexCount);
            foreach (var v in vertices)
            {
                int vtime;
                TEdge dominatorEdge;
                if (!timeStamps.TryGetValue(v, out vtime) ||
                    !predecessors.TryGetValue(v, out dominatorEdge))
                {
                    continue; // skip unreachable
                }

                var dominator = dominatorEdge.Source;
                int dominatorTime;
                if (!timeStamps.TryGetValue(dominator, out dominatorTime))
                {
                    continue;
                }

                foreach (var e in VisitedGraph.InEdges(v))
                {
                    var u = e.Source;
                    int utime;
                    if (!timeStamps.TryGetValue(u, out utime))
                    {
                        continue;
                    }

                    TVertex candidate;
                    if (utime < vtime)
                    {
                        candidate = u;
                    }
                    else
                    {
                        var ancestor = default(TVertex);
                        candidate = semi[ancestor];
                    }
                    var candidateTime = timeStamps[candidate];
                    if (candidateTime < dominatorTime)
                    {
                        dominator = candidate;
                        dominatorTime = candidateTime;
                    }
                }

                semi[v] = dominator;
            }

            // phase 3:
        }

        private class TimeStampObserver
            : Observers.IObserver<IVertexTimeStamperAlgorithm<TVertex, TEdge>>

        {
            public readonly List<TVertex> Vertices;

            public TimeStampObserver(List<TVertex> vertices)
            {
                Contract.Requires(vertices != null);

                Vertices = vertices;
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
                Vertices.Add(v);
            }
        }
    }
}