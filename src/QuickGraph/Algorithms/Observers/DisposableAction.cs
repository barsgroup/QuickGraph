namespace QuickGraph.Algorithms.Observers
{
    using System;
    using System.Diagnostics.Contracts;

    internal struct DisposableAction
        : IDisposable
    {
        public delegate void Action();

        private Action _action;

        public DisposableAction(Action action)
        {
            Contract.Requires(action != null);
            _action = action;
        }

        public void Dispose()
        {
            var a = _action;
            _action = null;
            if (a != null)
            {
                a();
            }
        }
    }
}