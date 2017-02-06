using System;

namespace QuickGraph
{
    public class NegativeWeightException 
        : QuickGraphException
    {
        public NegativeWeightException() { }
        public NegativeWeightException(string message) : base(message) { }
        public NegativeWeightException(string message, Exception inner) : base(message, inner) { }
    }
}
