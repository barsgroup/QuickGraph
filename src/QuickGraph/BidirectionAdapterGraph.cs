namespace QuickGraph
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using QuickGraph.Collections;

    /// <summary>Wraps a vertex list graph (out-edges only) and caches the in-edge dictionary.</summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public class BidirectionAdapterGraph<TVertex, TEdge>
        : IBidirectionalGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private static readonly TEdge[] EmptyEdges = new TEdge[0];

        private readonly IVertexAndEdgeListGraph<TVertex, TEdge> _baseGraph;

        private readonly Dictionary<TVertex, EdgeList<TVertex, TEdge>> _inEdges;

        public bool IsDirected => _baseGraph.IsDirected;

        public bool AllowParallelEdges => _baseGraph.AllowParallelEdges;

        public bool IsVerticesEmpty => _baseGraph.IsVerticesEmpty;

        public int VertexCount => _baseGraph.VertexCount;

        public IEnumerable<TVertex> Vertices => _baseGraph.Vertices;

        public bool IsEdgesEmpty => _baseGraph.IsEdgesEmpty;

        public int EdgeCount => _baseGraph.EdgeCount;

        public virtual IEnumerable<TEdge> Edges => _baseGraph.Edges;

        public BidirectionAdapterGraph(IVertexAndEdgeListGraph<TVertex, TEdge> baseGraph)
        {
            Contract.Requires(baseGraph != null);

            _baseGraph = baseGraph;
            _inEdges = new Dictionary<TVertex, EdgeList<TVertex, TEdge>>(_baseGraph.VertexCount);
            foreach (var edge in _baseGraph.Edges)
            {
                EdgeList<TVertex, TEdge> list;
                if (!_inEdges.TryGetValue(edge.Target, out list))
                {
                    _inEdges.Add(edge.Target, list = new EdgeList<TVertex, TEdge>());
                }
                list.Add(edge);
            }
        }

        [Pure]
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return _baseGraph.ContainsEdge(source, target);
        }

        [Pure]
        public bool ContainsEdge(TEdge edge)
        {
            return _baseGraph.ContainsEdge(edge);
        }

        [Pure]
        public bool ContainsVertex(TVertex vertex)
        {
            return _baseGraph.ContainsVertex(vertex);
        }

        [Pure]
        public int Degree(TVertex v)
        {
            return InDegree(v) + OutDegree(v);
        }

        [Pure]
        public int InDegree(TVertex v)
        {
            EdgeList<TVertex, TEdge> edges;
            if (_inEdges.TryGetValue(v, out edges))
            {
                return edges.Count;
            }
            return 0;
        }

        [Pure]
        public TEdge InEdge(TVertex v, int index)
        {
            return _inEdges[v][index];
        }

        [Pure]
        public IEnumerable<TEdge> InEdges(TVertex v)
        {
            EdgeList<TVertex, TEdge> edges;
            if (_inEdges.TryGetValue(v, out edges))
            {
                return edges;
            }
            return EmptyEdges;
        }

        [Pure]
        public bool IsInEdgesEmpty(TVertex v)
        {
            return InDegree(v) == 0;
        }

        [Pure] // InterfacePureBug
        public bool IsOutEdgesEmpty(TVertex v)
        {
            return _baseGraph.IsOutEdgesEmpty(v);
        }

        [Pure]
        public int OutDegree(TVertex v)
        {
            return _baseGraph.OutDegree(v);
        }

        [Pure]
        public TEdge OutEdge(TVertex v, int index)
        {
            return _baseGraph.OutEdge(v, index);
        }

        [Pure]
        public IEnumerable<TEdge> OutEdges(TVertex v)
        {
            return _baseGraph.OutEdges(v);
        }

        [Pure]
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            return _baseGraph.TryGetEdge(source, target, out edge);
        }

        [Pure]
        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            return _baseGraph.TryGetEdges(source, target, out edges);
        }

        [Pure]
        public bool TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            EdgeList<TVertex, TEdge> es;
            if (_inEdges.TryGetValue(v, out es))
            {
                edges = es;
                return true;
            }

            edges = null;
            return false;
        }

        [Pure]
        public bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            return _baseGraph.TryGetOutEdges(v, out edges);
        }
    }
}