namespace QuickGraph.Graphviz
{
    using System.Diagnostics.Contracts;

    using QuickGraph.Graphviz.Dot;

    [ContractClass(typeof(DotEngineContract))]
    public interface IDotEngine
    {
        string Run(
            GraphvizImageType imageType,
            string dot,
            string outputFileName);
    }

    [ContractClassFor(typeof(IDotEngine))]
    internal abstract class DotEngineContract
        : IDotEngine
    {
        #region IDotEngine Members

        string IDotEngine.Run(GraphvizImageType imageType, string dot, string outputFileName)
        {
            Contract.Requires(!string.IsNullOrEmpty(dot));
            Contract.Requires(!string.IsNullOrEmpty(outputFileName));
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            return null;
        }

        #endregion
    }
}