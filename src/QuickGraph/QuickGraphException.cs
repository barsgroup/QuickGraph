namespace QuickGraph
{
    using System;

    public abstract class QuickGraphException
        : Exception
    {
        protected QuickGraphException()
        {
        }

        protected QuickGraphException(string message) : base(message)
        {
        }

        protected QuickGraphException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}