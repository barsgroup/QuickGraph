namespace QuickGraph
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using QuickGraph.Clonable;

    /// <summary>
    ///     An immutable directed graph data structure efficient for large sparse graph representation where out-edge need
    ///     to be enumerated only.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public sealed class ArrayAdjacencyGraph<TVertex, TEdge>
        : IVertexAndEdgeListGraph<TVertex, TEdge>
          ,
          ICloneable
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<TVertex, TEdge[]> _vertexOutEdges;

        public ArrayAdjacencyGraph(
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph
        )
        {
            Contract.Requires(visitedGraph != null);
            _vertexOutEdges = new Dictionary<TVertex, TEdge[]>(visitedGraph.VertexCount);
            EdgeCount = visitedGraph.EdgeCount;
            foreach (var vertex in visitedGraph.Vertices)
            {
                var outEdges = new List<TEdge>(visitedGraph.OutEdges(vertex));
                _vertexOutEdges.Add(vertex, outEdges.ToArray());
            }
        }

        private ArrayAdjacencyGraph(
            Dictionary<TVertex, TEdge[]> vertexOutEdges,
            int edgeCount
        )
        {
            Contract.Requires(vertexOutEdges != null);
            Contract.Requires(edgeCount >= 0);
            Contract.Requires(edgeCount == vertexOutEdges.Sum(kv => kv.Value?.Length ?? 0));

            _vertexOutEdges = vertexOutEdges;
            EdgeCount = edgeCount;
        }

        #region IImplicitVertexSet<TVertex> Members

        public bool ContainsVertex(TVertex vertex)
        {
            return _vertexOutEdges.ContainsKey(vertex);
        }

        #endregion

        #region IIncidenceGraph<TVertex,TEdge> Members

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            TEdge edge;
            return TryGetEdge(source, target, out edge);
        }

        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            TEdge[] es;
            if (_vertexOutEdges.TryGetValue(source, out es))
            {
                List<TEdge> _edges = null;
                for (var i = 0; i < es.Length; i++)
                    if (es[i].Target.Equals(target))
                    {
                        if (_edges == null)
                        {
                            _edges = new List<TEdge>(es.Length - i);
                        }
                        _edges.Add(es[i]);
                    }

                edges = _edges;
                return edges != null;
            }

            edges = null;
            return false;
        }

        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            TEdge[] edges;
            if (_vertexOutEdges.TryGetValue(source, out edges) &&
                edges != null)
            {
                foreach (var item in edges)
                    if (item.Target.Equals(target))
                    {
                        edge = item;
                        return true;
                    }
            }

            edge = default(TEdge);
            return false;
        }

        #endregion

        #region IImplicitGraph<TVertex,TEdge> Members

        public bool IsOutEdgesEmpty(TVertex v)
        {
            return OutDegree(v) == 0;
        }

        public int OutDegree(TVertex v)
        {
            TEdge[] edges;
            if (_vertexOutEdges.TryGetValue(v, out edges) &&
                edges != null)
            {
                return edges.Length;
            }
            return 0;
        }

        public IEnumerable<TEdge> OutEdges(TVertex v)
        {
            TEdge[] edges;
            if (_vertexOutEdges.TryGetValue(v, out edges) &&
                edges != null)
            {
                return edges;
            }

            return Enumerable.Empty<TEdge>();
        }

        public bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            TEdge[] aedges;
            if (_vertexOutEdges.TryGetValue(v, out aedges) &&
                aedges != null)
            {
                edges = aedges;
                return true;
            }

            edges = null;
            return false;
        }

        public TEdge OutEdge(TVertex v, int index)
        {
            return _vertexOutEdges[v][index];
        }

        #endregion

        #region IGraph<TVertex,TEdge> Members

        public bool IsDirected => true;

        public bool AllowParallelEdges => true;

        #endregion

        #region IVertexSet<TVertex> Members

        public bool IsVerticesEmpty => _vertexOutEdges.Count == 0;

        public int VertexCount => _vertexOutEdges.Count;

        public IEnumerable<TVertex> Vertices => _vertexOutEdges.Keys;

        #endregion

        #region IEdgeSet<TVertex,TEdge> Members

        public bool IsEdgesEmpty => EdgeCount == 0;

        public int EdgeCount { get; }

        public IEnumerable<TEdge> Edges => _vertexOutEdges.Values.Where(e => e != null).SelectMany(e => e);

        public bool ContainsEdge(TEdge edge)
        {
            TEdge[] edges;
            if (_vertexOutEdges.TryGetValue(edge.Source, out edges) && edges != null)
            {
                for (var i = 0; i < edges.Length; i++)
                    if (edges[i].Equals(edge))
                    {
                        return true;
                    }
            }

            return false;
        }

        #endregion

        #region ICloneable Members

        /// <summary>Returns self since this class is immutable</summary>
        /// <returns></returns>
        public ArrayAdjacencyGraph<TVertex, TEdge> Clone()
        {
            return this;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }
}