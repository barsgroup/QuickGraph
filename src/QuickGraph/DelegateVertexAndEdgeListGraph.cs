﻿namespace QuickGraph
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>A delegate-based incidence graph</summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    public class DelegateVertexAndEdgeListGraph<TVertex, TEdge>
        : DelegateIncidenceGraph<TVertex, TEdge>
          ,
          IVertexAndEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>, IEquatable<TEdge>
    {
        private readonly IEnumerable<TVertex> _vertices;

        private int _edgeCount = -1;

        private int _vertexCount = -1;

        public bool IsVerticesEmpty
        {
            get
            {
                // shortcut if count is already computed
                if (_vertexCount > -1)
                {
                    return _vertexCount == 0;
                }

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
                // shortcut if edges is already computed
                if (_edgeCount > -1)
                {
                    return _edgeCount == 0;
                }

                foreach (var vertex in _vertices)
                foreach (var edge in OutEdges(vertex))
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
                foreach (var edge in OutEdges(vertex))
                    yield return edge;
            }
        }

        public DelegateVertexAndEdgeListGraph(
            IEnumerable<TVertex> vertices,
            TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges)
            : base(tryGetOutEdges)
        {
            Contract.Requires(vertices != null);
            Contract.Requires(
                vertices.All(
                    v =>
                    {
                        IEnumerable<TEdge> edges;
                        return tryGetOutEdges(v, out edges);
                    }));
            _vertices = vertices;
        }

        public bool ContainsEdge(TEdge edge)
        {
            IEnumerable<TEdge> edges;
            if (TryGetOutEdges(edge.Source, out edges))
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