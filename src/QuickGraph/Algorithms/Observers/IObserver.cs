namespace QuickGraph.Algorithms.Observers
{
    using System;
    using System.Diagnostics.Contracts;

    using QuickGraph.Algorithms.Observers.Contracts;

    /// <summary>An algorithm observer</summary>
    /// <typeparam name="TAlgorithm">type of the algorithm</typeparam>
    /// <reference-ref
    ///     id="gof02designpatterns" />
    [ContractClass(typeof(ObserverContract<>))]
    public interface IObserver<TAlgorithm>
    {
        /// <summary>Attaches to the algorithm events and returns a disposable object that can be used to detach from the events</summary>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        IDisposable Attach(TAlgorithm algorithm);
    }
}