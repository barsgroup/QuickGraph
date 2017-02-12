namespace QuickGraph.Graphviz.Dot
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    [DebuggerDisplay("{Width}x{Height}")]
    public struct GraphvizSizeF
    {
        public float Height { get; }

        public float Width { get; }

        public GraphvizSizeF(float width, float height)
        {
            Contract.Requires(width >= 0);
            Contract.Requires(height >= 0);

            Width = width;
            Height = height;
        }

        public bool IsEmpty => Width == 0 || Height == 0;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}x{1}", Width, Height);
        }
    }

    [DebuggerDisplay("{Width}x{Height}")]
    public struct GraphvizSize
    {
        public int Height { get; }

        public int Width { get; }

        public GraphvizSize(int width, int height)
        {
            Contract.Requires(width >= 0);
            Contract.Requires(height >= 0);

            Width = width;
            Height = height;
        }

        public bool IsEmpty
        {
            get { return Width == 0 || Height == 0; }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}x{1}", Width, Height);
        }
    }
}