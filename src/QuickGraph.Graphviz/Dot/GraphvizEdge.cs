namespace QuickGraph.Graphviz.Dot
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class GraphvizEdge
    {
        public string Comment { get; set; } = null;

        public GraphvizEdgeDirection Dir { get; set; } = GraphvizEdgeDirection.Forward;

        public GraphvizFont Font { get; set; }

        public GraphvizColor FontGraphvizColor { get; set; } = GraphvizColor.Black;

        public GraphvizEdgeExtremity Head { get; set; } = new GraphvizEdgeExtremity(true);

        public GraphvizArrow HeadArrow { get; set; } = null;

        public bool IsConstrained { get; set; } = true;

        public bool IsDecorated { get; set; } = false;

        public GraphvizEdgeLabel Label { get; set; } = new GraphvizEdgeLabel();

        public GraphvizLayer Layer { get; set; } = null;

        public int MinLength { get; set; } = 1;

        public GraphvizColor StrokeGraphvizColor { get; set; } = GraphvizColor.Black;

        public GraphvizEdgeStyle Style { get; set; } = GraphvizEdgeStyle.Unspecified;

        public GraphvizEdgeExtremity Tail { get; set; } = new GraphvizEdgeExtremity(false);

        public GraphvizArrow TailArrow { get; set; } = null;

        public string ToolTip { get; set; } = null;

        public string Url { get; set; } = null;

        public double Weight { get; set; } = 1;

        public string HeadPort { get; set; }

        public string TailPort { get; set; }

        public int Length { get; set; } = 1;

        public string ToDot()
        {
            var dic = new Dictionary<string, object>(StringComparer.Ordinal);
            if (Comment != null)
            {
                dic["comment"] = Comment;
            }
            if (Dir != GraphvizEdgeDirection.Forward)
            {
                dic["dir"] = Dir.ToString().ToLower();
            }
            if (Font != null)
            {
                dic["fontname"] = Font.Name;
                dic["fontsize"] = Font.SizeInPoints;
            }
            if (!FontGraphvizColor.Equals(GraphvizColor.Black))
            {
                dic["fontGraphvizColor"] = FontGraphvizColor;
            }
            Head.AddParameters(dic);
            if (HeadArrow != null)
            {
                dic["arrowhead"] = HeadArrow.ToDot();
            }
            if (!IsConstrained)
            {
                dic["constraint"] = IsConstrained;
            }
            if (IsDecorated)
            {
                dic["decorate"] = IsDecorated;
            }
            Label.AddParameters(dic);
            if (Layer != null)
            {
                dic["layer"] = Layer.Name;
            }
            if (MinLength != 1)
            {
                dic["minlen"] = MinLength;
            }
            if (!StrokeGraphvizColor.Equals(GraphvizColor.Black))
            {
                dic["GraphvizColor"] = StrokeGraphvizColor;
            }
            if (Style != GraphvizEdgeStyle.Unspecified)
            {
                dic["style"] = Style.ToString().ToLower();
            }
            Tail.AddParameters(dic);
            if (TailArrow != null)
            {
                dic["arrowtail"] = TailArrow.ToDot();
            }
            if (ToolTip != null)
            {
                dic["tooltip"] = ToolTip;
            }
            if (Url != null)
            {
                dic["URL"] = Url;
            }
            if (Weight != 1)
            {
                dic["weight"] = Weight;
            }
            if (HeadPort != null)
            {
                dic["headport"] = HeadPort;
            }
            if (TailPort != null)
            {
                dic["tailport"] = TailPort;
            }
            if (Length != 1)
            {
                dic["len"] = Length;
            }
            return GenerateDot(dic);
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
                if (entry.Value is GraphvizEdgeDirection)
                {
                    writer.Write("{0}={1}", entry.Key, ((GraphvizEdgeDirection)entry.Value).ToString().ToLower());
                    continue;
                }
                if (entry.Value is GraphvizEdgeStyle)
                {
                    writer.Write("{0}={1}", entry.Key, ((GraphvizEdgeStyle)entry.Value).ToString().ToLower());
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
                writer.Write(" {0}={1}", entry.Key, entry.Value.ToString().ToLower());
            }
            return writer.ToString();
        }
    }
}