namespace QuickGraph
{
    using System;
    using System.Diagnostics.Contracts;

    public abstract class VertexEventArgs<TVertex> : EventArgs
    {
        public TVertex Vertex { get; }

        protected VertexEventArgs(TVertex vertex)
        {
            Contract.Requires(vertex != null);
            Vertex = vertex;
        }
    }

    public delegate void VertexAction<TVertex>(TVertex vertex);
}