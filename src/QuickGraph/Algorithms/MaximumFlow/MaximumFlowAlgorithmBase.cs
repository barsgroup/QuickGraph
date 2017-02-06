namespace QuickGraph.Algorithms.MaximumFlow
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Services;

    /// <summary>Abstract base class for maximum flow algorithms.</summary>
    public abstract class MaximumFlowAlgorithm<TVertex, TEdge> :
        AlgorithmBase<IMutableVertexAndEdgeListGraph<TVertex, TEdge>>,
        IVertexColorizerAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private TVertex _sink;

        private TVertex _source;

        protected MaximumFlowAlgorithm(
            IAlgorithmComponent host,
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> capacities,
            EdgeFactory<TVertex, TEdge> edgeFactory
        )
            : base(host, visitedGraph)
        {
            Contract.Requires(capacities != null);

            Capacities = capacities;
            Predecessors = new Dictionary<TVertex, TEdge>();
            EdgeFactory = edgeFactory;
            ResidualCapacities = new Dictionary<TEdge, double>();
            VertexColors = new Dictionary<TVertex, GraphColor>();
        }

        public double Compute(TVertex source, TVertex sink)
        {
            Source = source;
            Sink = sink;
            Compute();
            return MaxFlow;
        }

        #region Properties

        public Dictionary<TVertex, TEdge> Predecessors { get; private set; }

        public Func<TEdge, double> Capacities { get; private set; }

        public Dictionary<TEdge, double> ResidualCapacities { get; private set; }

        public EdgeFactory<TVertex, TEdge> EdgeFactory { get; private set; }

        public Dictionary<TEdge, TEdge> ReversedEdges { get; protected set; }

        public Dictionary<TVertex, GraphColor> VertexColors { get; }

        public GraphColor GetVertexColor(TVertex vertex)
        {
            return VertexColors[vertex];
        }

        public TVertex Source
        {
            get { return _source; }
            set
            {
                Contract.Requires(value != null);
                _source = value;
            }
        }

        public TVertex Sink
        {
            get { return _sink; }
            set
            {
                Contract.Requires(value != null);
                _sink = value;
            }
        }

        public double MaxFlow { get; set; }

        #endregion
    }
}