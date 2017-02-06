namespace QuickGraph.Algorithms.Observers.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(Observers.IObserver<>))]
    internal abstract class ObserverContract<TAlgorithm>
        : Observers.IObserver<TAlgorithm>
    {
        IDisposable Observers.IObserver<TAlgorithm>.Attach(TAlgorithm algorithm)
        {
            Contract.Requires(algorithm != null);
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return default(IDisposable);
        }
    }
}