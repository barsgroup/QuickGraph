namespace QuickGraph.Algorithms.Search
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Services;

    /// <summary>A edge depth first search algorithm for implicit directed graphs</summary>
    /// <remarks>This is a variant of the classic DFS where the edges are color marked.</remarks>
    /// <reference-ref
    ///     idref="gross98graphtheory"
    ///     chapter="4.2" />
    public sealed class ImplicitEdgeDepthFirstSearchAlgorithm<TVertex, TEdge> :
        RootedAlgorithmBase<TVertex, IIncidenceGraph<TVertex, TEdge>>,
        ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>Gets the vertex color map</summary>
        /// <value>Vertex color (<see cref="GraphColor" />) dictionary</value>
        public IDictionary<TEdge, GraphColor> EdgeColors { get; } = new Dictionary<TEdge, GraphColor>();

        /// <summary>Gets or sets the maximum exploration depth, from the start vertex.</summary>
        /// <remarks>Defaulted at <c>int.MaxValue</c>.</remarks>
        /// <value>Maximum exploration depth.</value>
        public int MaxDepth { get; set; } = int.MaxValue;

        public ImplicitEdgeDepthFirstSearchAlgorithm(IIncidenceGraph<TVertex, TEdge> visitedGraph)
            : this(null, visitedGraph)
        {
        }

        public ImplicitEdgeDepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IIncidenceGraph<TVertex, TEdge> visitedGraph
        )
            : base(host, visitedGraph)
        {
        }

        /// <summary>Initializes the algorithm before computation.</summary>
        protected override void Initialize()
        {
            base.Initialize();

            EdgeColors.Clear();
        }

        protected override void InternalCompute()
        {
            TVertex rootVertex;
            if (!TryGetRootVertex(out rootVertex))
            {
                throw new InvalidOperationException("root vertex not set");
            }

            // initialize algorithm
            Initialize();

            // start whith him:
            OnStartVertex(rootVertex);

            var cancelManager = Services.CancelManager;

            // process each out edge of v
            foreach (var e in VisitedGraph.OutEdges(rootVertex))
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }

                if (!EdgeColors.ContainsKey(e))
                {
                    OnStartEdge(e);
                    Visit(e, 0);
                }
            }
        }

        /// <summary>Triggers the BackEdge event.</summary>
        /// <param name="e"></param>
        private void OnBackEdge(TEdge e)
        {
            if (BackEdge != null)
            {
                BackEdge(e);
            }
        }

        /// <summary>Triggers DiscoverEdge event</summary>
        /// <param name="se"></param>
        /// <param name="e"></param>
        private void OnDiscoverTreeEdge(TEdge se, TEdge e)
        {
            var eh = DiscoverTreeEdge;
            if (eh != null)
            {
                eh(se, e);
            }
        }

        /// <summary>Triggers the ForwardOrCrossEdge event.</summary>
        /// <param name="e"></param>
        private void OnFinishEdge(TEdge e)
        {
            if (FinishEdge != null)
            {
                FinishEdge(e);
            }
        }

        /// <summary>Triggers the ForwardOrCrossEdge event.</summary>
        /// <param name="e"></param>
        private void OnForwardOrCrossEdge(TEdge e)
        {
            if (ForwardOrCrossEdge != null)
            {
                ForwardOrCrossEdge(e);
            }
        }

        /// <summary>Triggers the StartEdge event.</summary>
        /// <param name="e"></param>
        private void OnStartEdge(TEdge e)
        {
            if (StartEdge != null)
            {
                StartEdge(e);
            }
        }

        /// <summary>Triggers the StartVertex event.</summary>
        /// <param name="v"></param>
        private void OnStartVertex(TVertex v)
        {
            if (StartVertex != null)
            {
                StartVertex(v);
            }
        }

        /// <summary>Triggers the TreeEdge event.</summary>
        /// <param name="e"></param>
        private void OnTreeEdge(TEdge e)
        {
            if (TreeEdge != null)
            {
                TreeEdge(e);
            }
        }

        /// <summary>Does a depth first search on the vertex u</summary>
        /// <param name="se">edge to explore</param>
        /// <param name="depth">current exploration depth</param>
        /// <exception cref="ArgumentNullException">se cannot be null</exception>
        private void Visit(TEdge se, int depth)
        {
            Contract.Requires(se != null);
            Contract.Requires(depth >= 0);

            if (depth > MaxDepth)
            {
                return;
            }

            // mark edge as gray
            EdgeColors[se] = GraphColor.Gray;

            // add edge to the search tree
            OnTreeEdge(se);

            var cancelManager = Services.CancelManager;

            // iterate over out-edges
            foreach (var e in VisitedGraph.OutEdges(se.Target))
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }

                // check edge is not explored yet,
                // if not, explore it.
                if (!EdgeColors.ContainsKey(e))
                {
                    OnDiscoverTreeEdge(se, e);
                    Visit(e, depth + 1);
                }
                else
                {
                    var c = EdgeColors[e];
                    if (EdgeColors[e] == GraphColor.Gray)
                    {
                        OnBackEdge(e);
                    }
                    else
                    {
                        OnForwardOrCrossEdge(e);
                    }
                }
            }

            // all out-edges have been explored
            EdgeColors[se] = GraphColor.Black;
            OnFinishEdge(se);
        }

        /// <summary>Invoked on the back edges in the graph.</summary>
        public event EdgeAction<TVertex, TEdge> BackEdge;

        /// <summary></summary>
        public event EdgeEdgeAction<TVertex, TEdge> DiscoverTreeEdge;

        /// <summary>
        ///     Invoked on a edge after all of its out edges have been added to the search tree and all of the adjacent
        ///     vertices have been discovered (but before their out-edges have been examined).
        /// </summary>
        public event EdgeAction<TVertex, TEdge> FinishEdge;

        /// <summary>Invoked on forward or cross edges in the graph. (In an undirected graph this method is never called.)</summary>
        public event EdgeAction<TVertex, TEdge> ForwardOrCrossEdge;

        /// <summary>Invoked on the first edge of a test case</summary>
        public event EdgeAction<TVertex, TEdge> StartEdge;

        /// <summary>Invoked on the source vertex once before the start of the search.</summary>
        public event VertexAction<TVertex> StartVertex;

        /// <summary>
        ///     Invoked on each edge as it becomes a member of the edges that form the search tree. If you wish to record
        ///     predecessors, do so at this event point.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> TreeEdge;
    }
}