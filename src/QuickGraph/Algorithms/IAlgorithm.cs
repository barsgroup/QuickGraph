namespace QuickGraph.Algorithms
{
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Contracts;

    [ContractClass(typeof(AlgorithmContract<>))]
    public interface IAlgorithm<TGraph> :
        IComputation
    {
        TGraph VisitedGraph { get; }
    }
}