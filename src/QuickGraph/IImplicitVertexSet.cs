namespace QuickGraph
{
    using System.Diagnostics.Contracts;

    using QuickGraph.Contracts;

    /// <summary>An implicit set of vertices</summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    [ContractClass(typeof(ImplicitVertexSetContract<>))]
    public interface IImplicitVertexSet<TVertex>
    {
        /// <summary>Determines whether the specified vertex contains vertex.</summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        ///     <c>true</c> if the specified vertex contains vertex; otherwise, <c>false</c>.
        /// </returns>
        [Pure]
        bool ContainsVertex(TVertex vertex);
    }
}