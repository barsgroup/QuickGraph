namespace QuickGraph.Graphviz.Dot
{
    using System.Text;

    public class GraphvizRecord
    {
        public GraphvizRecordCellCollection Cells { get; } = new GraphvizRecordCellCollection();

        public string ToDot()
        {
            if (Cells.Count == 0)
            {
                return string.Empty;
            }
            var builder = new StringBuilder();
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
            return builder.ToString();
        }

        public override string ToString()
        {
            return ToDot();
        }
    }
}