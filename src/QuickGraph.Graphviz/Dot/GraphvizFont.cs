namespace QuickGraph.Graphviz.Dot
{
    using System.Diagnostics.Contracts;

    public sealed class GraphvizFont
    {
        public string Name { get; }

        public float SizeInPoints { get; }

        public GraphvizFont(string name, float sizeInPoints)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(sizeInPoints > 0);

            Name = name;
            SizeInPoints = sizeInPoints;
        }
    }
}