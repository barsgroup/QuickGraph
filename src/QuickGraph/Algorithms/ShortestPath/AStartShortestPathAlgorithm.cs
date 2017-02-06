﻿namespace QuickGraph.Algorithms.ShortestPath
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Search;
    using QuickGraph.Algorithms.Services;
    using QuickGraph.Collections;

    /// <summary>A* single-source shortest path algorithm for directed graph with positive distance.</summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    /// <reference-ref
    ///     idref="lawler01combinatorial" />
    public sealed class AStarShortestPathAlgorithm<TVertex, TEdge>
        : ShortestPathAlgorithmBase<TVertex, TEdge, IVertexListGraph<TVertex, TEdge>>
          ,
          IVertexColorizerAlgorithm<TVertex, TEdge>
          ,
          IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
          ,
          IDistanceRecorderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private Dictionary<TVertex, double> _costs;

        private FibonacciQueue<TVertex, double> _vertexQueue;

        public Func<TVertex, double> CostHeuristic { get; }

        public AStarShortestPathAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            Func<TVertex, double> costHeuristic
        )
            : this(visitedGraph, weights, costHeuristic, DistanceRelaxers.ShortestDistance)
        {
        }

        public AStarShortestPathAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            Func<TVertex, double> costHeuristic,
            IDistanceRelaxer distanceRelaxer
        )
            : this(null, visitedGraph, weights, costHeuristic, distanceRelaxer)
        {
        }

        public AStarShortestPathAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            Func<TVertex, double> costHeuristic,
            IDistanceRelaxer distanceRelaxer
        )
            : base(host, visitedGraph, weights, distanceRelaxer)
        {
            Contract.Requires(costHeuristic != null);

            CostHeuristic = costHeuristic;
        }

        public void ComputeNoInit(TVertex s)
        {
            BreadthFirstSearchAlgorithm<TVertex, TEdge> bfs = null;

            try
            {
                bfs = new BreadthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    _vertexQueue,
                    VertexColors
                );

                bfs.InitializeVertex += InitializeVertex;
                bfs.DiscoverVertex += DiscoverVertex;
                bfs.StartVertex += StartVertex;
                bfs.ExamineEdge += ExamineEdge;
                bfs.ExamineVertex += ExamineVertex;
                bfs.FinishVertex += FinishVertex;

                bfs.ExamineEdge += InternalExamineEdge;
                bfs.TreeEdge += InternalTreeEdge;
                bfs.GrayTarget += InternalGrayTarget;
                bfs.BlackTarget += InternalBlackTarget;

                bfs.Visit(s);
            }
            finally
            {
                if (bfs != null)
                {
                    bfs.InitializeVertex -= InitializeVertex;
                    bfs.DiscoverVertex -= DiscoverVertex;
                    bfs.StartVertex -= StartVertex;
                    bfs.ExamineEdge -= ExamineEdge;
                    bfs.ExamineVertex -= ExamineVertex;
                    bfs.FinishVertex -= FinishVertex;

                    bfs.ExamineEdge -= InternalExamineEdge;
                    bfs.TreeEdge -= InternalTreeEdge;
                    bfs.GrayTarget -= InternalGrayTarget;
                    bfs.BlackTarget -= InternalBlackTarget;
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            VertexColors.Clear();
            _costs = new Dictionary<TVertex, double>(VisitedGraph.VertexCount);

            // init color, distance
            var initialDistance = DistanceRelaxer.InitialDistance;
            foreach (var u in VisitedGraph.Vertices)
            {
                VertexColors.Add(u, GraphColor.White);
                Distances.Add(u, initialDistance);
                _costs.Add(u, initialDistance);
            }
            _vertexQueue = new FibonacciQueue<TVertex, double>(_costs, DistanceRelaxer.Compare);
        }

        protected override void InternalCompute()
        {
            TVertex rootVertex;
            if (TryGetRootVertex(out rootVertex))
            {
                ComputeFromRoot(rootVertex);
            }
            else
            {
                foreach (var v in VisitedGraph.Vertices)
                    if (VertexColors[v] == GraphColor.White)
                    {
                        ComputeFromRoot(v);
                    }
            }
        }

        private void ComputeFromRoot(TVertex rootVertex)
        {
            Contract.Requires(rootVertex != null);
            Contract.Requires(VisitedGraph.ContainsVertex(rootVertex));
            Contract.Requires(VertexColors[rootVertex] == GraphColor.White);

            VertexColors[rootVertex] = GraphColor.Gray;
            Distances[rootVertex] = 0;
            ComputeNoInit(rootVertex);
        }

        private void InternalBlackTarget(TEdge e)
        {
            var target = e.Target;

            var decreased = Relax(e);
            var distance = Distances[target];
            if (decreased)
            {
                OnTreeEdge(e);
                _costs[target] = DistanceRelaxer.Combine(distance, CostHeuristic(target));
                _vertexQueue.Enqueue(target);
                VertexColors[target] = GraphColor.Gray;
            }
            else
            {
                OnEdgeNotRelaxed(e);
            }
        }

        private void InternalExamineEdge(TEdge args)
        {
            if (Weights(args) < 0)
            {
                throw new NegativeWeightException();
            }
        }

        private void InternalGrayTarget(TEdge e)
        {
            var target = e.Target;

            var decreased = Relax(e);
            var distance = Distances[target];
            if (decreased)
            {
                _costs[target] = DistanceRelaxer.Combine(distance, CostHeuristic(target));
                _vertexQueue.Update(target);
                OnTreeEdge(e);
            }
            else
            {
                OnEdgeNotRelaxed(e);
            }
        }

        private void InternalTreeEdge(TEdge args)
        {
            var decreased = Relax(args);
            if (decreased)
            {
                OnTreeEdge(args);
            }
            else
            {
                OnEdgeNotRelaxed(args);
            }
        }

        private void OnEdgeNotRelaxed(TEdge e)
        {
            var eh = EdgeNotRelaxed;
            if (eh != null)
            {
                eh(e);
            }
        }

        public event VertexAction<TVertex> DiscoverVertex;

        public event EdgeAction<TVertex, TEdge> EdgeNotRelaxed;

        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        public event VertexAction<TVertex> ExamineVertex;

        public event VertexAction<TVertex> FinishVertex;

        public event VertexAction<TVertex> InitializeVertex;

        public event VertexAction<TVertex> StartVertex;
    }
}