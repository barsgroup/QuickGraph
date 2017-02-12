namespace QuickGraph
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using QuickGraph.Collections;

    /// <summary>
    ///     A mutable directed graph data structure efficient for sparse graph representation where out-edge need to be
    ///     enumerated only.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public class AdjacencyGraph<TVertex, TEdge>
        : IEdgeListAndIncidenceGraph<TVertex, TEdge>,
          IMutableVertexAndEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        public static Type EdgeType => typeof(TEdge);

        private readonly IVertexEdgeDictionary<TVertex, TEdge> _vertexEdges;

        public int EdgeCapacity { get; set; } = -1;

        public bool IsDirected { get; } = true;

        public bool AllowParallelEdges
        {
            [Pure]
            get;
        }

        public bool IsVerticesEmpty => _vertexEdges.Count == 0;

        public int VertexCount => _vertexEdges.Count;

        public virtual IEnumerable<TVertex> Vertices => _vertexEdges.Keys;

        /// <summary>Gets a value indicating whether this instance is edges empty.</summary>
        /// <value>
        ///     <c>true</c> if this instance is edges empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEdgesEmpty => EdgeCount == 0;

        /// <summary>Gets the edge count.</summary>
        /// <value>The edge count.</value>
        public int EdgeCount { get; private set; }

        /// <summary>Gets the edges.</summary>
        /// <value>The edges.</value>
        public virtual IEnumerable<TEdge> Edges => _vertexEdges.Values.SelectMany(edges => edges);

        public AdjacencyGraph()
            : this(true)
        {
        }

        public AdjacencyGraph(bool allowParallelEdges, int vertexCapacity = -1)
            : this(allowParallelEdges, vertexCapacity, -1)
        {
        }

        public AdjacencyGraph(bool allowParallelEdges, int vertexCapacity, int edgeCapacity)
            : this(allowParallelEdges, vertexCapacity, edgeCapacity, EqualityComparer<TVertex>.Default)
        {
        }

        public AdjacencyGraph(bool allowParallelEdges,
                              int vertexCapacity,
                              int edgeCapacity,
                              IEqualityComparer<TVertex> vertexComparer)
        {
            Contract.Requires(vertexComparer != null);

            AllowParallelEdges = allowParallelEdges;
            _vertexEdges = vertexCapacity > -1
                               ? new VertexEdgeDictionary<TVertex, TEdge>(vertexCapacity, vertexComparer)
                               : new VertexEdgeDictionary<TVertex, TEdge>(vertexComparer);

            EdgeCapacity = edgeCapacity;
        }

        public AdjacencyGraph(
            bool allowParallelEdges,
            int capacity,
            int edgeCapacity,
            Func<int, IVertexEdgeDictionary<TVertex, TEdge>> vertexEdgesDictionaryFactory)
        {
            Contract.Requires(vertexEdgesDictionaryFactory != null);
            AllowParallelEdges = allowParallelEdges;
            _vertexEdges = vertexEdgesDictionaryFactory(capacity);
            EdgeCapacity = edgeCapacity;
        }

        /// <summary>Adds the edge to the graph</summary>
        /// <param name="e">the edge to add</param>
        /// <returns>true if the edge was added; false if it was already part of the graph</returns>
        public virtual bool AddEdge(TEdge e)
        {
            if (!AllowParallelEdges && ContainsEdge(e.Source, e.Target))
            {
                return false;
            }
            _vertexEdges[e.Source].Add(e);
            EdgeCount++;

            OnEdgeAdded(e);

            return true;
        }

        public int AddEdgeRange(IEnumerable<TEdge> edges) => edges.Count(AddEdge);

        public virtual bool AddVertex(TVertex v)
        {
            if (ContainsVertex(v))
            {
                return false;
            }

            var edgeList = EdgeCapacity > 0
                               ? new EdgeList<TVertex, TEdge>(EdgeCapacity)
                               : new EdgeList<TVertex, TEdge>();

            _vertexEdges.Add(v, edgeList);
            OnVertexAdded(v);
            return true;
        }

        public virtual int AddVertexRange(IEnumerable<TVertex> vertices) => vertices.Count(AddVertex);

        public virtual bool AddVerticesAndEdge(TEdge e)
        {
            AddVertex(e.Source);
            AddVertex(e.Target);
            return AddEdge(e);
        }

        /// <summary>Adds a range of edges to the graph</summary>
        /// <param name="edges"></param>
        /// <returns>the count edges that were added</returns>
        public int AddVerticesAndEdgeRange(IEnumerable<TEdge> edges) => edges.Count(AddVerticesAndEdge);

        public void Clear()
        {
            _vertexEdges.Clear();
            EdgeCount = 0;
            OnCleared(EventArgs.Empty);
        }

        public void ClearOutEdges(TVertex v)
        {
            var edges = _vertexEdges[v];
            var count = edges.Count;
            if (EdgeRemoved != null) // call only if someone is listening
            {
                foreach (var edge in edges)
                    OnEdgeRemoved(edge);
            }
            edges.Clear();
            EdgeCount -= count;
        }

        [Pure]
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            IEnumerable<TEdge> outEdges;
            return TryGetOutEdges(source, out outEdges) && outEdges.Any(outEdge => outEdge.Target.Equals(target));
        }

        public bool ContainsEdge(TEdge edge)
        {
            IEdgeList<TVertex, TEdge> edges;
            return _vertexEdges.TryGetValue(edge.Source, out edges) && edges.Contains(edge);
        }

        public bool ContainsVertex(TVertex v) => _vertexEdges.ContainsKey(v);

        public bool IsOutEdgesEmpty(TVertex v) => _vertexEdges[v].Count == 0;

        public int OutDegree(TVertex v) => _vertexEdges[v].Count;

        public TEdge OutEdge(TVertex v, int index) => _vertexEdges[v][index];

        public virtual IEnumerable<TEdge> OutEdges(TVertex v) => _vertexEdges[v];

        public virtual bool RemoveEdge(TEdge e)
        {
            IEdgeList<TVertex, TEdge> edges;
            if (_vertexEdges.TryGetValue(e.Source, out edges) && edges.Remove(e))
            {
                EdgeCount--;
                Contract.Assert(EdgeCount >= 0);
                OnEdgeRemoved(e);
                return true;
            }
            return false;
        }

        public int RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
        {
            var edges = new EdgeList<TVertex, TEdge>();
            edges.AddRange(Edges.Where(edge => predicate(edge)));

            foreach (var edge in edges)
                RemoveEdge(edge);

            return edges.Count;
        }

        public int RemoveOutEdgeIf(TVertex v, EdgePredicate<TVertex, TEdge> predicate)
        {
            var edges = _vertexEdges[v];
            var edgeToRemove = new EdgeList<TVertex, TEdge>(edges.Count);
            edgeToRemove.AddRange(edges.Where(edge => predicate(edge)));

            foreach (var edge in edgeToRemove)
            {
                edges.Remove(edge);
                OnEdgeRemoved(edge);
            }
            EdgeCount -= edgeToRemove.Count;

            return edgeToRemove.Count;
        }

        public virtual bool RemoveVertex(TVertex v)
        {
            if (!ContainsVertex(v))
            {
                return false;
            }

            // remove outedges
            {
                var edges = _vertexEdges[v];
                if (EdgeRemoved != null) // lazily notify
                {
                    foreach (var edge in edges)
                        OnEdgeRemoved(edge);
                }
                EdgeCount -= edges.Count;
                edges.Clear();
            }

            // iterage over edges and remove each edge touching the vertex
            var edgeToRemove = new EdgeList<TVertex, TEdge>();
            foreach (var kv in _vertexEdges)
            {
                if (kv.Key.Equals(v))
                {
                    continue; // we've already 
                }

                // collect edge to remove
                foreach (var edge in kv.Value)
                    if (edge.Target.Equals(v))
                    {
                        edgeToRemove.Add(edge);
                    }

                // remove edges
                foreach (var edge in edgeToRemove)
                {
                    kv.Value.Remove(edge);
                    OnEdgeRemoved(edge);
                }

                // update count
                EdgeCount -= edgeToRemove.Count;
                edgeToRemove.Clear();
            }

            Contract.Assert(EdgeCount >= 0);
            _vertexEdges.Remove(v);
            OnVertexRemoved(v);

            return true;
        }

        public int RemoveVertexIf(VertexPredicate<TVertex> predicate)
        {
            var vertices = new VertexList<TVertex>();
            vertices.AddRange(Vertices.Where(v => predicate(v)));

            foreach (var v in vertices)
                RemoveVertex(v);

            return vertices.Count;
        }

        public void TrimEdgeExcess()
        {
            foreach (var edges in _vertexEdges.Values)
                edges.TrimExcess();
        }

        [Pure]
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            IEdgeList<TVertex, TEdge> edgeList;
            if (_vertexEdges.TryGetValue(source, out edgeList) &&
                edgeList.Count > 0)
            {
                foreach (var e in edgeList)
                    if (e.Target.Equals(target))
                    {
                        edge = e;
                        return true;
                    }
            }
            edge = default(TEdge);
            return false;
        }

        [Pure]
        public virtual bool TryGetEdges(
            TVertex source,
            TVertex target,
            out IEnumerable<TEdge> edges)
        {
            IEdgeList<TVertex, TEdge> outEdges;
            if (_vertexEdges.TryGetValue(source, out outEdges))
            {
                var list = new List<TEdge>(outEdges.Count);
                foreach (var edge in outEdges)
                    if (edge.Target.Equals(target))
                    {
                        list.Add(edge);
                    }

                edges = list;
                return true;
            }
            edges = null;
            return false;
        }

        public virtual bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            IEdgeList<TVertex, TEdge> list;
            if (_vertexEdges.TryGetValue(v, out list))
            {
                edges = list;
                return true;
            }

            edges = null;
            return false;
        }

        protected virtual void OnEdgeAdded(TEdge args) => EdgeAdded?.Invoke(args);

        protected virtual void OnEdgeRemoved(TEdge args) => EdgeRemoved?.Invoke(args);

        protected virtual void OnVertexAdded(TVertex args)
        {
            Contract.Requires(args != null);

            VertexAdded?.Invoke(args);
        }

        protected virtual void OnVertexRemoved(TVertex args)
        {
            Contract.Requires(args != null);

            VertexRemoved?.Invoke(args);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(EdgeCount >= 0);
        }

        private void OnCleared(EventArgs e) => Cleared?.Invoke(this, e);

        public event EventHandler Cleared;

        public event EdgeAction<TVertex, TEdge> EdgeAdded;

        public event EdgeAction<TVertex, TEdge> EdgeRemoved;

        public event VertexAction<TVertex> VertexAdded;

        public event VertexAction<TVertex> VertexRemoved;

        #region ICloneable Members

        private AdjacencyGraph(
            IVertexEdgeDictionary<TVertex, TEdge> vertexEdges,
            int edgeCount,
            int edgeCapacity,
            bool allowParallelEdges
        )
        {
            Contract.Requires(vertexEdges != null);
            Contract.Requires(edgeCount >= 0);

            _vertexEdges = vertexEdges;
            EdgeCount = edgeCount;
            EdgeCapacity = edgeCapacity;
            AllowParallelEdges = allowParallelEdges;
        }

        [Pure]
        public AdjacencyGraph<TVertex, TEdge> Clone()
        {
            return new AdjacencyGraph<TVertex, TEdge>(
                _vertexEdges.Clone(),
                EdgeCount,
                EdgeCapacity,
                AllowParallelEdges
            );
        }

        #endregion
    }
}