namespace QuickGraph.Algorithms.MaximumFlow
{
    using System.Threading.Tasks;


    using Xunit;

    public class AllVerticesGraphAugmentorAlgorithmTest
    {
        public void Augment(
            IMutableVertexAndEdgeListGraph<string, Edge<string>> g)
        {
            var vertexCount = g.VertexCount;
            var edgeCount = g.EdgeCount;
            var vertexId = g.VertexCount + 1;
            var edgeId = g.EdgeCount + 1;
            using (var augmentor = new AllVerticesGraphAugmentorAlgorithm<string, Edge<string>>(
                g,
                () => vertexId++.ToString(),
                (s, t) => new Edge<string>(s, t)
            ))
            {
                augmentor.Compute();
                VerifyCount(g, augmentor, vertexCount);
                VerifySourceConnector(g, augmentor);
                VerifySinkConnector(g, augmentor);
            }
            Assert.Equal(g.VertexCount, vertexCount);
            Assert.Equal(g.EdgeCount, edgeCount);
        }

        //[Fact]
        //public void AugmentAll()
        //{
        //    Parallel.ForEach(TestGraphFactory.GetAdjacencyGraphs(), Augment);
        //}

        private static void VerifyCount<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor,
            int vertexCount)
            where TEdge : IEdge<TVertex>
        {
            Assert.Equal(vertexCount + 2, g.VertexCount);
            Assert.True(g.ContainsVertex(augmentor.SuperSource));
            Assert.True(g.ContainsVertex(augmentor.SuperSink));
        }

        private static void VerifySinkConnector<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor)
            where TEdge : IEdge<TVertex>
        {
            foreach (var v in g.Vertices)
            {
                if (v.Equals(augmentor.SuperSink))
                {
                    continue;
                }
                if (v.Equals(augmentor.SuperSink))
                {
                    continue;
                }
                Assert.True(g.ContainsEdge(v, augmentor.SuperSink));
            }
        }

        private static void VerifySourceConnector<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor)
            where TEdge : IEdge<TVertex>
        {
            foreach (var v in g.Vertices)
            {
                if (v.Equals(augmentor.SuperSource))
                {
                    continue;
                }
                if (v.Equals(augmentor.SuperSink))
                {
                    continue;
                }
                Assert.True(g.ContainsEdge(augmentor.SuperSource, v));
            }
        }
    }

    public sealed class StringVertexFactory
    {
        private int _id;

        public string Prefix { get; set; }

        public StringVertexFactory()
            : this("Super")
        {
        }

        public StringVertexFactory(string prefix)
        {
            Prefix = prefix;
        }

        public string CreateVertex()
        {
            return Prefix + ++_id;
        }
    }
}