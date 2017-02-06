namespace QuickGraph.Algorithms.ShortestPath
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Search;
    using QuickGraph.Algorithms.Services;
    using QuickGraph.Collections;

    /// <summary>A single-source shortest path algorithm for undirected graph with positive distance.</summary>
    /// <reference-ref
    ///     idref="lawler01combinatorial" />
    public sealed class UndirectedDijkstraShortestPathAlgorithm<TVertex, TEdge>
        : UndirectedShortestPathAlgorithmBase<TVertex, TEdge>
          ,
          IVertexColorizerAlgorithm<TVertex, TEdge>
          ,
          IUndirectedVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
          ,
          IDistanceRecorderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private IPriorityQueue<TVertex> _vertexQueue;

        public UndirectedDijkstraShortestPathAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights)
            : this(visitedGraph, weights, DistanceRelaxers.ShortestDistance)
        {
        }

        public UndirectedDijkstraShortestPathAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
        )
            : this(null, visitedGraph, weights, distanceRelaxer)
        {
        }

        public UndirectedDijkstraShortestPathAlgorithm(
            IAlgorithmComponent host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
        )
            : base(host, visitedGraph, weights, distanceRelaxer)
        {
        }

        public void ComputeNoInit(TVertex s)
        {
            Contract.Requires(s != null);
            Contract.Requires(VisitedGraph.ContainsVertex(s));

            UndirectedBreadthFirstSearchAlgorithm<TVertex, TEdge> bfs = null;
            try
            {
                bfs = new UndirectedBreadthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    _vertexQueue,
                    VertexColors
                );

                bfs.InitializeVertex += InitializeVertex;
                bfs.DiscoverVertex += DiscoverVertex;
                bfs.StartVertex += StartVertex;
                bfs.ExamineEdge += ExamineEdge;
#if DEBUG
                bfs.ExamineEdge += e => AssertHeap();
#endif
                bfs.ExamineVertex += ExamineVertex;
                bfs.FinishVertex += FinishVertex;

                bfs.TreeEdge += InternalTreeEdge;
                bfs.GrayTarget += InternalGrayTarget;

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

                    bfs.TreeEdge -= InternalTreeEdge;
                    bfs.GrayTarget -= InternalGrayTarget;
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            var initialDistance = DistanceRelaxer.InitialDistance;

            // init color, distance
            foreach (var u in VisitedGraph.Vertices)
            {
                VertexColors.Add(u, GraphColor.White);
                Distances.Add(u, initialDistance);
            }
            _vertexQueue = new FibonacciQueue<TVertex, double>(DistancesIndexGetter());
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

        [Conditional("DEBUG")]
        private void AssertHeap()
        {
            if (_vertexQueue.Count == 0)
            {
                return;
            }

            var top = _vertexQueue.Peek();
            var vertices = _vertexQueue.ToArray();
            for (var i = 1; i < vertices.Length; ++i)
                if (Distances[top] > Distances[vertices[i]])
                {
                    Contract.Assert(false);
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

        private void InternalGrayTarget(object sender, UndirectedEdgeEventArgs<TVertex, TEdge> args)
        {
            Contract.Requires(args != null);

            var decreased = Relax(args.Edge, args.Source, args.Target);
            if (decreased)
            {
                _vertexQueue.Update(args.Target);
                AssertHeap();
                OnTreeEdge(args.Edge, args.Reversed);
            }
            else
            {
                OnEdgeNotRelaxed(args.Edge, args.Reversed);
            }
        }

        private void InternalTreeEdge(object sender, UndirectedEdgeEventArgs<TVertex, TEdge> args)
        {
            Contract.Requires(args != null);

            var decreased = Relax(args.Edge, args.Source, args.Target);
            if (decreased)
            {
                OnTreeEdge(args.Edge, args.Reversed);
            }
            else
            {
                OnEdgeNotRelaxed(args.Edge, args.Reversed);
            }
        }

        private void OnEdgeNotRelaxed(TEdge e, bool reversed)
        {
            var eh = EdgeNotRelaxed;
            if (eh != null)
            {
                eh(this, new UndirectedEdgeEventArgs<TVertex, TEdge>(e, reversed));
            }
        }

        public event VertexAction<TVertex> DiscoverVertex;

        public event UndirectedEdgeAction<TVertex, TEdge> EdgeNotRelaxed;

        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        public event VertexAction<TVertex> ExamineVertex;

        public event VertexAction<TVertex> FinishVertex;

        public event VertexAction<TVertex> InitializeVertex;

        public event VertexAction<TVertex> StartVertex;
    }
}