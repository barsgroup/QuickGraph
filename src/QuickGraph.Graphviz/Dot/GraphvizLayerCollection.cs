namespace QuickGraph.Graphviz.Dot
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.IO;

    public class GraphvizLayerCollection : Collection<GraphvizLayer>
    {
        private string _mSeparators = ":";

        public string Separators
        {
            get { return _mSeparators; }
            set
            {
                Contract.Requires(!string.IsNullOrEmpty(value));

                _mSeparators = value;
            }
        }

        public GraphvizLayerCollection()
        {
        }

        public GraphvizLayerCollection(GraphvizLayer[] items)
            : base(items)
        {
        }

        public GraphvizLayerCollection(GraphvizLayerCollection items)
            : base(items)
        {
        }

        public string ToDot()
        {
            if (Count == 0)
            {
                return null;
            }
            using (var writer = new StringWriter())
            {
                writer.Write("layers=\"");
                var flag = false;
                foreach (var layer in this)
                {
                    if (flag)
                    {
                        writer.Write(Separators);
                    }
                    else
                    {
                        flag = true;
                    }
                    writer.Write(layer.Name);
                }
                writer.WriteLine("\";");
                writer.WriteLine("layersep=\"{0}\"", Separators);
                return writer.ToString();
            }
        }
    }
}