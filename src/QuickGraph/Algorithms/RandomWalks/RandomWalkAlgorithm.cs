namespace QuickGraph.Algorithms.RandomWalks
{
    using System.Diagnostics.Contracts;

    public sealed class RandomWalkAlgorithm<TVertex, TEdge>
        : ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private IEdgeChain<TVertex, TEdge> _edgeChain;

        public IImplicitGraph<TVertex, TEdge> VisitedGraph { get; }

        public IEdgeChain<TVertex, TEdge> EdgeChain
        {
            get { return _edgeChain; }
            set
            {
                Contract.Requires(value != null);

                _edgeChain = value;
            }
        }

        public EdgePredicate<TVertex, TEdge> EndPredicate { get; set; }

        public RandomWalkAlgorithm(IImplicitGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new NormalizedMarkovEdgeChain<TVertex, TEdge>())
        {
        }

        public RandomWalkAlgorithm(
            IImplicitGraph<TVertex, TEdge> visitedGraph,
            IEdgeChain<TVertex, TEdge> edgeChain
        )
        {
            Contract.Requires(visitedGraph != null);
            Contract.Requires(edgeChain != null);

            VisitedGraph = visitedGraph;
            _edgeChain = edgeChain;
        }

        public void Generate(TVertex root)
        {
            Contract.Requires(root != null);

            Generate(root, 100);
        }

        public void Generate(TVertex root, int walkCount)
        {
            Contract.Requires(root != null);

            var count = 0;
            var e = default(TEdge);
            var v = root;

            OnStartVertex(root);
            while (count < walkCount && TryGetSuccessor(v, out e))
            {
                // if dead end stop
                if (e == null)
                {
                    break;
                }

                // if end predicate, test
                if (EndPredicate != null && EndPredicate(e))
                {
                    break;
                }
                OnTreeEdge(e);
                v = e.Target;

                // upgrade count
                ++count;
            }
            OnEndVertex(v);
        }

        private void OnEndVertex(TVertex v)
        {
            if (EndVertex != null)
            {
                EndVertex(v);
            }
        }

        private void OnStartVertex(TVertex v)
        {
            if (StartVertex != null)
            {
                StartVertex(v);
            }
        }

        private void OnTreeEdge(TEdge e)
        {
            if (TreeEdge != null)
            {
                TreeEdge(e);
            }
        }

        private bool TryGetSuccessor(TVertex u, out TEdge successor)
        {
            return EdgeChain.TryGetSuccessor(VisitedGraph, u, out successor);
        }

        public event VertexAction<TVertex> EndVertex;

        public event VertexAction<TVertex> StartVertex;

        public event EdgeAction<TVertex, TEdge> TreeEdge;
    }
}