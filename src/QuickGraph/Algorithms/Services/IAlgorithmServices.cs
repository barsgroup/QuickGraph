namespace QuickGraph.Algorithms.Services
{
    using System.Diagnostics.Contracts;

    /// <summary>Common services available to algorithm instances</summary>
    public interface IAlgorithmServices
    {
        ICancelManager CancelManager { get; }
    }

    internal class AlgorithmServices :
        IAlgorithmServices
    {
        private readonly IAlgorithmComponent _host;

        private ICancelManager _cancelManager;

        public ICancelManager CancelManager
        {
            get
            {
                if (_cancelManager == null)
                {
                    _cancelManager = _host.GetService<ICancelManager>();
                }
                return _cancelManager;
            }
        }

        public AlgorithmServices(IAlgorithmComponent host)
        {
            Contract.Requires(host != null);

            _host = host;
        }
    }
}