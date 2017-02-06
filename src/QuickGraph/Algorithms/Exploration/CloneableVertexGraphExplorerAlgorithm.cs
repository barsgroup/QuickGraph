namespace QuickGraph.Algorithms.Exploration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Services;
    using QuickGraph.Clonable;

    public sealed class CloneableVertexGraphExplorerAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IMutableVertexAndEdgeSet<TVertex, TEdge>>
          ,
          ITreeBuilderAlgorithm<TVertex, TEdge>
        where TVertex : IComparable<TVertex>, ICloneable
        where TEdge : IEdge<TVertex>
    {
        private readonly Queue<TVertex> _unexploredVertices = new Queue<TVertex>();

        public IList<ITransitionFactory<TVertex, TEdge>> TransitionFactories { get; } =
            new List<ITransitionFactory<TVertex, TEdge>>();

        public VertexPredicate<TVertex> AddVertexPredicate { get; set; } = v => true;

        public VertexPredicate<TVertex> ExploreVertexPredicate { get; set; } = v => true;

        public EdgePredicate<TVertex, TEdge> AddEdgePredicate { get; set; } = e => true;

        public Predicate<CloneableVertexGraphExplorerAlgorithm<TVertex, TEdge>> FinishedPredicate { get; set; } =
            new DefaultFinishedPredicate().Test;

        public IEnumerable<TVertex> UnexploredVertices => _unexploredVertices;

        public bool FinishedSuccessfully { get; private set; }

        public CloneableVertexGraphExplorerAlgorithm(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph
        )
            : this(null, visitedGraph)
        {
        }

        public CloneableVertexGraphExplorerAlgorithm(
            IAlgorithmComponent host,
            IMutableVertexAndEdgeSet<TVertex, TEdge> visitedGraph
        )
            : base(host, visitedGraph)
        {
        }

        protected override void InternalCompute()
        {
            TVertex rootVertex;
            if (!TryGetRootVertex(out rootVertex))
            {
                throw new InvalidOperationException("RootVertex is not specified");
            }

            VisitedGraph.Clear();
            _unexploredVertices.Clear();
            FinishedSuccessfully = false;

            if (!AddVertexPredicate(rootVertex))
            {
                throw new ArgumentException("StartVertex does not satisfy AddVertexPredicate");
            }
            OnDiscoverVertex(rootVertex);

            while (_unexploredVertices.Count > 0)
            {
                // are we done yet ?
                if (!FinishedPredicate(this))
                {
                    FinishedSuccessfully = false;
                    return;
                }

                var current = _unexploredVertices.Dequeue();
                var clone = (TVertex)current.Clone();

                // let's make sure we want to explore this one
                if (!ExploreVertexPredicate(clone))
                {
                    continue;
                }

                foreach (var transitionFactory in TransitionFactories)
                    GenerateFromTransitionFactory(clone, transitionFactory);
            }

            FinishedSuccessfully = true;
        }

        private void GenerateFromTransitionFactory(
            TVertex current,
            ITransitionFactory<TVertex, TEdge> transitionFactory
        )
        {
            if (!transitionFactory.IsValid(current))
            {
                return;
            }

            foreach (var transition in transitionFactory.Apply(current))
            {
                if (
                    !AddVertexPredicate(transition.Target)
                    || !AddEdgePredicate(transition))
                {
                    OnEdgeSkipped(transition);
                    continue;
                }

                var backEdge = VisitedGraph.ContainsVertex(transition.Target);
                if (!backEdge)
                {
                    OnDiscoverVertex(transition.Target);
                }

                VisitedGraph.AddEdge(transition);
                if (backEdge)
                {
                    OnBackEdge(transition);
                }
                else
                {
                    OnTreeEdge(transition);
                }
            }
        }

        private void OnBackEdge(TEdge e)
        {
            Contract.Requires(e != null);
            var eh = BackEdge;
            if (eh != null)
            {
                eh(e);
            }
        }

        private void OnDiscoverVertex(TVertex v)
        {
            Contract.Requires(v != null);

            VisitedGraph.AddVertex(v);
            _unexploredVertices.Enqueue(v);

            var eh = DiscoverVertex;
            if (eh != null)
            {
                eh(v);
            }
        }

        private void OnEdgeSkipped(TEdge e)
        {
            Contract.Requires(e != null);
            var eh = EdgeSkipped;
            if (eh != null)
            {
                eh(e);
            }
        }

        private void OnTreeEdge(TEdge e)
        {
            Contract.Requires(e != null);

            var eh = TreeEdge;
            if (eh != null)
            {
                eh(e);
            }
        }

        public event EdgeAction<TVertex, TEdge> BackEdge;

        public event VertexAction<TVertex> DiscoverVertex;

        public event EdgeAction<TVertex, TEdge> EdgeSkipped;

        public event EdgeAction<TVertex, TEdge> TreeEdge;

        public sealed class DefaultFinishedPredicate
        {
            public int MaxVertexCount { get; set; } = 1000;

            public int MaxEdgeCount { get; set; } = 1000;

            public DefaultFinishedPredicate()
            {
            }

            public DefaultFinishedPredicate(
                int maxVertexCount,
                int maxEdgeCount)
            {
                MaxVertexCount = maxVertexCount;
                MaxEdgeCount = maxEdgeCount;
            }

            public bool Test(CloneableVertexGraphExplorerAlgorithm<TVertex, TEdge> t)
            {
                if (t.VisitedGraph.VertexCount > MaxVertexCount)
                {
                    return false;
                }
                if (t.VisitedGraph.EdgeCount > MaxEdgeCount)
                {
                    return false;
                }
                return true;
            }
        }
    }
}