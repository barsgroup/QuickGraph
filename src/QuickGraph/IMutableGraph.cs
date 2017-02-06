namespace QuickGraph
{
    using System;
    using System.Diagnostics.Contracts;

    using QuickGraph.Contracts;

    /// <summary>A mutable graph instance</summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [ContractClass(typeof(MutableGraphContract<,>))]
    public interface IMutableGraph<TVertex, TEdge>
        : IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>Clears the vertex and edges</summary>
        void Clear();

        /// <summary>Called when the graph vertices and edges have been cleared.</summary>
        event EventHandler Cleared;
    }
}