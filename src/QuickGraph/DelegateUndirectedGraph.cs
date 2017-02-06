namespace QuickGraph
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>A functional implicit undirected graph</summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    public class DelegateUndirectedGraph<TVertex, TEdge>
        : DelegateImplicitUndirectedGraph<TVertex, TEdge>
          ,
          IUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly IEnumerable<TVertex> _vertices;

        private int _edgeCount = -1;

        private int _vertexCount = -1;

        public bool IsVerticesEmpty
        {
            get
            {
                // shortcut
                if (_vertexCount > -1)
                {
                    return _vertexCount == 0;
                }

                // count
                foreach (var vertex in _vertices)
                    return false;
                return true;
            }
        }

        public int VertexCount
        {
            get
            {
                if (_vertexCount < 0)
                {
                    _vertexCount = _vertices.Count();
                }
                return _vertexCount;
            }
        }

        public virtual IEnumerable<TVertex> Vertices => _vertices;

        public bool IsEdgesEmpty
        {
            get
            {
                if (_vertexCount == 0 ||
                    _edgeCount == 0)
                {
                    return true; // no vertices or no edges.
                }

                foreach (var vertex in _vertices)
                foreach (var edge in AdjacentEdges(vertex))
                    return false;
                return true;
            }
        }

        public int EdgeCount
        {
            get
            {
                if (_edgeCount < 0)
                {
                    _edgeCount = Edges.Count();
                }
                return _edgeCount;
            }
        }

        public virtual IEnumerable<TEdge> Edges
        {
            get
            {
                foreach (var vertex in _vertices)
                foreach (var edge in AdjacentEdges(vertex))
                    if (edge.Source.Equals(vertex))
                    {
                        yield return edge;
                    }
            }
        }

        public DelegateUndirectedGraph(
            IEnumerable<TVertex> vertices,
            TryFunc<TVertex, IEnumerable<TEdge>> tryGetAdjacentEdges,
            bool allowParallelEdges)
            : base(tryGetAdjacentEdges, allowParallelEdges)
        {
            Contract.Requires(vertices != null);
            Contract.Requires(
                vertices.All(
                    v =>
                    {
                        IEnumerable<TEdge> edges;
                        return tryGetAdjacentEdges(v, out edges);
                    }));
            _vertices = vertices;
        }

        public bool ContainsEdge(TEdge edge)
        {
            IEnumerable<TEdge> edges;
            if (TryGetAdjacentEdges(edge.Source, out edges))
            {
                foreach (var e in edges)
                    if (e.Equals(edge))
                    {
                        return true;
                    }
            }
            return false;
        }
    }
}