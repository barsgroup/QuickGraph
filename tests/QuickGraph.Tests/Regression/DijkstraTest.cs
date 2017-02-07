namespace QuickGraph.Tests.Regression
{
    using System.Collections.Generic;

    using QuickGraph.Algorithms;
    using QuickGraph.Algorithms.Observers;
    using QuickGraph.Algorithms.ShortestPath;

    using Xunit;

    public class DijkstraTest
    {
        [Fact]
        public void Scenario()
        {
            var graph = new AdjacencyGraph<string, Edge<string>>(true);

            // Add some vertices to the graph
            graph.AddVertex("A");
            graph.AddVertex("B");
            graph.AddVertex("C");
            graph.AddVertex("D");
            graph.AddVertex("E");
            graph.AddVertex("F");
            graph.AddVertex("G");
            graph.AddVertex("H");
            graph.AddVertex("I");
            graph.AddVertex("J");

            // Create the edges
            var aB = new Edge<string>("A", "B");
            var aD = new Edge<string>("A", "D");
            var bA = new Edge<string>("B", "A");
            var bC = new Edge<string>("B", "C");
            var bE = new Edge<string>("B", "E");
            var cB = new Edge<string>("C", "B");
            var cF = new Edge<string>("C", "F");
            var cJ = new Edge<string>("C", "J");
            var dE = new Edge<string>("D", "E");
            var dG = new Edge<string>("D", "G");
            var eD = new Edge<string>("E", "D");
            var eF = new Edge<string>("E", "F");
            var eH = new Edge<string>("E", "H");
            var fI = new Edge<string>("F", "I");
            var fJ = new Edge<string>("F", "J");
            var gD = new Edge<string>("G", "D");
            var gH = new Edge<string>("G", "H");
            var hG = new Edge<string>("H", "G");
            var hI = new Edge<string>("H", "I");
            var iF = new Edge<string>("I", "F");
            var iJ = new Edge<string>("I", "J");
            var iH = new Edge<string>("I", "H");
            var jF = new Edge<string>("J", "F");

            // Add the edges
            graph.AddEdge(aB);
            graph.AddEdge(aD);
            graph.AddEdge(bA);
            graph.AddEdge(bC);
            graph.AddEdge(bE);
            graph.AddEdge(cB);
            graph.AddEdge(cF);
            graph.AddEdge(cJ);
            graph.AddEdge(dE);
            graph.AddEdge(dG);
            graph.AddEdge(eD);
            graph.AddEdge(eF);
            graph.AddEdge(eH);
            graph.AddEdge(fI);
            graph.AddEdge(fJ);
            graph.AddEdge(gD);
            graph.AddEdge(gH);
            graph.AddEdge(hG);
            graph.AddEdge(hI);
            graph.AddEdge(iF);
            graph.AddEdge(iH);
            graph.AddEdge(iJ);
            graph.AddEdge(jF);

            // Define some weights to the edges
            var edgeCost = new Dictionary<Edge<string>, double>(graph.EdgeCount);
            edgeCost.Add(aB, 4);
            edgeCost.Add(aD, 1);
            edgeCost.Add(bA, 74);
            edgeCost.Add(bC, 2);
            edgeCost.Add(bE, 12);
            edgeCost.Add(cB, 12);
            edgeCost.Add(cF, 74);
            edgeCost.Add(cJ, 12);
            edgeCost.Add(dE, 32);
            edgeCost.Add(dG, 22);
            edgeCost.Add(eD, 66);
            edgeCost.Add(eF, 76);
            edgeCost.Add(eH, 33);
            edgeCost.Add(fI, 11);
            edgeCost.Add(fJ, 21);
            edgeCost.Add(gD, 12);
            edgeCost.Add(gH, 10);
            edgeCost.Add(hG, 2);
            edgeCost.Add(hI, 72);
            edgeCost.Add(iF, 31);
            edgeCost.Add(iH, 18);
            edgeCost.Add(iJ, 7);
            edgeCost.Add(jF, 8);

            // We want to use Dijkstra on this graph
            var dijkstra = new DijkstraShortestPathAlgorithm<string, Edge<string>>(graph, e => edgeCost[e]);

            // Attach a Vertex Predecessor Recorder Observer to give us the paths
            var predecessorObserver = new VertexPredecessorRecorderObserver<string, Edge<string>>();
            using (predecessorObserver.Attach(dijkstra))
            {
                // Run the algorithm with A set to be the source
                dijkstra.Compute("A");
            }

            foreach (var kvp in predecessorObserver.VertexPredecessors)
                TestConsole.WriteLine("If you want to get to {0} you have to enter through the in edge {1}", kvp.Key, kvp.Value);

            foreach (var v in graph.Vertices)
            {
                var distance =
                    AlgorithmExtensions.ComputePredecessorCost(
                        predecessorObserver.VertexPredecessors,
                        edgeCost,
                        v);
                TestConsole.WriteLine("A -> {0}: {1}", v, distance);
            }
        }
    }
}