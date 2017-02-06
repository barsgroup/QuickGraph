namespace QuickGraph.Algorithms.ConnectedComponents
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using QuickGraph.Algorithms.Search;
    using QuickGraph.Algorithms.Services;

    public sealed class StronglyConnectedComponentsAlgorithm<TVertex, TEdge> :
        AlgorithmBase<IVertexListGraph<TVertex, TEdge>>,
        IConnectedComponentAlgorithm<TVertex, TEdge, IVertexListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<TVertex, int> _discoverTimes;

        private readonly Dictionary<TVertex, TVertex> _roots;

        private readonly Stack<TVertex> _stack;

        private int _dfsTime;

        public IDictionary<TVertex, TVertex> Roots => _roots;

        public IDictionary<TVertex, int> DiscoverTimes => _discoverTimes;

        public IDictionary<TVertex, int> Components { get; }

        public int ComponentCount { get; private set; }

        public StronglyConnectedComponentsAlgorithm(
            IVertexListGraph<TVertex, TEdge> g)
            : this(g, new Dictionary<TVertex, int>())
        {
        }

        public StronglyConnectedComponentsAlgorithm(
            IVertexListGraph<TVertex, TEdge> g,
            IDictionary<TVertex, int> components)
            : this(null, g, components)
        {
        }

        public StronglyConnectedComponentsAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> g,
            IDictionary<TVertex, int> components)
            : base(host, g)
        {
            Contract.Requires(components != null);

            Components = components;
            _roots = new Dictionary<TVertex, TVertex>();
            _discoverTimes = new Dictionary<TVertex, int>();
            _stack = new Stack<TVertex>();
            ComponentCount = 0;
            _dfsTime = 0;
        }

        protected override void InternalCompute()
        {
            Contract.Ensures(ComponentCount >= 0);
            Contract.Ensures(VisitedGraph.VertexCount == 0 || ComponentCount > 0);
            Contract.Ensures(VisitedGraph.Vertices.All(v => Components.ContainsKey(v)));
            Contract.Ensures(VisitedGraph.VertexCount == Components.Count);
            Contract.Ensures(Components.Values.All(c => c <= ComponentCount));

            Components.Clear();
            Roots.Clear();
            DiscoverTimes.Clear();
            _stack.Clear();
            ComponentCount = 0;
            _dfsTime = 0;

            DepthFirstSearchAlgorithm<TVertex, TEdge> dfs = null;
            try
            {
                dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    new Dictionary<TVertex, GraphColor>(VisitedGraph.VertexCount)
                );
                dfs.DiscoverVertex += DiscoverVertex;
                dfs.FinishVertex += FinishVertex;

                dfs.Compute();
            }
            finally
            {
                if (dfs != null)
                {
                    dfs.DiscoverVertex -= DiscoverVertex;
                    dfs.FinishVertex -= FinishVertex;
                }
            }
        }

        private void DiscoverVertex(TVertex v)
        {
            Roots[v] = v;
            Components[v] = int.MaxValue;
            DiscoverTimes[v] = _dfsTime++;
            _stack.Push(v);
        }

        /// <summary>Used internally</summary>
        private void FinishVertex(TVertex v)
        {
            var roots = Roots;

            foreach (var e in VisitedGraph.OutEdges(v))
            {
                var w = e.Target;
                if (Components[w] == int.MaxValue)
                {
                    roots[v] = MinDiscoverTime(roots[v], roots[w]);
                }
            }

            if (_roots[v].Equals(v))
            {
                var w = default(TVertex);
                do
                {
                    w = _stack.Pop();
                    Components[w] = ComponentCount;
                }
                while (!w.Equals(v));
                ++ComponentCount;
            }
        }

        private TVertex MinDiscoverTime(TVertex u, TVertex v)
        {
            Contract.Requires(u != null);
            Contract.Requires(v != null);
            Contract.Ensures(
                DiscoverTimes[u] < DiscoverTimes[v]
                    ? Contract.Result<TVertex>().Equals(u)
                    : Contract.Result<TVertex>().Equals(v)
            );

            if (_discoverTimes[u] < _discoverTimes[v])
            {
                return u;
            }
            return v;
        }
    }
}