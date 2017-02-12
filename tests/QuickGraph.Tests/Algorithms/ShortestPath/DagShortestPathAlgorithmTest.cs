namespace QuickGraph.Algorithms.ShortestPath
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using QuickGraph.Algorithms.Observers;
    using QuickGraph.Serialization;

    using Xunit;

    public class DagShortestPathAlgorithmTest
    {
        public void Compute<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            // is this a dag ?
            var isDag = g.IsDirectedAcyclicGraph();

            var relaxer = DistanceRelaxers.ShortestDistance;
            var vertices = new List<TVertex>(g.Vertices);
            foreach (var root in vertices)
                if (isDag)
                {
                    Search(g, root, relaxer);
                }
                else
                {
                    try
                    {
                        Search(g, root, relaxer);
                    }
                    catch (NonAcyclicGraphException)
                    {
                        TestConsole.WriteLine("NonAcyclicGraphException caught (as expected)");
                    }
                }
        }

        public void ComputeCriticalPath<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            // is this a dag ?
            var isDag = g.IsDirectedAcyclicGraph();

            var relaxer = DistanceRelaxers.CriticalDistance;
            var vertices = new List<TVertex>(g.Vertices);
            foreach (var root in vertices)
                if (isDag)
                {
                    Search(g, root, relaxer);
                }
                else
                {
                    try
                    {
                        Search(g, root, relaxer);
                        Assert.False(true, "should have found the acyclic graph");
                    }
                    catch (NonAcyclicGraphException)
                    {
                        TestConsole.WriteLine("NonAcyclicGraphException caught (as expected)");
                    }
                }
        }

        [Fact]
        public void DagShortestPathAll()
        {
            Parallel.ForEach(
                TestGraphFactory.GetAdjacencyGraphs(),
                g =>
                {
                    Compute(g);
                    ComputeCriticalPath(g);
                });
        }

        private void Search<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> g,
            TVertex root,
            IDistanceRelaxer relaxer)
            where TEdge : IEdge<TVertex>
        {
            var algo =
                new DagShortestPathAlgorithm<TVertex, TEdge>(
                    g,
                    e => 1,
                    relaxer
                );
            var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (predecessors.Attach(algo))
            {
                algo.Compute(root);
            }

            Verify(algo, predecessors);
        }

        private static void Verify<TVertex, TEdge>(
            DagShortestPathAlgorithm<TVertex, TEdge> algo,
            VertexPredecessorRecorderObserver<TVertex, TEdge> predecessors)
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
                Assert.Equal(
                    algo.Distances[v],
                    algo.Distances[predecessor.Source] + 1
                );
            }
        }
    }
}