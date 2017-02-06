namespace QuickGraph
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>An event involving an edge.</summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public class EdgeEventArgs<TVertex, TEdge>
        : EventArgs
        where TEdge : IEdge<TVertex>
    {
        /// <summary>Gets the edge.</summary>
        /// <value>The edge.</value>
        public TEdge Edge { get; }

        /// <summary>Initializes a new instance of the <see cref="EdgeEventArgs&lt;TVertex, TEdge&gt;" /> class.</summary>
        /// <param name="edge">The edge.</param>
        public EdgeEventArgs(TEdge edge)
        {
            Contract.Requires(edge != null);

            Edge = edge;
        }
    }

    /// <summary>The handler for events involving edges</summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    /// <param name="e"></param>
    public delegate void EdgeAction<TVertex, in TEdge>(TEdge e)
        where TEdge : IEdge<TVertex>;
}