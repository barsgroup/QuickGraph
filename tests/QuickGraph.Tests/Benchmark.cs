namespace QuickGraph.Tests
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;

    public static class PreciseTimer
    {
        private static readonly long frequency;

        /// <summary>Gets the frequency.</summary>
        /// <value>The frequency.</value>
        public static long Frequency
        {
            get { return frequency; }
        }

        /// <summary>Gets the current ticks value.</summary>
        /// <value>The now.</value>
        public static long Now
        {
            get
            {
                long startTime;
                if (!Win32.QueryPerformanceCounter(out startTime))
                {
                    throw new Win32Exception("QueryPerformanceCounter failed");
                }
                return startTime;
            }
        }

        static PreciseTimer()
        {
            if (!Win32.QueryPerformanceFrequency(out frequency))
            {
                // high-performance counter not supported
                throw new Win32Exception();
            }
        }

        /// <summary>Returns the duration of the timer (in seconds)</summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [Pure]
        public static double ToSeconds(long start, long end)
        {
            Contract.Requires(start >= 0 && end >= 0 && start <= end);

            return (end - start) / (double)frequency;
        }

        /// <summary>Returns the duration in seconds</summary>
        /// <param name="ticks">The ticks.</param>
        /// <returns></returns>
        [Pure]
        public static double ToSeconds(long ticks)
        {
            Contract.Requires(ticks >= 0);
            return ticks / (double)frequency;
        }

        /// <summary>Returns the duration in seconds from <paramref name="start" />
        /// </summary>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static double ToSecondsFromNow(long start)
        {
            return ToSeconds(start, Now);
        }

        private sealed class Win32
        {
            [DllImport("Kernel32.dll")]
            public static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

            [DllImport("Kernel32.dll")]
            public static extern bool QueryPerformanceFrequency(out long lpFrequency);
        }
    }

    public sealed class Benchmark
    {
        private long _duration;

        public string Name { get; }

        public double Seconds
        {
            get { return PreciseTimer.ToSeconds(_duration); }
        }

        public long Samples { get; private set; }

        public Benchmark(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Name = name;
        }

        public void Run(Action action)
        {
            var start = PreciseTimer.Now;
            try
            {
                action();
            }
            finally
            {
                var finish = PreciseTimer.Now;
                _duration += finish - start;
                Samples++;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}s, {2} samples", Name, Seconds, Samples);
        }
    }
}