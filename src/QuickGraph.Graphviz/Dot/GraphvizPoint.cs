namespace QuickGraph.Graphviz.Dot
{
    public sealed class GraphvizPoint
    {
        public int X { get; }

        public int Y { get; }

        public GraphvizPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}