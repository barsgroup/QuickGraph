namespace QuickGraph.Tests
{
    using System.Collections.Generic;

    using Xunit;

    public class DataStructureTest
    {
        [Fact]
        public void DisplayLinkedList()
        {
            var target = new LinkedList<int>();
            target.AddFirst(0);
            target.AddFirst(1);
        }
    }
}