namespace QuickGraph
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using QuickGraph.Clonable;
    using QuickGraph.Collections;

    [DebuggerDisplay("EdgeCount = {EdgeCount}")]
    public class EdgeListGraph<TVertex, TEdge>
        : IEdgeListGraph<TVertex, TEdge>
          ,
          IMutableEdgeListGraph<TVertex, TEdge>
          ,
          ICloneable
        where TEdge : IEdge<TVertex>
    {
        private readonly EdgeEdgeDictionary<TVertex, TEdge> _edges = new EdgeEdgeDictionary<TVertex, TEdge>();

        public bool IsEdgesEmpty => _edges.Count == 0;

        public int EdgeCount => _edges.Count;

        public IEnumerable<TEdge> Edges => _edges.Keys;

        public bool IsDirected { get; } = true;

        public bool AllowParallelEdges { get; } = true;

        public EdgeListGraph()
        {
        }

        public EdgeListGraph(bool isDirected, bool allowParralelEdges)
        {
            IsDirected = isDirected;
            AllowParallelEdges = allowParralelEdges;
        }

        public bool AddEdge(TEdge edge)
        {
            if (ContainsEdge(edge))
            {
                return false;
            }
            _edges.Add(edge, edge);
            OnEdgeAdded(edge);
            return true;
        }

        public int AddEdgeRange(IEnumerable<TEdge> edges)
        {
            var count = 0;
            foreach (var edge in edges)
                if (AddEdge(edge))
                {
                    count++;
                }
            return count;
        }

        public bool AddVerticesAndEdge(TEdge edge)
        {
            return AddEdge(edge);
        }

        public int AddVerticesAndEdgeRange(IEnumerable<TEdge> edges)
        {
            var count = 0;
            foreach (var edge in edges)
                if (AddVerticesAndEdge(edge))
                {
                    count++;
                }
            return count;
        }

        public void Clear()
        {
            var edges = _edges.Clone();
            _edges.Clear();
            foreach (var edge in edges.Keys)
                OnEdgeRemoved(edge);
            OnCleared(EventArgs.Empty);
        }

        [Pure]
        public bool ContainsEdge(TEdge edge)
        {
            return _edges.ContainsKey(edge);
        }

        public bool RemoveEdge(TEdge edge)
        {
            if (_edges.Remove(edge))
            {
                OnEdgeRemoved(edge);
                return true;
            }
            return false;
        }

        public int RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
        {
            var edgesToRemove = new List<TEdge>();
            foreach (var edge in Edges)
                if (predicate(edge))
                {
                    edgesToRemove.Add(edge);
                }

            foreach (var edge in edgesToRemove)
                _edges.Remove(edge);
            return edgesToRemove.Count;
        }

        protected virtual void OnEdgeAdded(TEdge args)
        {
            var eh = EdgeAdded;
            if (eh != null)
            {
                eh(args);
            }
        }

        protected virtual void OnEdgeRemoved(TEdge args)
        {
            var eh = EdgeRemoved;
            if (eh != null)
            {
                eh(args);
            }
        }

        private void OnCleared(EventArgs e)
        {
            var eh = Cleared;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        public event EventHandler Cleared;

        public event EdgeAction<TVertex, TEdge> EdgeAdded;

        public event EdgeAction<TVertex, TEdge> EdgeRemoved;

        #region ICloneable Members

        private EdgeListGraph(
            bool isDirected,
            bool allowParralelEdges,
            EdgeEdgeDictionary<TVertex, TEdge> edges)
        {
            Contract.Requires(edges != null);

            IsDirected = isDirected;
            AllowParallelEdges = allowParralelEdges;
            _edges = edges;
        }

        public EdgeListGraph<TVertex, TEdge> Clone()
        {
            return new EdgeListGraph<TVertex, TEdge>(
                IsDirected,
                AllowParallelEdges,
                _edges.Clone()
            );
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region IVertexSet<TVertex> Members

        [Pure]
        public bool IsVerticesEmpty => _edges.Count == 0;

        [Pure]
        public int VertexCount => GetVertexCounts().Count;

        [Pure]
        public IEnumerable<TVertex> Vertices => GetVertexCounts().Keys;

        private Dictionary<TVertex, int> GetVertexCounts()
        {
            var vertices = new Dictionary<TVertex, int>(EdgeCount * 2);
            foreach (var e in Edges)
            {
                vertices[e.Source]++;
                vertices[e.Target]++;
            }
            return vertices;
        }

        [Pure]
        public bool ContainsVertex(TVertex vertex)
        {
            foreach (var e in Edges)
                if (e.Source.Equals(vertex) ||
                    e.Target.Equals(vertex))
                {
                    return true;
                }

            return false;
        }

        #endregion
    }
}