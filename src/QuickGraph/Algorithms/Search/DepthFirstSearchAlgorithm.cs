﻿namespace QuickGraph.Algorithms.Search
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Services;

    /// <summary>A depth first search algorithm for directed graph</summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    /// <reference-ref
    ///     idref="gross98graphtheory"
    ///     chapter="4.2" />
    public sealed class DepthFirstSearchAlgorithm<TVertex, TEdge> :
        RootedAlgorithmBase<TVertex, IVertexListGraph<TVertex, TEdge>>,
        IDistanceRecorderAlgorithm<TVertex, TEdge>,
        IVertexColorizerAlgorithm<TVertex, TEdge>,
        IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>,
        IVertexTimeStamperAlgorithm<TVertex, TEdge>,
        ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private int _maxDepth = int.MaxValue;

        public IDictionary<TVertex, GraphColor> VertexColors { get; }

        public Func<IEnumerable<TEdge>, IEnumerable<TEdge>> OutEdgeEnumerator { get; }

        public int MaxDepth
        {
            get { return _maxDepth; }
            set
            {
                Contract.Requires(value > 0);
                _maxDepth = value;
            }
        }

        /// <summary>Initializes a new instance of the algorithm.</summary>
        /// <param name="visitedGraph">visited graph</param>
        public DepthFirstSearchAlgorithm(IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TVertex, GraphColor>())
        {
        }

        /// <summary>Initializes a new instance of the algorithm.</summary>
        /// <param name="visitedGraph">visited graph</param>
        /// <param name="colors">vertex color map</param>
        public DepthFirstSearchAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> colors
        )
            : this(null, visitedGraph, colors)
        {
        }

        /// <summary>Initializes a new instance of the algorithm.</summary>
        /// <param name="host">algorithm host</param>
        /// <param name="visitedGraph">visited graph</param>
        public DepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph
        )
            : this(host, visitedGraph, new Dictionary<TVertex, GraphColor>(), e => e)
        {
        }

        /// <summary>Initializes a new instance of the algorithm.</summary>
        /// <param name="host">algorithm host</param>
        /// <param name="visitedGraph">visited graph</param>
        /// <param name="colors">vertex color map</param>
        public DepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> colors
        )
            : this(host, visitedGraph, colors, e => e)
        {
        }

        /// <summary>Initializes a new instance of the algorithm.</summary>
        /// <param name="host">algorithm host</param>
        /// <param name="visitedGraph">visited graph</param>
        /// <param name="colors">vertex color map</param>
        /// <param name="outEdgeEnumerator">
        ///     Delegate that takes the enumeration of out-edges and reorders them. All vertices passed
        ///     to the method should be enumerated once and only once. May be null.
        /// </param>
        public DepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> colors,
            Func<IEnumerable<TEdge>, IEnumerable<TEdge>> outEdgeEnumerator
        )
            : base(host, visitedGraph)
        {
            Contract.Requires(colors != null);
            Contract.Requires(outEdgeEnumerator != null);

            VertexColors = colors;
            OutEdgeEnumerator = outEdgeEnumerator;
        }

        public GraphColor GetVertexColor(TVertex vertex)
        {
            return VertexColors[vertex];
        }

        public void Visit(TVertex root)
        {
            Contract.Requires(root != null);

            var todo = new Stack<SearchFrame>();
            var oee = OutEdgeEnumerator;
            VertexColors[root] = GraphColor.Gray;
            OnDiscoverVertex(root);

            var cancelManager = Services.CancelManager;
            var enumerable = oee(VisitedGraph.OutEdges(root));
            todo.Push(new SearchFrame(root, enumerable.GetEnumerator(), 0));
            while (todo.Count > 0)
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }

                var frame = todo.Pop();
                var u = frame.Vertex;
                var depth = frame.Depth;
                var edges = frame.Edges;

                if (depth > MaxDepth)
                {
                    if (edges != null)
                    {
                        edges.Dispose();
                    }
                    VertexColors[u] = GraphColor.Black;
                    OnFinishVertex(u);
                    continue;
                }

                while (edges.MoveNext())
                {
                    var e = edges.Current;
                    if (cancelManager.IsCancelling)
                    {
                        return;
                    }

                    OnExamineEdge(e);
                    var v = e.Target;
                    var c = VertexColors[v];
                    switch (c)
                    {
                        case GraphColor.White:
                            OnTreeEdge(e);
                            todo.Push(new SearchFrame(u, edges, depth));
                            u = v;
                            edges = oee(VisitedGraph.OutEdges(u)).GetEnumerator();
                            depth++;
                            VertexColors[u] = GraphColor.Gray;
                            OnDiscoverVertex(u);
                            break;
                        case GraphColor.Gray:
                            OnBackEdge(e);
                            break;
                        case GraphColor.Black:
                            OnForwardOrCrossEdge(e);
                            break;
                    }
                }
                if (edges != null)
                {
                    edges.Dispose();
                }

                VertexColors[u] = GraphColor.Black;
                OnFinishVertex(u);
            }
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
            // if there is a starting vertex, start whith him:
            TVertex rootVertex;
            if (TryGetRootVertex(out rootVertex))
            {
                OnStartVertex(rootVertex);
                Visit(rootVertex);
            }
            else
            {
                var cancelManager = Services.CancelManager;

                // process each vertex 
                foreach (var u in VisitedGraph.Vertices)
                {
                    if (cancelManager.IsCancelling)
                    {
                        return;
                    }
                    if (VertexColors[u] == GraphColor.White)
                    {
                        OnStartVertex(u);
                        Visit(u);
                    }
                }
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(MaxDepth > 0);
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

        public event EdgeAction<TVertex, TEdge> BackEdge;

        public event VertexAction<TVertex> DiscoverVertex;

        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        public event VertexAction<TVertex> FinishVertex;

        public event EdgeAction<TVertex, TEdge> ForwardOrCrossEdge;

        public event VertexAction<TVertex> InitializeVertex;

        public event VertexAction<TVertex> StartVertex;

        public event EdgeAction<TVertex, TEdge> TreeEdge;

        private struct SearchFrame
        {
            public readonly TVertex Vertex;

            public readonly IEnumerator<TEdge> Edges;

            public readonly int Depth;

            public SearchFrame(TVertex vertex, IEnumerator<TEdge> edges, int depth)
            {
                Contract.Requires(vertex != null);
                Contract.Requires(edges != null);
                Contract.Requires(depth >= 0);
                Vertex = vertex;
                Edges = edges;
                Depth = depth;
            }
        }
    }
}