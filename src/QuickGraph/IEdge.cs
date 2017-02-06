namespace QuickGraph
{
    using System.Diagnostics.Contracts;

    using QuickGraph.Contracts;

    /// <summary>A directed edge</summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    [ContractClass(typeof(EdgeContract<>))]
    public interface IEdge<TVertex>
    {
        /// <summary>Gets the source vertex</summary>
        TVertex Source { get; }

        /// <summary>Gets the target vertex</summary>
        TVertex Target { get; }
    }
}