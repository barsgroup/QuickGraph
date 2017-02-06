namespace QuickGraph.Algorithms.ShortestPath
{
    using System;

    using QuickGraph.Algorithms.Services;

    /// <summary>A single-source shortest path algorithm for directed acyclic graph.</summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    /// <reference-ref
    ///     id="boost" />
    public sealed class DagShortestPathAlgorithm<TVertex, TEdge> :
        ShortestPathAlgorithmBase<TVertex, TEdge, IVertexListGraph<TVertex, TEdge>>,
        IVertexColorizerAlgorithm<TVertex, TEdge>,
        ITreeBuilderAlgorithm<TVertex, TEdge>,
        IDistanceRecorderAlgorithm<TVertex, TEdge>,
        IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        public DagShortestPathAlgorithm(
            IVertexListGraph<TVertex, TEdge> g,
            Func<TEdge, double> weights
        )
            : this(g, weights, DistanceRelaxers.ShortestDistance)
        {
        }

        public DagShortestPathAlgorithm(
            IVertexListGraph<TVertex, TEdge> g,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
        )
            : this(null, g, weights, distanceRelaxer)
        {
        }

        public DagShortestPathAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> g,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
        )
            : base(host, g, weights, distanceRelaxer)
        {
        }

        public void ComputeNoInit(TVertex s)
        {
            var orderedVertices = VisitedGraph.TopologicalSort();

            OnDiscoverVertex(s);
            foreach (var v in orderedVertices)
            {
                OnExamineVertex(v);
                foreach (var e in VisitedGraph.OutEdges(v))
                {
                    OnDiscoverVertex(e.Target);
                    var decreased = Relax(e);
                    if (decreased)
                    {
                        OnTreeEdge(e);
                    }
                    else
                    {
                        OnEdgeNotRelaxed(e);
                    }
                }
                OnFinishVertex(v);
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            // init color, distance
            var initialDistance = DistanceRelaxer.InitialDistance;
            foreach (var u in VisitedGraph.Vertices)
            {
                VertexColors.Add(u, GraphColor.White);
                Distances.Add(u, initialDistance);
                OnInitializeVertex(u);
            }
        }

        protected override void InternalCompute()
        {
            TVertex rootVertex;
            if (!TryGetRootVertex(out rootVertex))
            {
                throw new InvalidOperationException("RootVertex not initialized");
            }

            VertexColors[rootVertex] = GraphColor.Gray;
            Distances[rootVertex] = 0;
            ComputeNoInit(rootVertex);
        }

        private void OnDiscoverVertex(TVertex v)
        {
            if (DiscoverVertex != null)
            {
                DiscoverVertex(v);
            }
        }

        private void OnEdgeNotRelaxed(TEdge e)
        {
            if (EdgeNotRelaxed != null)
            {
                EdgeNotRelaxed(e);
            }
        }

        private void OnExamineEdge(TEdge e)
        {
            if (ExamineEdge != null)
            {
                ExamineEdge(e);
            }
        }

        private void OnExamineVertex(TVertex v)
        {
            if (ExamineVertex != null)
            {
                ExamineVertex(v);
            }
        }

        private void OnFinishVertex(TVertex v)
        {
            if (FinishVertex != null)
            {
                FinishVertex(v);
            }
        }

        private void OnInitializeVertex(TVertex v)
        {
            if (InitializeVertex != null)
            {
                InitializeVertex(v);
            }
        }

        private void OnStartVertex(TVertex v)
        {
            var eh = StartVertex;
            if (eh != null)
            {
                eh(v);
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