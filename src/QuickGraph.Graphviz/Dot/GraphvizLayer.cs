namespace QuickGraph.Graphviz.Dot
{
    using System.Diagnostics.Contracts;

    public class GraphvizLayer
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                Contract.Requires(!string.IsNullOrEmpty(value));
                _name = value;
            }
        }

        public GraphvizLayer(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            _name = name;
        }
    }
}