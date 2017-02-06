namespace QuickGraph
{
    using System;

    public class ParallelEdgeNotAllowedException
        : QuickGraphException
    {
        public ParallelEdgeNotAllowedException()
        {
        }

        public ParallelEdgeNotAllowedException(string message) : base(message)
        {
        }

        public ParallelEdgeNotAllowedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}