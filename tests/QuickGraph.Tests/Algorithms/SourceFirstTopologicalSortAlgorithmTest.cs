namespace QuickGraph.Algorithms
{
    using System.Threading.Tasks;

    using QuickGraph.Algorithms.TopologicalSort;
    using QuickGraph.Serialization;

    using Xunit;

    public class SourceFirstTopologicalSortAlgorithmTest
    {
        public void Sort<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var topo = new SourceFirstTopologicalSortAlgorithm<TVertex, TEdge>(g);
            try
            {
                topo.Compute();
            }
            catch (NonAcyclicGraphException)
            {
            }
        }

        [Fact]
        public void SortAll()
        {
            Parallel.ForEach(
                TestGraphFactory.GetAdjacencyGraphs(),
                g =>
                    Sort(g));
        }
    }
}