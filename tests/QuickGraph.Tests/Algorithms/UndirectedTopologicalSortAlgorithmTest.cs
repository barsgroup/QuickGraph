namespace QuickGraph.Algorithms
{
    using QuickGraph.Algorithms.TopologicalSort;

    public class UndirectedTopologicalSortAlgorithmTest
    {
        public void Compute(IUndirectedGraph<string, Edge<string>> g)
        {
            var topo =
                new UndirectedTopologicalSortAlgorithm<string, Edge<string>>(g);
            topo.AllowCyclicGraph = true;
            topo.Compute();

            Display(topo);
        }

        private void Display(UndirectedTopologicalSortAlgorithm<string, Edge<string>> topo)
        {
            var index = 0;
            foreach (var v in topo.SortedVertices)
                TestConsole.WriteLine("{0}: {1}", index++, v);
        }
    }
}