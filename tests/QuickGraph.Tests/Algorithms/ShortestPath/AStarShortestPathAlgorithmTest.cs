namespace QuickGraph.Algorithms.ShortestPath
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using QuickGraph.Algorithms.Observers;
    using QuickGraph.Serialization;
    using QuickGraph.Tests.Traits;

    using Xunit;

    public class AStartShortestPathAlgorithmTest
    {
        public void AStar<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g, TVertex root)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (var e in g.Edges)
                distances[e] = g.OutDegree(e.Source) + 1;

            var algo = new AStarShortestPathAlgorithm<TVertex, TEdge>(
                g,
                e => distances[e],
                v => 0
            );
            var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (predecessors.Attach(algo))
            {
                algo.Compute(root);
            }

            Verify(algo, predecessors);
        }

        [Fact]
        [TestCategory(TestCategories.LongRunning)]
        public void AStartAll()
        {
            Parallel.ForEach(
                TestGraphFactory.GetAdjacencyGraphs(),
                g =>
                {
                    foreach (var root in g.Vertices)
                        AStar(g, root);
                });
        }

        private static void Verify<TVertex, TEdge>(
            AStarShortestPathAlgorithm<TVertex, TEdge> algo,
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