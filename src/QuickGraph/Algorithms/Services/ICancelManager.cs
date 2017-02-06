namespace QuickGraph.Algorithms.Services
{
    using System;
    using System.Threading;

    public interface ICancelManager :
        IService
    {
        /// <summary>Gets a value indicating if a cancellation request is pending.</summary>
        /// <returns></returns>
        bool IsCancelling { get; }

        /// <summary>Requests the component to cancel its computation</summary>
        void Cancel();

        /// <summary>Resets the cancel state</summary>
        void ResetCancel();

        /// <summary>Raised when the cancel method is called</summary>
        event EventHandler CancelRequested;

        /// <summary>Raised when the cancel state has been reseted</summary>
        event EventHandler CancelReseted;
    }

    internal class CancelManager :
        ICancelManager
    {
        private int _cancelling;

        public bool IsCancelling => _cancelling > 0;

        public void Cancel()
        {
            var value = Interlocked.Increment(ref _cancelling);
            if (value == 0)
            {
                var eh = CancelRequested;
                if (eh != null)
                {
                    eh(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>Resets the cancel state</summary>
        public void ResetCancel()
        {
            var value = Interlocked.Exchange(ref _cancelling, 0);
            if (value != 0)
            {
                var eh = CancelReseted;
                if (eh != null)
                {
                    eh(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler CancelRequested;

        /// <summary>Raised when the cancel state has been reseted</summary>
        public event EventHandler CancelReseted;
    }
}