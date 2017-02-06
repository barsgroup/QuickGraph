namespace QuickGraph
{
    using System;

    public class NegativeCycleGraphException
        : QuickGraphException
    {
        public NegativeCycleGraphException()
        {
        }

        public NegativeCycleGraphException(string message) : base(message)
        {
        }

        public NegativeCycleGraphException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}