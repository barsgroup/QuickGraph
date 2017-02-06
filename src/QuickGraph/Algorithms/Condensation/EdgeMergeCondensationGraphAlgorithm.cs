namespace QuickGraph.Algorithms.Condensation
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public sealed class EdgeMergeCondensationGraphAlgorithm<TVertex, TEdge> :
        AlgorithmBase<IBidirectionalGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        public IMutableBidirectionalGraph<TVertex,
            MergedEdge<TVertex, TEdge>
        > CondensatedGraph { get; }

        public VertexPredicate<TVertex> VertexPredicate { get; }

        public EdgeMergeCondensationGraphAlgorithm(
            IBidirectionalGraph<TVertex, TEdge> visitedGraph,
            IMutableBidirectionalGraph<TVertex, MergedEdge<TVertex, TEdge>> condensatedGraph,
            VertexPredicate<TVertex> vertexPredicate
        )
            : base(visitedGraph)
        {
            Contract.Requires(condensatedGraph != null);
            Contract.Requires(vertexPredicate != null);

            CondensatedGraph = condensatedGraph;
            VertexPredicate = vertexPredicate;
        }

        protected override void InternalCompute()
        {
            // adding vertices to the new graph
            // and pusing filtered vertices in queue
            var filteredVertices = new Queue<TVertex>();
            foreach (var v in VisitedGraph.Vertices)
            {
                CondensatedGraph.AddVertex(v);
                if (!VertexPredicate(v))
                {
                    filteredVertices.Enqueue(v);
                }
            }

            // adding all edges
            foreach (var edge in VisitedGraph.Edges)
            {
                var mergedEdge = new MergedEdge<TVertex, TEdge>(edge.Source, edge.Target);
                mergedEdge.Edges.Add(edge);

                CondensatedGraph.AddEdge(mergedEdge);
            }

            // remove vertices
            while (filteredVertices.Count > 0)
            {
                var filteredVertex = filteredVertices.Dequeue();

                // do the cross product between inedges and outedges
                MergeVertex(filteredVertex);
            }
        }

        private void MergeVertex(TVertex v)
        {
            // get in edges and outedge
            var inEdges =
                new List<MergedEdge<TVertex, TEdge>>(CondensatedGraph.InEdges(v));
            var outEdges =
                new List<MergedEdge<TVertex, TEdge>>(CondensatedGraph.OutEdges(v));

            // remove vertex
            CondensatedGraph.RemoveVertex(v);

            // add condensated edges
            for (var i = 0; i < inEdges.Count; ++i)
            {
                var inEdge = inEdges[i];
                if (inEdge.Source.Equals(v))
                {
                    continue;
                }

                for (var j = 0; j < outEdges.Count; ++j)
                {
                    var outEdge = outEdges[j];
                    if (outEdge.Target.Equals(v))
                    {
                        continue;
                    }

                    var newEdge =
                        MergedEdge<TVertex, TEdge>.Merge(inEdge, outEdge);
                    CondensatedGraph.AddEdge(newEdge);
                }
            }
        }
    }
}