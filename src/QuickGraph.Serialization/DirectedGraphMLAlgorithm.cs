namespace QuickGraph.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms;
    using QuickGraph.Serialization.DirectedGraphML;

    public sealed class DirectedGraphMlAlgorithm<TVertex, TEdge>
        : AlgorithmBase<IVertexAndEdgeListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly EdgeIdentity<TVertex, TEdge> _edgeIdentities;

        private readonly VertexIdentity<TVertex> _vertexIdentities;

        public DirectedGraph DirectedGraph { get; private set; }

        public DirectedGraphMlAlgorithm(
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            VertexIdentity<TVertex> vertexIdentities,
            EdgeIdentity<TVertex, TEdge> edgeIdentities)
            : base(visitedGraph)
        {
            Contract.Requires(vertexIdentities != null);
            _vertexIdentities = vertexIdentities;
            _edgeIdentities = edgeIdentities;
        }

        protected override void InternalCompute()
        {
            var cancelManager = Services.CancelManager;
            DirectedGraph = new DirectedGraph();

            var nodes = new List<DirectedGraphNode>(VisitedGraph.VertexCount);
            foreach (var vertex in VisitedGraph.Vertices)
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }

                var node = new DirectedGraphNode
                           {
                               Id = _vertexIdentities(vertex)
                           };
                OnFormatNode(vertex, node);
                nodes.Add(node);
            }
            DirectedGraph.Nodes = nodes.ToArray();

            var links = new List<DirectedGraphLink>(VisitedGraph.EdgeCount);
            foreach (var edge in VisitedGraph.Edges)
            {
                if (cancelManager.IsCancelling)
                {
                    return;
                }

                var link = new DirectedGraphLink
                           {
                               Label = _edgeIdentities(edge),
                               Source = _vertexIdentities(edge.Source),
                               Target = _vertexIdentities(edge.Target)
                           };
                OnFormatEdge(edge, link);
                links.Add(link);
            }
            DirectedGraph.Links = links.ToArray();

            OnFormatGraph();
        }

        private void OnFormatEdge(TEdge edge, DirectedGraphLink link)
        {
            Contract.Requires(edge != null);
            Contract.Requires(link != null);

            FormatEdge?.Invoke(edge, link);
        }

        private void OnFormatGraph()
        {
            FormatGraph?.Invoke(VisitedGraph, DirectedGraph);
        }

        private void OnFormatNode(TVertex vertex, DirectedGraphNode node)
        {
            Contract.Requires(node != null);

            FormatNode?.Invoke(vertex, node);
        }

        /// <summary>Raised when a new link is added to the graph</summary>
        public event Action<TEdge, DirectedGraphLink> FormatEdge;

        /// <summary>Raised when the graph is about to be returned</summary>
        public event Action<IVertexAndEdgeListGraph<TVertex, TEdge>, DirectedGraph> FormatGraph;

        /// <summary>Raised when a new node is added to the graph</summary>
        public event Action<TVertex, DirectedGraphNode> FormatNode;
    }
}