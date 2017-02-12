namespace QuickGraph.Tests.Algorithms.MaximumFlow
{
    using System.Threading.Tasks;

    using QuickGraph.Algorithms;
    using QuickGraph.Serialization;

    using Xunit;

    public class EdmondsKarpMaximumFlowAlgorithmTest
    {
        public void EdmondsKarpMaxFlow<TVertex, TEdge>(IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
                                                       EdgeFactory<TVertex, TEdge> edgeFactory)
            where TEdge : IEdge<TVertex>
        {
            foreach (var source in g.Vertices)
            foreach (var sink in g.Vertices)
            {
                if (source.Equals(sink))
                {
                    continue;
                }

                RunMaxFlowAlgorithm(g, edgeFactory, source, sink);
            }
        }

        [Fact]
        public void EdmondsKarpMaxFlowAll()
        {
            Parallel.ForEach(
                TestGraphFactory.GetAdjacencyGraphs(),
                g =>
                {
                    if (g.VertexCount > 0)
                    {
                        EdmondsKarpMaxFlow(g, (source, target) => new Edge<string>(source, target));
                    }
                });
        }

        private static double RunMaxFlowAlgorithm<TVertex, TEdge>(IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
                                                                  EdgeFactory<TVertex, TEdge> edgeFactory,
                                                                  TVertex source,
                                                                  TVertex sink) where TEdge : IEdge<TVertex>
        {
            TryFunc<TVertex, TEdge> flowPredecessors;
            var flow = AlgorithmExtensions.MaximumFlowEdmondsKarp(
                g,
                e => 1,
                source,
                sink,
                out flowPredecessors,
                edgeFactory
            );

            return flow;
        }
    }
}