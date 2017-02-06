namespace QuickGraph.Algorithms.ConnectedComponents
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Services;
    using QuickGraph.Collections;

    public sealed class IncrementalConnectedComponentsAlgorithm<TVertex, TEdge>
        : AlgorithmBase<IMutableVertexAndEdgeSet<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private Dictionary<TVertex, int> _components;

        private ForestDisjointSet<TVertex> _ds;

        public int ComponentCount
        {
            get
            {
                Contract.Assert(_ds != null);
                return _ds.SetCount;
            }
        }

        public IncrementalConnectedComponentsAlgorithm(IMutableVertexAndEdgeSet<TVertex, TEdge> visitedGraph)
            : this(null, visitedGraph)
        {
        }

        public IncrementalConnectedComponentsAlgorithm(IAlgorithmComponent host,
                                                       IMutableVertexAndEdgeSet<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

        /// <summary>
        ///     Gets a copy of the connected components. Key is the number of components, Value contains the vertex ->
        ///     component index map.
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<int, IDictionary<TVertex, int>> GetComponents()
        {
            Contract.Ensures(
                Contract.Result<KeyValuePair<int, IDictionary<TVertex, int>>>().Key == ComponentCount);
            Contract.Ensures(
                Contract.Result<KeyValuePair<int, IDictionary<TVertex, int>>>().Value.Count == VisitedGraph.VertexCount);

            // TODO: more contracts
            Contract.Assert(_ds != null);

            var representatives = new Dictionary<TVertex, int>(_ds.SetCount);
            if (_components == null)
            {
                _components = new Dictionary<TVertex, int>(VisitedGraph.VertexCount);
            }
            foreach (var v in VisitedGraph.Vertices)
            {
                var representative = _ds.FindSet(v);
                int index;
                if (!representatives.TryGetValue(representative, out index))
                {
                    representatives[representative] = index = representatives.Count;
                }
                _components[v] = index;
            }

            return new KeyValuePair<int, IDictionary<TVertex, int>>(_ds.SetCount, _components);
        }

        protected override void InternalCompute()
        {
            _ds = new ForestDisjointSet<TVertex>(VisitedGraph.VertexCount);

            // initialize 1 set per vertex
            foreach (var v in VisitedGraph.Vertices)
                _ds.MakeSet(v);

            // join existing edges
            foreach (var e in VisitedGraph.Edges)
                _ds.Union(e.Source, e.Target);

            // unhook/hook to graph event
            VisitedGraph.EdgeAdded += VisitedGraph_EdgeAdded;
            VisitedGraph.EdgeRemoved += VisitedGraph_EdgeRemoved;
            VisitedGraph.VertexAdded += VisitedGraph_VertexAdded;
            VisitedGraph.VertexRemoved += VisitedGraph_VertexRemoved;
        }

        private void VisitedGraph_EdgeAdded(TEdge e)
        {
            _ds.Union(e.Source, e.Target);
        }

        private void VisitedGraph_EdgeRemoved(TEdge e)
        {
            throw new InvalidOperationException("edge removal not supported for incremental connected components");
        }

        private void VisitedGraph_VertexAdded(TVertex v)
        {
            _ds.MakeSet(v);
        }

        private void VisitedGraph_VertexRemoved(TVertex e)
        {
            throw new InvalidOperationException("vertex removal not supported for incremental connected components");
        }
    }
}