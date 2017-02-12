namespace QuickGraph.Algorithms.ShortestPath
{
    using System.Collections.Generic;
    using System.Linq;

    using QuickGraph.Algorithms.Observers;

    using Xunit;

    public class DijkstraShortestPathTestOld
    {
        [Fact]
        public void CheckPredecessorDoubleLineGraph()
        {
            var g = new AdjacencyGraph<int, Edge<int>>(true);
            g.AddVertex(1);
            g.AddVertex(2);
            g.AddVertex(3);

            var e12 = new Edge<int>(1, 2);
            g.AddEdge(e12);
            var e23 = new Edge<int>(2, 3);
            g.AddEdge(e23);
            var e13 = new Edge<int>(1, 3);
            g.AddEdge(e13);

            var dij = new DijkstraShortestPathAlgorithm<int, Edge<int>>(g, e => 1);
            var vis = new VertexPredecessorRecorderObserver<int, Edge<int>>();
            using (vis.Attach(dij))
            {
                dij.Compute(1);
            }

            IEnumerable<Edge<int>> path;
            Assert.True(vis.TryGetPath(2, out path));
            var col = path.ToList();
            Assert.Equal(1, col.Count);
            Assert.Equal(e12, col[0]);

            Assert.True(vis.TryGetPath(3, out path));
            col = path.ToList();
            Assert.Equal(1, col.Count);
            Assert.Equal(e13, col[0]);
        }

        [Fact]
        public void CheckPredecessorLineGraph()
        {
            var g = new AdjacencyGraph<int, Edge<int>>(true);
            g.AddVertex(1);
            g.AddVertex(2);
            g.AddVertex(3);

            var e12 = new Edge<int>(1, 2);
            g.AddEdge(e12);
            var e23 = new Edge<int>(2, 3);
            g.AddEdge(e23);

            var dij = new DijkstraShortestPathAlgorithm<int, Edge<int>>(g, e => 1);
            var vis = new VertexPredecessorRecorderObserver<int, Edge<int>>();
            using (vis.Attach(dij))
            {
                dij.Compute(1);
            }

            IEnumerable<Edge<int>> path;
            Assert.True(vis.TryGetPath(2, out path));
            var col = path.ToList();
            Assert.Equal(1, col.Count);
            Assert.Equal(e12, col[0]);

            Assert.True(vis.TryGetPath(3, out path));
            col = path.ToList();
            Assert.Equal(2, col.Count);
            Assert.Equal(e12, col[0]);
            Assert.Equal(e23, col[1]);
        }

        [Fact]
        public void RunOnDoubleLineGraph()
        {
            var g = new AdjacencyGraph<int, Edge<int>>(true);
            g.AddVertex(1);
            g.AddVertex(2);
            g.AddVertex(3);

            var e12 = new Edge<int>(1, 2);
            g.AddEdge(e12);
            var e23 = new Edge<int>(2, 3);
            g.AddEdge(e23);
            var e13 = new Edge<int>(1, 3);
            g.AddEdge(e13);

            var dij = new DijkstraShortestPathAlgorithm<int, Edge<int>>(g, e => 1);
            dij.Compute(1);

            Assert.Equal(0.0, dij.Distances[1]);
            Assert.Equal(1.0, dij.Distances[2]);
            Assert.Equal(1.0, dij.Distances[3]);
        }

        [Fact]
        public void RunOnLineGraph()
        {
            var g = new AdjacencyGraph<int, Edge<int>>(true);
            g.AddVertex(1);
            g.AddVertex(2);
            g.AddVertex(3);

            g.AddEdge(new Edge<int>(1, 2));
            g.AddEdge(new Edge<int>(2, 3));

            var dij = new DijkstraShortestPathAlgorithm<int, Edge<int>>(g, e => 1);
            dij.Compute(1);

            Assert.Equal(0, dij.Distances[1]);
            Assert.Equal(1, dij.Distances[2]);
            Assert.Equal(2, dij.Distances[3]);
        }
    }

    public class DijkstraAlgoTest
    {
        private DijkstraShortestPathAlgorithm<string, Edge<string>> _algo;

        private AdjacencyGraph<string, Edge<string>> _graph;

        private VertexPredecessorRecorderObserver<string, Edge<string>> _predecessorObserver;

        [Fact]
        public void CreateGraph()
        {
            _graph = new AdjacencyGraph<string, Edge<string>>(true);

            // Add some vertices to the graph
            _graph.AddVertex("A");
            _graph.AddVertex("B");

            _graph.AddVertex("D");
            _graph.AddVertex("C");
            _graph.AddVertex("E");

            // Create the edges
            var aB = new Edge<string>("A", "B");
            var aC = new Edge<string>("A", "C");
            var bE = new Edge<string>("B", "E");
            var cD = new Edge<string>("C", "D");
            var dE = new Edge<string>("D", "E");

            // Add edges to the graph
            _graph.AddEdge(aB);
            _graph.AddEdge(aC);
            _graph.AddEdge(cD);
            _graph.AddEdge(dE);
            _graph.AddEdge(bE);

            // Define some weights to the edges
            var weight = new Dictionary<Edge<string>, double>(_graph.EdgeCount);
            weight.Add(aB, 30);
            weight.Add(aC, 30);
            weight.Add(bE, 60);
            weight.Add(cD, 40);
            weight.Add(dE, 4);

            _algo = new DijkstraShortestPathAlgorithm<string, Edge<string>>(_graph, e => weight[e]);

            // Attach a Vertex Predecessor Recorder Observer to give us the paths
            _predecessorObserver = new VertexPredecessorRecorderObserver<string, Edge<string>>();

            using (_predecessorObserver.Attach(_algo))

                // Run the algorithm with A set to be the source
            {
                _algo.Compute("A");
            }

            Assert.True(_algo.Distances["E"] == 74);
        }
    }

    public class BoostDijkstraTest
    {
        [Fact]
        public void Compute()
        {
            var g = new AdjacencyGraph<char, Edge<char>>();
            var distances = new Dictionary<Edge<char>, double>();

            g.AddVertexRange("ABCDE");
            AddEdge(g, distances, 'A', 'C', 1);
            AddEdge(g, distances, 'B', 'B', 2);
            AddEdge(g, distances, 'B', 'D', 1);
            AddEdge(g, distances, 'B', 'E', 2);
            AddEdge(g, distances, 'C', 'B', 7);
            AddEdge(g, distances, 'C', 'D', 3);
            AddEdge(g, distances, 'D', 'E', 1);
            AddEdge(g, distances, 'E', 'A', 1);
            AddEdge(g, distances, 'E', 'B', 1);

            var dijkstra = new DijkstraShortestPathAlgorithm<char, Edge<char>>(g, AlgorithmExtensions.GetIndexer(distances));
            var predecessors = new VertexPredecessorRecorderObserver<char, Edge<char>>();

            using (predecessors.Attach(dijkstra))
            {
                dijkstra.Compute('A');
            }

            Assert.Equal(0, dijkstra.Distances['A']);
            Assert.Equal(6, dijkstra.Distances['B']);
            Assert.Equal(1, dijkstra.Distances['C']);
            Assert.Equal(4, dijkstra.Distances['D']);
            Assert.Equal(5, dijkstra.Distances['E']);
        }

        private static void AddEdge(
            AdjacencyGraph<char, Edge<char>> g,
            Dictionary<Edge<char>, double> distances,
            char source,
            char target,
            double weight)
        {
            var ac = new Edge<char>(source, target);
            distances[ac] = weight;
            g.AddEdge(ac);
        }
    }
}