namespace QuickGraph.Algorithms
{
    using System.Collections.Generic;

    public interface IDistanceRelaxer
        : IComparer<double>
    {
        double InitialDistance { get; }

        double Combine(double distance, double weight);
    }
}