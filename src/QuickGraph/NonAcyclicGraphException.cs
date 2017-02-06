namespace QuickGraph
{
    using System;

    public class NonAcyclicGraphException
        : QuickGraphException
    {
        public NonAcyclicGraphException()
        {
        }

        public NonAcyclicGraphException(string message) : base(message)
        {
        }

        public NonAcyclicGraphException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}