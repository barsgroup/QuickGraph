namespace QuickGraph.Algorithms
{
    public static class DistanceRelaxers
    {
        public static readonly IDistanceRelaxer ShortestDistance = new ShortestDistanceRelaxer();

        public static readonly IDistanceRelaxer CriticalDistance = new CriticalDistanceRelaxer();

        public static readonly IDistanceRelaxer EdgeShortestDistance = new EdgeDistanceRelaxer();

        private sealed class CriticalDistanceRelaxer :
            IDistanceRelaxer
        {
            public double InitialDistance => double.MinValue;

            public double Combine(double distance, double weight)
            {
                return distance + weight;
            }

            public int Compare(double a, double b)
            {
                return -a.CompareTo(b);
            }
        }

        private sealed class EdgeDistanceRelaxer
            : IDistanceRelaxer
        {
            public double InitialDistance => 0;

            public double Combine(double distance, double weight)
            {
                return distance + weight;
            }

            public int Compare(double a, double b)
            {
                return a.CompareTo(b);
            }
        }

        private sealed class ShortestDistanceRelaxer
            : IDistanceRelaxer
        {
            public double InitialDistance => double.MaxValue;

            public double Combine(double distance, double weight)
            {
                return distance + weight;
            }

            public int Compare(double a, double b)
            {
                return a.CompareTo(b);
            }
        }
    }
}