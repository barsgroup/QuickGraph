namespace QuickGraph.Algorithms.Search
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Services;

    /// <summary>A edge depth first search algorithm for directed graphs</summary>
    /// <remarks>This is a variant of the classic DFS algorithm where the edges are color marked instead of the vertices.</remarks>
    /// <reference-ref
    ///     idref="gross98graphtheory"
    ///     chapter="4.2" />
    public sealed class EdgeDepthFirstSearchAlgorithm<TVertex, TEdge> :
        RootedAlgorithmBase<TVertex, IEdgeListAndIncidenceGraph<TVertex, TEdge>>,
        IEdgeColorizerAlgorithm<TVertex, TEdge>,
        IEdgePredecessorRecorderAlgorithm<TVertex, TEdge>,
        ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        public int MaxDepth { get; set; } = int.MaxValue;

        public IDictionary<TEdge, GraphColor> EdgeColors { get; }

        public EdgeDepthFirstSearchAlgorithm(IEdgeListAndIncidenceGraph<TVertex, TEdge> g)
            : this(g, new Dictionary<TEdge, GraphColor>())
        {
        }

        public EdgeDepthFirstSearchAlgorithm(
            IEdgeListAndIncidenceGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TEdge, GraphColor> colors
        )
            : this(null, visitedGraph, colors)
        {
        }

        public EdgeDepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IEdgeListAndIncidenceGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TEdge, GraphColor> colors
        )
            : base(host, visitedGraph)
        {
            Contract.Requires(colors != null);

            EdgeColors = colors;
        }

        public void Visit(TEdge se, int depth)
        {
            if (depth > MaxDepth)
            {
                return;
            }
            var cancelManager = Services.CancelManager;

            // mark edge as gray
            EdgeColors[se] = GraphColor.Gray;

            // add edge to the search tree
            OnTreeEdge(se);

            // iterate over out-edges
            foreach (var e in VisitedGraph.OutEdges(se.Target))
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }

                // check edge is not explored yet,
                // if not, explore it.
                if (EdgeColors[e] == GraphColor.White)
                {
                    OnDiscoverTreeEdge(se, e);
                    Visit(e, depth + 1);
                }
                else if (EdgeColors[e] == GraphColor.Gray)
                {
                    // edge is being explored
                    OnBackEdge(e);
                }
                else

                    // edge is black
                {
                    OnForwardOrCrossEdge(e);
                }
            }

            // all out-edges have been explored
            EdgeColors[se] = GraphColor.Black;
            OnFinishEdge(se);
        }

        protected override void Initialize()
        {
            // put all vertex to white
            var cancelManager = Services.CancelManager;
            foreach (var e in VisitedGraph.Edges)
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }
                EdgeColors[e] = GraphColor.White;
                OnInitializeEdge(e);
            }
        }

        protected override void InternalCompute()
        {
            Initialize();
            var cancelManager = Services.CancelManager;
            if (cancelManager.IsCancelling)
            {
                return;
            }

            // start whith him:
            TVertex rootVertex;
            if (TryGetRootVertex(out rootVertex))
            {
                OnStartVertex(rootVertex);

                // process each out edge of v
                foreach (var e in VisitedGraph.OutEdges(rootVertex))
                {
                    if (cancelManager.IsCancelling)
                    {
                        return;
                    }
                    if (EdgeColors[e] == GraphColor.White)
                    {
                        OnStartEdge(e);
                        Visit(e, 0);
                    }
                }
            }

            // process the rest of the graph edges
            foreach (var e in VisitedGraph.Edges)
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }
                if (EdgeColors[e] == GraphColor.White)
                {
                    OnStartEdge(e);
                    Visit(e, 0);
                }
            }
        }

        private void OnBackEdge(TEdge e)
        {
            if (BackEdge != null)
            {
                BackEdge(e);
            }
        }

        private void OnDiscoverTreeEdge(TEdge e, TEdge targetEge)
        {
            var eh = DiscoverTreeEdge;
            if (eh != null)
            {
                eh(e, targetEge);
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

        private void OnFinishEdge(TEdge e)
        {
            if (FinishEdge != null)
            {
                FinishEdge(e);
            }
        }

        private void OnForwardOrCrossEdge(TEdge e)
        {
            if (ForwardOrCrossEdge != null)
            {
                ForwardOrCrossEdge(e);
            }
        }

        private void OnInitializeEdge(TEdge e)
        {
            var eh = InitializeEdge;
            if (eh != null)
            {
                eh(e);
            }
        }

        private void OnStartEdge(TEdge e)
        {
            if (StartEdge != null)
            {
                StartEdge(e);
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

        public event EdgeAction<TVertex, TEdge> BackEdge;

        public event EdgeEdgeAction<TVertex, TEdge> DiscoverTreeEdge;

        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        public event EdgeAction<TVertex, TEdge> FinishEdge;

        public event EdgeAction<TVertex, TEdge> ForwardOrCrossEdge;

        public event EdgeAction<TVertex, TEdge> InitializeEdge;

        public event EdgeAction<TVertex, TEdge> StartEdge;

        public event VertexAction<TVertex> StartVertex;

        public event EdgeAction<TVertex, TEdge> TreeEdge;
    }
}