namespace QuickGraph.Algorithms.Search
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Services;

    /// <summary>A depth and height first search algorithm for directed graphs</summary>
    /// <remarks>
    ///     This is a modified version of the classic DFS algorithm where the search is performed both in depth and
    ///     height.
    /// </remarks>
    /// <reference-ref
    ///     idref="gross98graphtheory"
    ///     chapter="4.2" />
    public sealed class BidirectionalDepthFirstSearchAlgorithm<TVertex, TEdge> :
        RootedAlgorithmBase<TVertex, IBidirectionalGraph<TVertex, TEdge>>,
        IVertexColorizerAlgorithm<TVertex, TEdge>,
        ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        public IDictionary<TVertex, GraphColor> VertexColors { get; }

        public int MaxDepth { get; set; } = int.MaxValue;

        public BidirectionalDepthFirstSearchAlgorithm(IBidirectionalGraph<TVertex, TEdge> g)
            : this(g, new Dictionary<TVertex, GraphColor>())
        {
        }

        public BidirectionalDepthFirstSearchAlgorithm(
            IBidirectionalGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> colors
        )
            : this(null, visitedGraph, colors)
        {
        }

        public BidirectionalDepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IBidirectionalGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> colors
        )
            : base(host, visitedGraph)
        {
            Contract.Requires(colors != null);

            VertexColors = colors;
        }

        public GraphColor GetVertexColor(TVertex vertex)
        {
            return VertexColors[vertex];
        }

        public void Visit(TVertex u, int depth)
        {
            Contract.Requires(u != null);

            if (depth > MaxDepth)
            {
                return;
            }

            VertexColors[u] = GraphColor.Gray;
            OnDiscoverVertex(u);

            var cancelManager = Services.CancelManager;
            var v = default(TVertex);
            foreach (var e in VisitedGraph.OutEdges(u))
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }

                OnExamineEdge(e);
                v = e.Target;
                ProcessEdge(depth, v, e);
            }

            foreach (var e in VisitedGraph.InEdges(u))
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }

                OnExamineEdge(e);
                v = e.Source;
                ProcessEdge(depth, v, e);
            }

            VertexColors[u] = GraphColor.Black;
            OnFinishVertex(u);
        }

        protected override void Initialize()
        {
            base.Initialize();

            VertexColors.Clear();
            foreach (var u in VisitedGraph.Vertices)
            {
                VertexColors[u] = GraphColor.White;
                OnInitializeVertex(u);
            }
        }

        protected override void InternalCompute()
        {
            // put all vertex to white
            Initialize();

            // if there is a starting vertex, start whith him:
            TVertex rootVertex;
            if (TryGetRootVertex(out rootVertex))
            {
                OnStartVertex(rootVertex);
                Visit(rootVertex, 0);
            }

            // process each vertex 
            var cancelManager = Services.CancelManager;
            foreach (var u in VisitedGraph.Vertices)
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }
                if (VertexColors[u] == GraphColor.White)
                {
                    OnStartVertex(u);
                    Visit(u, 0);
                }
            }
        }

        private void OnBackEdge(TEdge e)
        {
            var eh = BackEdge;
            if (eh != null)
            {
                eh(e);
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

        private void OnFinishVertex(TVertex v)
        {
            var eh = FinishVertex;
            if (eh != null)
            {
                eh(v);
            }
        }

        private void OnForwardOrCrossEdge(TEdge e)
        {
            var eh = ForwardOrCrossEdge;
            if (eh != null)
            {
                eh(e);
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

        private void OnStartVertex(TVertex v)
        {
            var eh = StartVertex;
            if (eh != null)
            {
                eh(v);
            }
        }

        private void OnTreeEdge(TEdge e)
        {
            var eh = TreeEdge;
            if (eh != null)
            {
                eh(e);
            }
        }

        private void ProcessEdge(int depth, TVertex v, TEdge e)
        {
            var c = VertexColors[v];
            if (c == GraphColor.White)
            {
                OnTreeEdge(e);
                Visit(v, depth + 1);
            }
            else if (c == GraphColor.Gray)
            {
                OnBackEdge(e);
            }
            else
            {
                OnForwardOrCrossEdge(e);
            }
        }

        public event EdgeAction<TVertex, TEdge> BackEdge;

        public event VertexAction<TVertex> DiscoverVertex;

        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        public event VertexAction<TVertex> FinishVertex;

        public event EdgeAction<TVertex, TEdge> ForwardOrCrossEdge;

        public event VertexAction<TVertex> InitializeVertex;

        public event VertexAction<TVertex> StartVertex;

        public event EdgeAction<TVertex, TEdge> TreeEdge;
    }
}