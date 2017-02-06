namespace QuickGraph.Algorithms
{
    using System;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Contracts;

    [ContractClass(typeof(ComputationContract))]
    public interface IComputation
    {
        object SyncRoot { get; }

        ComputationState State { get; }

        void Abort();

        void Compute();

        event EventHandler Aborted;

        event EventHandler Finished;

        event EventHandler Started;

        event EventHandler StateChanged;
    }
}