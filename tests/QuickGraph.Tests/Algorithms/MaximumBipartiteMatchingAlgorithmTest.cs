namespace QuickGraph.Tests.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using QuickGraph.Algorithms;
    using QuickGraph.Algorithms.MaximumFlow;

    using Xunit;

    public class MaximumBipartiteMatchingAlgorithmTest
    {
        private readonly EdgeFactory<string, Edge<string>> _edgeFactory =
            (source, target) => new Edge<string>(source, target);

        [Fact]
        public void BipartiteMaxMatchSimpleReversedEdgesTest()
        {
            var integers = Enumerable.Range(0, 100);
            var even = integers.Where(n => n % 2 == 0).Select(n => n.ToString());
            var odd = integers.Where(n => n % 2 != 0).Select(n => n.ToString());

            // Create the edges from odd to even
            var edges = TestHelper.CreateAllPairwiseEdges(odd, even, _edgeFactory);

            var setA = even;
            var setB = odd;
            var expectedMatchSize = Math.Min(setA.Count(), setB.Count());

            RunBipartiteMatch(edges, setA, setB, expectedMatchSize);
        }

        [Fact]
        public void BipartiteMaxMatchSimpleTest()
        {
            var integers = Enumerable.Range(0, 100);
            var even = integers.Where(n => n % 2 == 0).Select(n => n.ToString());
            var odd = integers.Where(n => n % 2 != 0).Select(n => n.ToString());

            // Create the edges from even to odd
            var edges = TestHelper.CreateAllPairwiseEdges(even, odd, _edgeFactory);

            var setA = even;
            var setB = odd;
            var expectedMatchSize = Math.Min(setA.Count(), setB.Count());

            RunBipartiteMatch(edges, setA, setB, expectedMatchSize);
        }

        [Fact]
        public void BipartiteMaxMatchTwoFullyConnectedSetsTest()
        {
            var setA = new List<string>();
            var setB = new List<string>();

            var nodesInSet1 = 100;
            var nodesInSet2 = 10;

            //Create a set of vertices in each set which all match each other
            var integers = Enumerable.Range(0, nodesInSet1);
            var even = integers.Where(n => n % 2 == 0).Select(n => n.ToString());
            var odd = integers.Where(n => n % 2 != 0).Select(n => n.ToString());
            var edges = TestHelper.CreateAllPairwiseEdges(even, odd, _edgeFactory);

            setA.AddRange(even);
            setB.AddRange(odd);

            //Create another set of vertices in each set which all match each other
            integers = Enumerable.Range(nodesInSet1 + 1, nodesInSet2);
            even = integers.Where(n => n % 2 == 0).Select(n => n.ToString());
            odd = integers.Where(n => n % 2 != 0).Select(n => n.ToString());
            edges.AddRange(TestHelper.CreateAllPairwiseEdges(even, odd, _edgeFactory));

            setA.AddRange(even);
            setB.AddRange(odd);

            var expectedMatchSize = Math.Min(setA.Count(), setB.Count());

            RunBipartiteMatch(edges, setA, setB, expectedMatchSize);
        }

        [Fact]
        public void BipartiteMaxMatchUnequalPartitionsTest()
        {
            var setA = new List<string>();
            var setB = new List<string>();

            //Create a bipartite graph with small and large vertex partitions
            var smallerSetSize = 1;
            var largerSetSize = 1000;

            //Create a set of vertices in each set which all match each other
            var leftNodes = Enumerable.Range(0, smallerSetSize).Select(n => "L" + n);
            var rightNodes = Enumerable.Range(0, largerSetSize).Select(n => "R" + n);
            var edges = TestHelper.CreateAllPairwiseEdges(leftNodes, rightNodes, _edgeFactory);

            setA.AddRange(leftNodes);
            setB.AddRange(rightNodes);

            var expectedMatchSize = Math.Min(setA.Count(), setB.Count());

            RunBipartiteMatch(edges, setA, setB, expectedMatchSize);
        }

        public MaximumBipartiteMatchingAlgorithm<TVertex, TEdge> MaxBipartiteMatch<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            IEnumerable<TVertex> vertexSetA,
            IEnumerable<TVertex> vertexSetB,
            VertexFactory<TVertex> vertexFactory,
            EdgeFactory<TVertex, TEdge> edgeFactory,
            int expectedMatchSize)
            where TEdge : IEdge<TVertex>
        {
            var maxMatch = new MaximumBipartiteMatchingAlgorithm<TVertex, TEdge>(
                g,
                vertexSetA,
                vertexSetB,
                vertexFactory,
                edgeFactory);

            var startTime = DateTime.Now;

            maxMatch.Compute();

            var computeTime = DateTime.Now - startTime;

            Assert.True(computeTime < TimeSpan.FromMinutes(5));

            AssertThatMaxMatchEdgesAreValid(vertexSetA, vertexSetB, maxMatch);

            Assert.True(maxMatch.MatchedEdges.Count == expectedMatchSize);

            return maxMatch;
        }

        private static void AssertThatMaxMatchEdgesAreValid<TVertex, TEdge>(IEnumerable<TVertex> vertexSetA,
                                                                            IEnumerable<TVertex> vertexSetB,
                                                                            MaximumBipartiteMatchingAlgorithm<TVertex, TEdge> maxMatch) where TEdge : IEdge<TVertex>
        {
            foreach (var edge in maxMatch.MatchedEdges)
            {
                var isValidEdge = vertexSetA.Contains(edge.Source) && vertexSetB.Contains(edge.Target) ||
                                  vertexSetB.Contains(edge.Source) && vertexSetA.Contains(edge.Target);
                Assert.True(isValidEdge, "match contains invalid edges");
            }
        }

        private void RunBipartiteMatch(List<Edge<string>> edges,
                                       IEnumerable<string> setA,
                                       IEnumerable<string> setB,
                                       int expectedMatchSize)
        {
            var graph = edges.ToAdjacencyGraph<string, Edge<string>>();

            var vertexFactory = new StringVertexFactory();

            if (graph.VertexCount > 0)
            {
                MaxBipartiteMatch(
                    graph,
                    setA,
                    setB,
                    () => vertexFactory.CreateVertex(),
                    (source, target) => new Edge<string>(source, target),
                    expectedMatchSize
                );
            }
        }
    }
}