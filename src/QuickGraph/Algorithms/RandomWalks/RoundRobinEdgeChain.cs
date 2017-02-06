namespace QuickGraph.Algorithms.RandomWalks
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class RoundRobinEdgeChain<TVertex, TEdge>
        : IEdgeChain<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<TVertex, int> _outEdgeIndices = new Dictionary<TVertex, int>();

        public bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> g, TVertex u, out TEdge successor)
        {
            var outDegree = g.OutDegree(u);
            if (outDegree > 0)
            {
                int index;
                if (!_outEdgeIndices.TryGetValue(u, out index))
                {
                    index = 0;
                    _outEdgeIndices.Add(u, index);
                }
                var e = g.OutEdge(u, index);
                _outEdgeIndices[u] = ++index % outDegree;

                successor = e;
                return true;
            }

            successor = default(TEdge);
            return false;
        }

        public bool TryGetSuccessor(IEnumerable<TEdge> edges, TVertex u, out TEdge successor)
        {
            var edgeCount = edges.Count();

            if (edgeCount > 0)
            {
                int index;
                if (!_outEdgeIndices.TryGetValue(u, out index))
                {
                    index = 0;
                    _outEdgeIndices.Add(u, index);
                }
                var e = edges.ElementAt(index);
                _outEdgeIndices[u] = ++index % edgeCount;
                successor = e;
            }
            successor = default(TEdge);
            return false;
        }
    }
}