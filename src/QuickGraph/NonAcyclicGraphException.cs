using System;

namespace QuickGraph
{
    public class NonAcyclicGraphException
        : QuickGraphException
    {
        public NonAcyclicGraphException() { }
        public NonAcyclicGraphException(string message) : base( message ) { }
        public NonAcyclicGraphException(string message, System.Exception inner) : base( message, inner ) { }
    }
}


