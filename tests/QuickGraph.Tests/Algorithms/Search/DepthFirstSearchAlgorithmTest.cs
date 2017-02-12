﻿namespace QuickGraph.Algorithms.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using QuickGraph.Serialization;

    using Xunit;

    public class DepthFirstAlgorithmSearchTest
    {
        public void DepthFirstSearch<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var parents = new Dictionary<TVertex, TVertex>();
            var discoverTimes = new Dictionary<TVertex, int>();
            var finishTimes = new Dictionary<TVertex, int>();
            var time = 0;
            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(g);

            dfs.StartVertex += args =>
            {
                Assert.Equal(dfs.VertexColors[args], GraphColor.White);
                Assert.False(parents.ContainsKey(args));
                parents[args] = args;
            };

            dfs.DiscoverVertex += args =>
            {
                Assert.Equal(dfs.VertexColors[args], GraphColor.Gray);
                Assert.Equal(dfs.VertexColors[parents[args]], GraphColor.Gray);

                discoverTimes[args] = time++;
            };

            dfs.ExamineEdge += args =>
            {
                Assert.Equal(dfs.VertexColors[args.Source], GraphColor.Gray);
            };

            dfs.TreeEdge += args =>
            {
                Assert.Equal(dfs.VertexColors[args.Target], GraphColor.White);
                parents[args.Target] = args.Source;
            };

            dfs.BackEdge += args =>
            {
                Assert.Equal(dfs.VertexColors[args.Target], GraphColor.Gray);
            };

            dfs.ForwardOrCrossEdge += args =>
            {
                Assert.Equal(dfs.VertexColors[args.Target], GraphColor.Black);
            };

            dfs.FinishVertex += args =>
            {
                Assert.Equal(dfs.VertexColors[args], GraphColor.Black);
                finishTimes[args] = time++;
            };

            dfs.Compute();

            // check
            // all vertices should be black
            foreach (var v in g.Vertices)
            {
                Assert.True(dfs.VertexColors.ContainsKey(v));
                Assert.Equal(dfs.VertexColors[v], GraphColor.Black);
            }

            foreach (var u in g.Vertices)
            foreach (var v in g.Vertices)
                if (!u.Equals(v))
                {
                    Assert.True(
                        finishTimes[u] < discoverTimes[v]
                        || finishTimes[v] < discoverTimes[u]
                        || discoverTimes[v] < discoverTimes[u]
                        && finishTimes[u] < finishTimes[v]
                        && IsDescendant(parents, u, v)
                        || discoverTimes[u] < discoverTimes[v]
                        && finishTimes[v] < finishTimes[u]
                        && IsDescendant(parents, v, u)
                    );
                }
        }

        [Fact]
        public void DepthFirstSearchAll()
        {
            Parallel.ForEach(TestGraphFactory.GetAdjacencyGraphs(), DepthFirstSearch);
        }

        private static bool IsDescendant<TVertex>(
            Dictionary<TVertex, TVertex> parents,
            TVertex u,
            TVertex v)
        {
            TVertex t;
            var p = u;
            do
            {
                t = p;
                p = parents[t];
                if (p.Equals(v))
                {
                    return true;
                }
            }
            while (!t.Equals(p));

            return false;
        }
    }
}