namespace QuickGraph.Tests.Algorithms
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using QuickGraph.Algorithms;
    using QuickGraph.Algorithms.Observers;
    using QuickGraph.Algorithms.Search;

    using Xunit;

    public class TarjanOfflineLeastCommonAncestorAlgorithmTest
    {
        public void TarjanOfflineLeastCommonAncestorAlgorithm<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> g,
            TVertex root,
            SEquatableEdge<TVertex>[] pairs
        )
            where TEdge : IEdge<TVertex>
        {
            var lca = g.OfflineLeastCommonAncestorTarjan(root, pairs);
            var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(g);
            using (predecessors.Attach(dfs))
            {
                dfs.Compute(root);
            }

            TVertex ancestor;
            foreach (var pair in pairs)
                if (lca(pair, out ancestor))
                {
                    Assert.True(predecessors.VertexPredecessors.IsPredecessor(root, pair.Source));
                    Assert.True(predecessors.VertexPredecessors.IsPredecessor(root, pair.Target));
                }
        }

        //[Fact(Skip="legacy")]
        //public void TarjanOfflineLeastCommonAncestorAlgorithmAll()
        //{
        //    Parallel.ForEach(
        //        TestGraphFactory.GetAdjacencyGraphs(),
        //        g =>
        //        {
        //            if (g.VertexCount == 0)
        //            {
        //                return;
        //            }

        //            var pairs = new List<SEquatableEdge<string>>();
        //            foreach (var v in g.Vertices)
        //            foreach (var w in g.Vertices)
        //                if (!v.Equals(w))
        //                {
        //                    pairs.Add(new SEquatableEdge<string>(v, w));
        //                }

        //            var count = 0;
        //            foreach (var root in g.Vertices)
        //            {
        //                TarjanOfflineLeastCommonAncestorAlgorithm(
        //                    g,
        //                    root,
        //                    pairs.ToArray());
        //                if (count++ > 10)
        //                {
        //                    break;
        //                }
        //            }
        //        });
        //}
    }
}