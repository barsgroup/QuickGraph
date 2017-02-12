namespace QuickGraph.Algorithms.ShortestPath
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using QuickGraph.Algorithms.Observers;
    using QuickGraph.Serialization;

    using Xunit;

    public class DijkstraShortestPathAlgorithmTest
    {
        public void Dijkstra<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g, TVertex root)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>(g.EdgeCount);
            foreach (var e in g.Edges)
                distances[e] = g.OutDegree(e.Source) + 1;

            var algo = new DijkstraShortestPathAlgorithm<TVertex, TEdge>(
                g,
                e => distances[e]
            );
            var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (predecessors.Attach(algo))
            {
                algo.Compute(root);
            }

            Verify(algo, predecessors);
        }

        [Fact]
        public void DijkstraAll()
        {
            Parallel.ForEach(
                TestGraphFactory.GetAdjacencyGraphs(),
                g =>
                {
                    foreach (var root in g.Vertices)
                        Dijkstra(g, root);
                });
        }

        [Fact]
        public void Repro12359()
        {
            var g = new AdjacencyGraph<string, Edge<string>>();
            TestGraphFactory.LoadGraph("netcoreapp1.1/GraphML/repro12359.graphml", g);
            var i = 0;
            foreach (var v in g.Vertices)
            {
                if (i++ > 5)
                {
                    break;
                }
                Dijkstra(g, v);
            }
        }

        private static void Verify<TVertex, TEdge>(
            DijkstraShortestPathAlgorithm<TVertex, TEdge> algo,
            VertexPredecessorRecorderObserver<TVertex, TEdge> predecessors
        )
            where TEdge : IEdge<TVertex>
        {
            // let's verify the result
            foreach (var v in algo.VisitedGraph.Vertices)
            {
                TEdge predecessor;
                if (!predecessors.VertexPredecessors.TryGetValue(v, out predecessor))
                {
                    continue;
                }
                if (predecessor.Source.Equals(v))
                {
                    continue;
                }
                double vd, vp;
                bool found;
                Assert.Equal(
                    found = algo.TryGetDistance(v, out vd),
                    algo.TryGetDistance(predecessor.Source, out vp)
                );
            }
        }
    }
}