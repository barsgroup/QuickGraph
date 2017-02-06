namespace QuickGraph.Algorithms.Search
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Services;
    using QuickGraph.Collections;

    /// <summary>A breath first search algorithm for undirected graphs</summary>
    /// <reference-ref
    ///     idref="gross98graphtheory"
    ///     chapter="4.2" />
    public sealed class UndirectedBreadthFirstSearchAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IUndirectedGraph<TVertex, TEdge>>
          ,
          IUndirectedVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
          ,
          IDistanceRecorderAlgorithm<TVertex, TEdge>
          ,
          IVertexColorizerAlgorithm<TVertex, TEdge>
          ,
          IUndirectedTreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly IQueue<TVertex> _vertexQueue;

        public IDictionary<TVertex, GraphColor> VertexColors { get; }

        public UndirectedBreadthFirstSearchAlgorithm(IUndirectedGraph<TVertex, TEdge> g)
            : this(g, new Collections.Queue<TVertex>(), new Dictionary<TVertex, GraphColor>())
        {
        }

        public UndirectedBreadthFirstSearchAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IQueue<TVertex> vertexQueue,
            IDictionary<TVertex, GraphColor> vertexColors
        )
            : this(null, visitedGraph, vertexQueue, vertexColors)
        {
        }

        public UndirectedBreadthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IQueue<TVertex> vertexQueue,
            IDictionary<TVertex, GraphColor> vertexColors
        )
            : base(host, visitedGraph)
        {
            Contract.Requires(vertexQueue != null);
            Contract.Requires(vertexColors != null);

            VertexColors = vertexColors;
            _vertexQueue = vertexQueue;
        }

        public GraphColor GetVertexColor(TVertex vertex)
        {
            return VertexColors[vertex];
        }

        public void Visit(TVertex s)
        {
            EnqueueRoot(s);
            FlushVisitQueue();
        }

        protected override void Initialize()
        {
            base.Initialize();

            // initialize vertex u
            var cancelManager = Services.CancelManager;
            if (cancelManager.IsCancelling)
            {
                return;
            }
            foreach (var v in VisitedGraph.Vertices)
            {
                VertexColors[v] = GraphColor.White;
                OnInitializeVertex(v);
            }
        }

        protected override void InternalCompute()
        {
            TVertex rootVertex;
            if (!TryGetRootVertex(out rootVertex))
            {
                throw new InvalidOperationException("missing root vertex");
            }
            EnqueueRoot(rootVertex);
            FlushVisitQueue();
        }

        private void EnqueueRoot(TVertex s)
        {
            OnStartVertex(s);
            VertexColors[s] = GraphColor.Gray;
            OnDiscoverVertex(s);
            _vertexQueue.Enqueue(s);
        }

        private void FlushVisitQueue()
        {
            var cancelManager = Services.CancelManager;

            while (_vertexQueue.Count > 0)
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }

                var u = _vertexQueue.Dequeue();

                OnExamineVertex(u);
                foreach (var e in VisitedGraph.AdjacentEdges(u))
                {
                    var reversed = e.Target.Equals(u);
                    var v = reversed
                                ? e.Source
                                : e.Target;
                    OnExamineEdge(e);

                    var vColor = VertexColors[v];
                    if (vColor == GraphColor.White)
                    {
                        OnTreeEdge(e, reversed);
                        VertexColors[v] = GraphColor.Gray;
                        OnDiscoverVertex(v);
                        _vertexQueue.Enqueue(v);
                    }
                    else
                    {
                        OnNonTreeEdge(e, reversed);
                        if (vColor == GraphColor.Gray)
                        {
                            OnGrayTarget(e, reversed);
                        }
                        else
                        {
                            OnBlackTarget(e, reversed);
                        }
                    }
                }
                VertexColors[u] = GraphColor.Black;
                OnFinishVertex(u);
            }
        }

        private void OnBlackTarget(TEdge e, bool reversed)
        {
            var eh = BlackTarget;
            if (eh != null)
            {
                eh(this, new UndirectedEdgeEventArgs<TVertex, TEdge>(e, reversed));
            }
        }

        private void OnDiscoverVertex(TVertex v)
        {
            var eh = DiscoverVertex;
            if (eh != null)
            {
                eh(v);
            }
        }

        private void OnExamineEdge(TEdge e)
        {
            var eh = ExamineEdge;
            if (eh != null)
            {
                eh(e);
            }
        }

        private void OnExamineVertex(TVertex v)
        {
            var eh = ExamineVertex;
            if (eh != null)
            {
                eh(v);
            }
        }

        private void OnFinishVertex(TVertex v)
        {
            var eh = FinishVertex;
            if (eh != null)
            {
                eh(v);
            }
        }

        private void OnGrayTarget(TEdge e, bool reversed)
        {
            var eh = GrayTarget;
            if (eh != null)
            {
                eh(this, new UndirectedEdgeEventArgs<TVertex, TEdge>(e, reversed));
            }
        }

        private void OnInitializeVertex(TVertex v)
        {
            var eh = InitializeVertex;
            if (eh != null)
            {
                eh(v);
            }
        }

        private void OnNonTreeEdge(TEdge e, bool reversed)
        {
            var eh = NonTreeEdge;
            if (eh != null)
            {
                eh(this, new UndirectedEdgeEventArgs<TVertex, TEdge>(e, reversed));
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

        private void OnTreeEdge(TEdge e, bool reversed)
        {
            var eh = TreeEdge;
            if (eh != null)
            {
                eh(this, new UndirectedEdgeEventArgs<TVertex, TEdge>(e, reversed));
            }
        }

        public event UndirectedEdgeAction<TVertex, TEdge> BlackTarget;

        public event VertexAction<TVertex> DiscoverVertex;

        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        public event VertexAction<TVertex> ExamineVertex;

        public event VertexAction<TVertex> FinishVertex;

        public event UndirectedEdgeAction<TVertex, TEdge> GrayTarget;

        public event VertexAction<TVertex> InitializeVertex;

        public event UndirectedEdgeAction<TVertex, TEdge> NonTreeEdge;

        public event VertexAction<TVertex> StartVertex;

        public event UndirectedEdgeAction<TVertex, TEdge> TreeEdge;
    }
}