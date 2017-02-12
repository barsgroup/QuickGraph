namespace QuickGraph.Algorithms
{
    using QuickGraph.Algorithms.TopologicalSort;
    using QuickGraph.Serialization;

    using Xunit;

    public class UndirectedFirstTopologicalSortAlgorithmTest
    {
        public void Compute<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var topo =
                new UndirectedFirstTopologicalSortAlgorithm<TVertex, TEdge>(g);
            topo.AllowCyclicGraph = true;
            topo.Compute();
        }

        [Fact]
        public void UndirectedFirstTopologicalSortAll()
        {
            foreach (var g in TestGraphFactory.GetUndirectedGraphs())
                Compute(g);
        }
    }
}