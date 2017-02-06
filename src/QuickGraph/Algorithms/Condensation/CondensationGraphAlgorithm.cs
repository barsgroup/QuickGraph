namespace QuickGraph.Algorithms.Condensation
{
    using System;
    using System.Collections.Generic;

    using QuickGraph.Algorithms.ConnectedComponents;

    public sealed class CondensationGraphAlgorithm<TVertex, TEdge, TGraph> :
        AlgorithmBase<IVertexAndEdgeListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>, new()
    {
        public IMutableBidirectionalGraph<
            TGraph,
            CondensedEdge<TVertex, TEdge, TGraph>
        > CondensedGraph { get; private set; }

        public bool StronglyConnected { get; set; } = true;

        public CondensationGraphAlgorithm(IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
        }

        protected override void InternalCompute()
        {
            // create condensated graph
            CondensedGraph = new BidirectionalGraph<
                TGraph,
                CondensedEdge<TVertex, TEdge, TGraph>
            >(false);
            if (VisitedGraph.VertexCount == 0)
            {
                return;
            }

            // compute strongly connected components
            var components = new Dictionary<TVertex, int>(VisitedGraph.VertexCount);
            var componentCount = ComputeComponentCount(components);

            var cancelManager = Services.CancelManager;
            if (cancelManager.IsCancelling)
            {
                return;
            }

            // create list vertices
            var condensatedVertices = new Dictionary<int, TGraph>(componentCount);
            for (var i = 0; i < componentCount; ++i)
            {
                var v = new TGraph();
                condensatedVertices.Add(i, v);
                CondensedGraph.AddVertex(v);
            }

            // addingvertices
            foreach (var v in VisitedGraph.Vertices)
                condensatedVertices[components[v]].AddVertex(v);
            if (cancelManager.IsCancelling)
            {
                return;
            }

            // condensated edges
            var condensatedEdges = new Dictionary<EdgeKey, CondensedEdge<TVertex, TEdge, TGraph>>(componentCount);

            // iterate over edges and condensate graph
            foreach (var edge in VisitedGraph.Edges)
            {
                // get component ids
                var sourceId = components[edge.Source];
                var targetId = components[edge.Target];

                // get vertices
                var sources = condensatedVertices[sourceId];
                if (sourceId == targetId)
                {
                    sources.AddEdge(edge);
                    continue;
                }

                var targets = condensatedVertices[targetId];

                // at last add edge
                var edgeKey = new EdgeKey(sourceId, targetId);
                CondensedEdge<TVertex, TEdge, TGraph> condensatedEdge;
                if (!condensatedEdges.TryGetValue(edgeKey, out condensatedEdge))
                {
                    condensatedEdge = new CondensedEdge<TVertex, TEdge, TGraph>(sources, targets);
                    condensatedEdges.Add(edgeKey, condensatedEdge);
                    CondensedGraph.AddEdge(condensatedEdge);
                }
                condensatedEdge.Edges.Add(edge);
            }
        }

        private int ComputeComponentCount(Dictionary<TVertex, int> components)
        {
            IConnectedComponentAlgorithm<TVertex, TEdge, IVertexListGraph<TVertex, TEdge>> componentAlgorithm;
            if (StronglyConnected)
            {
                componentAlgorithm = new StronglyConnectedComponentsAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    components);
            }
            else
            {
                componentAlgorithm = new WeaklyConnectedComponentsAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    components);
            }
            componentAlgorithm.Compute();
            return componentAlgorithm.ComponentCount;
        }

        private struct EdgeKey
            : IEquatable<EdgeKey>
        {
            private readonly int _sourceId;

            private readonly int _targetId;

            public EdgeKey(int sourceId, int targetId)
            {
                _sourceId = sourceId;
                _targetId = targetId;
            }

            public bool Equals(EdgeKey other)
            {
                return
                    _sourceId == other._sourceId
                    && _targetId == other._targetId;
            }

            public override int GetHashCode()
            {
                return HashCodeHelper.Combine(_sourceId, _targetId);
            }
        }
    }
}