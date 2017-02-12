namespace QuickGraph.Graphviz.Dot
{
    using System.Text;

    public class GraphvizRecordCell
    {
        public GraphvizRecordCellCollection Cells { get; } = new GraphvizRecordCellCollection();

        public bool HasPort => Port?.Length > 0;

        public bool HasText => Text?.Length > 0;

        public string Port { get; set; } = null;

        public string Text { get; set; } = null;

        protected GraphvizRecordEscaper Escaper { get; } = new GraphvizRecordEscaper();

        public string ToDot()
        {
            var builder = new StringBuilder();
            if (HasPort)
            {
                builder.AppendFormat("<{0}> ", Escaper.Escape(Port));
            }
            if (HasText)
            {
                builder.AppendFormat("{0}", Escaper.Escape(Text));
            }
            if (Cells.Count > 0)
            {
                builder.Append(" { ");
                var flag = false;
                foreach (var cell in Cells)
                {
                    if (flag)
                    {
                        builder.AppendFormat(" | {0}", cell.ToDot());
                        continue;
                    }
                    builder.Append(cell.ToDot());
                    flag = true;
                }
                builder.Append(" } ");
            }
            return builder.ToString();
        }

        public override string ToString()
        {
            return ToDot();
        }
    }
}