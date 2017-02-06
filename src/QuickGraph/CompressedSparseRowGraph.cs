namespace QuickGraph
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using QuickGraph.Clonable;

    /// <summary>
    ///     Directed graph representation using a Compressed Sparse Row representation
    ///     (http://www.cs.utk.edu/~dongarra/etemplates/node373.html)
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public sealed class CompressedSparseRowGraph<TVertex>
        : IVertexSet<TVertex>
          ,
          IEdgeSet<TVertex, SEquatableEdge<TVertex>>
          ,
          IVertexListGraph<TVertex, SEquatableEdge<TVertex>>
          ,
          ICloneable
    {
        private readonly TVertex[] _outEdges;

        private readonly Dictionary<TVertex, Range> _outEdgeStartRanges;

        public int EdgeCount => _outEdges.Length;

        public bool IsEdgesEmpty => _outEdges.Length > 0;

        public IEnumerable<SEquatableEdge<TVertex>> Edges
        {
            get
            {
                foreach (var kv in _outEdgeStartRanges)
                {
                    var source = kv.Key;
                    var range = kv.Value;
                    for (var i = range.Start; i < range.End; ++i)
                    {
                        var target = _outEdges[i];
                        yield return new SEquatableEdge<TVertex>(source, target);
                    }
                }
            }
        }

        public bool IsDirected => true;

        public bool AllowParallelEdges => false;

        public bool IsVerticesEmpty => _outEdgeStartRanges.Count > 0;

        public int VertexCount => _outEdgeStartRanges.Count;

        public IEnumerable<TVertex> Vertices => _outEdgeStartRanges.Keys;

        private CompressedSparseRowGraph(
            Dictionary<TVertex, Range> outEdgeStartRanges,
            TVertex[] outEdges
        )
        {
            Contract.Requires(outEdgeStartRanges != null);
            Contract.Requires(outEdges != null);

            _outEdgeStartRanges = outEdgeStartRanges;
            _outEdges = outEdges;
        }

        public CompressedSparseRowGraph<TVertex> Clone()
        {
            var ranges = new Dictionary<TVertex, Range>(_outEdgeStartRanges);
            var edges = (TVertex[])_outEdges.Clone();
            return new CompressedSparseRowGraph<TVertex>(ranges, edges);
        }

        public bool ContainsEdge(SEquatableEdge<TVertex> edge)
        {
            return ContainsEdge(edge.Source, edge.Target);
        }

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            Range range;
            if (_outEdgeStartRanges.TryGetValue(source, out range))
            {
                for (var i = range.Start; i < range.End; ++i)
                    if (_outEdges[i].Equals(target))
                    {
                        return true;
                    }
            }

            return false;
        }

        public bool ContainsVertex(TVertex vertex)
        {
            return _outEdgeStartRanges.ContainsKey(vertex);
        }

        public static CompressedSparseRowGraph<TVertex> FromGraph<TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph
        )
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(visitedGraph != null);
            Contract.Ensures(Contract.Result<CompressedSparseRowGraph<TVertex>>() != null);

            var outEdgeStartRanges = new Dictionary<TVertex, Range>(visitedGraph.VertexCount);
            var outEdges = new TVertex[visitedGraph.EdgeCount];

            var start = 0;
            var end = 0;
            var index = 0;
            foreach (var vertex in visitedGraph.Vertices)
            {
                end = index + visitedGraph.OutDegree(vertex);
                var range = new Range(start, end);
                outEdgeStartRanges.Add(vertex, range);
                foreach (var edge in visitedGraph.OutEdges(vertex))
                    outEdges[index++] = edge.Target;
                Contract.Assert(index == end);
            }
            Contract.Assert(index == outEdges.Length);

            return new CompressedSparseRowGraph<TVertex>(
                outEdgeStartRanges,
                outEdges);
        }

        public bool IsOutEdgesEmpty(TVertex v)
        {
            return _outEdgeStartRanges[v].Length == 0;
        }

        public int OutDegree(TVertex v)
        {
            return _outEdgeStartRanges[v].Length;
        }

        public SEquatableEdge<TVertex> OutEdge(TVertex v, int index)
        {
            var range = _outEdgeStartRanges[v];
            var targetIndex = range.Start + index;
            Contract.Assert(targetIndex < range.End);
            return new SEquatableEdge<TVertex>(v, _outEdges[targetIndex]);
        }

        public IEnumerable<SEquatableEdge<TVertex>> OutEdges(TVertex v)
        {
            var range = _outEdgeStartRanges[v];
            for (var i = range.Start; i < range.End; ++i)
                yield return new SEquatableEdge<TVertex>(v, _outEdges[i]);
        }

        public bool TryGetEdge(
            TVertex source,
            TVertex target,
            out SEquatableEdge<TVertex> edge)
        {
            if (ContainsEdge(source, target))
            {
                edge = new SEquatableEdge<TVertex>(source, target);
                return true;
            }

            edge = default(SEquatableEdge<TVertex>);
            return false;
        }

        public bool TryGetEdges(
            TVertex source,
            TVertex target,
            out IEnumerable<SEquatableEdge<TVertex>> edges)
        {
            if (ContainsEdge(source, target))
            {
                edges = new[] { new SEquatableEdge<TVertex>(source, target) };
                return true;
            }

            edges = null;
            return false;
        }

        public bool TryGetOutEdges(TVertex v, out IEnumerable<SEquatableEdge<TVertex>> edges)
        {
            Range range;
            if (_outEdgeStartRanges.TryGetValue(v, out range) &&
                range.Length > 0)
            {
                edges = OutEdges(v);
                return true;
            }

            edges = null;
            return false;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        private struct Range
        {
            public readonly int Start;

            public readonly int End;

            public Range(int start, int end)
            {
                Contract.Requires(start >= 0);
                Contract.Requires(start <= end);
                Contract.Ensures(Contract.ValueAtReturn(out this).Start == start);
                Contract.Ensures(Contract.ValueAtReturn(out this).End == end);

                Start = start;
                End = end;
            }

            public int Length
            {
                get
                {
                    Contract.Ensures(Contract.Result<int>() >= 0);

                    return End - Start;
                }
            }
        }
    }
}