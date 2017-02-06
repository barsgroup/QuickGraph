namespace QuickGraph
{
    using System;

    /// <summary>Exception raised when an algorithm detects a non-strongly connected graph.</summary>
    public class NonStronglyConnectedGraphException
        : QuickGraphException
    {
        public NonStronglyConnectedGraphException()
        {
        }

        public NonStronglyConnectedGraphException(string message) : base(message)
        {
        }

        public NonStronglyConnectedGraphException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}