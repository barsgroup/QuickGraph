﻿namespace QuickGraph.Algorithms.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using QuickGraph.Serialization;

    using Xunit;

    public class BreadthFirstAlgorithmSearchTest
    {
        [Fact]
        public void BreadthFirstSearchAll()
        {
            Parallel.ForEach(
                TestGraphFactory.GetAdjacencyGraphs(),
                g =>
                {
                    foreach (var v in g.Vertices)
                        RunBfs(g, v);
                });
        }

        public void RunBfs<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g, TVertex sourceVertex)
            where TEdge : IEdge<TVertex>
        {
            var parents = new Dictionary<TVertex, TVertex>();
            var distances = new Dictionary<TVertex, int>();
            var currentVertex = default(TVertex);
            var currentDistance = 0;
            var algo = new BreadthFirstSearchAlgorithm<TVertex, TEdge>(g);

            algo.InitializeVertex += u =>
            {
                Assert.Equal(algo.VertexColors[u], GraphColor.White);
            };

            algo.DiscoverVertex += u =>
            {
                Assert.Equal(algo.VertexColors[u], GraphColor.Gray);
                if (u.Equals(sourceVertex))
                {
                    currentVertex = sourceVertex;
                }
                else
                {
                    Assert.NotNull(currentVertex);
                    Assert.Equal(parents[u], currentVertex);
                    Assert.Equal(distances[u], currentDistance + 1);
                    Assert.Equal(distances[u], distances[parents[u]] + 1);
                }
            };
            algo.ExamineEdge += args =>
            {
                Assert.Equal(args.Source, currentVertex);
            };

            algo.ExamineVertex += args =>
            {
                var u = args;
                currentVertex = u;

                // Ensure that the distances monotonically increase.
                Assert.True(
                    distances[u] == currentDistance
                    || distances[u] == currentDistance + 1
                );

                if (distances[u] == currentDistance + 1) // new level
                {
                    ++currentDistance;
                }
            };
            algo.TreeEdge += args =>
            {
                var u = args.Source;
                var v = args.Target;

                Assert.Equal(algo.VertexColors[v], GraphColor.White);
                Assert.Equal(distances[u], currentDistance);
                parents[v] = u;
                distances[v] = distances[u] + 1;
            };
            algo.NonTreeEdge += args =>
            {
                var u = args.Source;
                var v = args.Target;

                Assert.False(algo.VertexColors[v] == GraphColor.White);

                if (algo.VisitedGraph.IsDirected)
                {
                    // cross or back edge
                    Assert.True(distances[v] <= distances[u] + 1);
                }
                else
                {
                    // cross edge (or going backwards on a tree edge)
                    Assert.True(
                        distances[v] == distances[u]
                        || distances[v] == distances[u] + 1
                        || distances[v] == distances[u] - 1
                    );
                }
            };

            algo.GrayTarget += args =>
            {
                Assert.Equal(algo.VertexColors[args.Target], GraphColor.Gray);
            };
            algo.BlackTarget += args =>
            {
                Assert.Equal(algo.VertexColors[args.Target], GraphColor.Black);

                foreach (var e in algo.VisitedGraph.OutEdges(args.Target))
                    Assert.False(algo.VertexColors[e.Target] == GraphColor.White);
            };

            algo.FinishVertex += args =>
            {
                Assert.Equal(algo.VertexColors[args], GraphColor.Black);
            };

            parents.Clear();
            distances.Clear();
            currentDistance = 0;

            foreach (var v in g.Vertices)
            {
                distances[v] = int.MaxValue;
                parents[v] = v;
            }
            distances[sourceVertex] = 0;
            algo.Compute(sourceVertex);

            // All white vertices should be unreachable from the source.
            foreach (var v in g.Vertices)
                if (algo.VertexColors[v] == GraphColor.White)
                {
                    //!IsReachable(start,u,g);
                }

            // The shortest path to a child should be one longer than
            // shortest path to the parent.
            foreach (var v in g.Vertices)
                if (!parents[v].Equals(v)) // *ui not the root of the bfs tree
                {
                    Assert.Equal(distances[v], distances[parents[v]] + 1);
                }
        }
    }
}