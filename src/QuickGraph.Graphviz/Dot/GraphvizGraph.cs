namespace QuickGraph.Graphviz.Dot
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class GraphvizGraph
    {
        public string Name { get; set; } = "G";

        public GraphvizColor BackgroundGraphvizColor { get; set; } = GraphvizColor.White;

        public GraphvizClusterMode ClusterRank { get; set; } = GraphvizClusterMode.Local;

        public string Comment { get; set; } = null;

        public GraphvizFont Font { get; set; } = null;

        public GraphvizColor FontGraphvizColor { get; set; } = GraphvizColor.Black;

        public bool IsCentered { get; set; } = false;

        public bool IsCompounded { get; set; } = false;

        public bool IsConcentrated { get; set; } = false;

        public bool IsLandscape { get; set; } = false;

        public bool IsNormalized { get; set; } = false;

        public bool IsReMinCross { get; set; } = false;

        public string Label { get; set; } = null;

        public GraphvizLabelJustification LabelJustification { get; set; } = GraphvizLabelJustification.C;

        public GraphvizLabelLocation LabelLocation { get; set; } = GraphvizLabelLocation.B;

        public GraphvizLayerCollection Layers { get; } = new GraphvizLayerCollection();

        public double McLimit { get; set; } = 1;

        public double NodeSeparation { get; set; } = 0.25;

        public int NsLimit { get; set; } = -1;

        public int NsLimit1 { get; set; } = -1;

        public GraphvizOutputMode OutputOrder { get; set; } = GraphvizOutputMode.BreadthFirst;

        public GraphvizPageDirection PageDirection { get; set; } = GraphvizPageDirection.Bl;

        public GraphvizSizeF PageSize { get; set; } = new GraphvizSizeF(0, 0);

        public double Quantum { get; set; } = 0;

        public GraphvizRankDirection RankDirection { get; set; } = GraphvizRankDirection.Tb;

        public double RankSeparation { get; set; } = 0.5;

        public GraphvizRatioMode Ratio { get; set; } = GraphvizRatioMode.Auto;

        public double Resolution { get; set; } = 0.96;

        public int Rotate { get; set; } = 0;

        public int SamplePoints { get; set; } = 8;

        public int SearchSize { get; set; } = 30;

        public GraphvizSizeF Size { get; set; } = new GraphvizSizeF(0, 0);

        public string StyleSheet { get; set; } = null;

        public string Url { get; set; } = null;

        public string ToDot()
        {
            var pairs = new Dictionary<string, object>(StringComparer.Ordinal);
            if (Url != null)
            {
                pairs["URL"] = Url;
            }
            if (!BackgroundGraphvizColor.Equals(GraphvizColor.White))
            {
                pairs["bgGraphvizColor"] = BackgroundGraphvizColor;
            }
            if (IsCentered)
            {
                pairs["center"] = true;
            }
            if (ClusterRank != GraphvizClusterMode.Local)
            {
                pairs["clusterrank"] = ClusterRank.ToString().ToLower();
            }
            if (Comment != null)
            {
                pairs["comment"] = Comment;
            }
            if (IsCompounded)
            {
                pairs["compound"] = IsCompounded;
            }
            if (IsConcentrated)
            {
                pairs["concentrated"] = IsConcentrated;
            }
            if (Font != null)
            {
                pairs["fontname"] = Font.Name;
                pairs["fontsize"] = Font.SizeInPoints;
            }
            if (!FontGraphvizColor.Equals(GraphvizColor.Black))
            {
                pairs["fontGraphvizColor"] = FontGraphvizColor;
            }
            if (Label != null)
            {
                pairs["label"] = Label;
            }
            if (LabelJustification != GraphvizLabelJustification.C)
            {
                pairs["labeljust"] = LabelJustification.ToString().ToLower();
            }
            if (LabelLocation != GraphvizLabelLocation.B)
            {
                pairs["labelloc"] = LabelLocation.ToString().ToLower();
            }
            if (Layers.Count != 0)
            {
                pairs["layers"] = Layers.ToDot();
            }
            if (McLimit != 1)
            {
                pairs["mclimit"] = McLimit;
            }
            if (NodeSeparation != 0.25)
            {
                pairs["nodesep"] = NodeSeparation;
            }
            if (IsNormalized)
            {
                pairs["normalize"] = IsNormalized;
            }
            if (NsLimit > 0)
            {
                pairs["nslimit"] = NsLimit;
            }
            if (NsLimit1 > 0)
            {
                pairs["nslimit1"] = NsLimit1;
            }
            if (OutputOrder != GraphvizOutputMode.BreadthFirst)
            {
                pairs["outputorder"] = OutputOrder.ToString().ToLower();
            }
            if (!PageSize.IsEmpty)
            {
                pairs["page"] = string.Format("{0},{1}", PageSize.Width, PageSize.Height);
            }
            if (PageDirection != GraphvizPageDirection.Bl)
            {
                pairs["pagedir"] = PageDirection.ToString().ToLower();
            }
            if (Quantum > 0)
            {
                pairs["quantum"] = Quantum;
            }
            if (RankSeparation != 0.5)
            {
                pairs["ranksep"] = RankSeparation;
            }
            if (Ratio != GraphvizRatioMode.Auto)
            {
                pairs["ratio"] = Ratio.ToString().ToLower();
            }
            if (IsReMinCross)
            {
                pairs["remincross"] = IsReMinCross;
            }
            if (Resolution != 0.96)
            {
                pairs["resolution"] = Resolution;
            }
            if (Rotate != 0)
            {
                pairs["rotate"] = Rotate;
            }
            else if (IsLandscape)
            {
                pairs["orientation"] = "[1L]*";
            }
            if (SamplePoints != 8)
            {
                pairs["samplepoints"] = SamplePoints;
            }
            if (SearchSize != 30)
            {
                pairs["searchsize"] = SearchSize;
            }
            if (!Size.IsEmpty)
            {
                pairs["size"] = string.Format("{0},{1}", Size.Width, Size.Height);
            }
            if (StyleSheet != null)
            {
                pairs["stylesheet"] = StyleSheet;
            }
            if (RankDirection != GraphvizRankDirection.Tb)
            {
                pairs["rankdir"] = RankDirection;
            }
            return GenerateDot(pairs);
        }

        public override string ToString()
        {
            return ToDot();
        }

        internal string GenerateDot(Dictionary<string, object> pairs)
        {
            var flag = false;
            var writer = new StringWriter();
            foreach (var entry in pairs)
            {
                if (flag)
                {
                    writer.Write(", ");
                }
                else
                {
                    flag = true;
                }
                if (entry.Value is string)
                {
                    writer.Write("{0}=\"{1}\"", entry.Key, entry.Value.ToString());
                    continue;
                }
                if (entry.Value is GraphvizColor)
                {
                    var graphvizColor = (GraphvizColor)entry.Value;
                    writer.Write(
                        "{0}=\"#{1}{2}{3}{4}\"",
                        entry.Key,
                        graphvizColor.R.ToString("x2").ToUpper(),
                        graphvizColor.G.ToString("x2").ToUpper(),
                        graphvizColor.B.ToString("x2").ToUpper(),
                        graphvizColor.A.ToString("x2").ToUpper());
                    continue;
                }
                if (entry.Value is GraphvizRankDirection || entry.Value is GraphvizPageDirection)
                {
                    writer.Write("{0}={1};", entry.Key, entry.Value.ToString());
                    continue;
                }
                writer.Write(" {0}={1}", entry.Key, entry.Value.ToString().ToLower());
            }
            return writer.ToString();
        }
    }
}