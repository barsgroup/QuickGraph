namespace QuickGraph.Graphviz.Dot
{
    using System;

    public struct GraphvizColor
        : IEquatable<GraphvizColor>
    {
        public GraphvizColor(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public byte A { get; }

        public byte R { get; }

        public byte G { get; }

        public byte B { get; }

        public static GraphvizColor Black => new GraphvizColor(0xFF, 0, 0, 0);

        public static GraphvizColor White => new GraphvizColor(0xFF, 0xFF, 0xFF, 0xFF);

        public static GraphvizColor LightYellow => new GraphvizColor(0xFF, 0xFF, 0xFF, 0xE0);

        public bool Equals(GraphvizColor other)
        {
            return A == other.A && R == other.R && G == other.G && B == other.B;
        }

        public override int GetHashCode()
        {
            return (A << 24) | (R << 16) | (G << 8) | B;
        }
    }
}