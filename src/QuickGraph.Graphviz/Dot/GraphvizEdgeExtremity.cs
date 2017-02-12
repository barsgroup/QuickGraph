namespace QuickGraph.Graphviz.Dot
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class GraphvizEdgeExtremity
    {
        public bool IsClipped { get; set; }

        public bool IsHead { get; }

        public string Label { get; set; }

        public string Logical { get; set; }

        public string Same { get; set; }

        public string ToolTip { get; set; }

        public string Url { get; set; }

        public GraphvizEdgeExtremity(bool isHead)
        {
            IsHead = isHead;
            Url = null;
            IsClipped = true;
            Label = null;
            ToolTip = null;
            Logical = null;
            Same = null;
        }

        public void AddParameters(IDictionary<string, object> dic)
        {
            Contract.Requires(dic != null);

            var text = IsHead
                           ? "head"
                           : "tail";

            if (Url != null)
            {
                dic.Add(text + "URL", Url);
            }
            if (!IsClipped)
            {
                dic.Add(text + "clip", IsClipped);
            }
            if (Label != null)
            {
                dic.Add(text + "label", Label);
            }
            if (ToolTip != null)
            {
                dic.Add(text + "tooltip", ToolTip);
            }
            if (Logical != null)
            {
                dic.Add("l" + text, Logical);
            }
            if (Same != null)
            {
                dic.Add("same" + text, Same);
            }
        }
    }
}