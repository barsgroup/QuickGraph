namespace QuickGraph.Graphviz.Dot
{
    using System.Collections.Generic;
    using System.IO;

    public class GraphvizVertex
    {
        public GraphvizPoint Position { get; set; }

        public string BottomLabel { get; set; } = null;

        public string Comment { get; set; } = null;

        public double Distorsion { get; set; } = 0;

        public GraphvizColor FillColor { get; set; } = GraphvizColor.White;

        public bool FixedSize { get; set; } = false;

        public GraphvizFont Font { get; set; } = null;

        public GraphvizColor FontColor { get; set; } = GraphvizColor.Black;

        public string Group { get; set; } = null;

        public string Label { get; set; } = null;

        public GraphvizLayer Layer { get; set; } = null;

        public double Orientation { get; set; } = 0;

        public int Peripheries { get; set; } = -1;

        public GraphvizRecord Record { get; set; } = new GraphvizRecord();

        public bool Regular { get; set; } = false;

        public GraphvizVertexShape Shape { get; set; } = GraphvizVertexShape.Unspecified;

        public int Sides { get; set; } = 4;

        public GraphvizSizeF Size { get; set; } = new GraphvizSizeF(0f, 0f);

        public double Skew { get; set; } = 0;

        public GraphvizColor StrokeColor { get; set; } = GraphvizColor.Black;

        public GraphvizVertexStyle Style { get; set; } = GraphvizVertexStyle.Unspecified;

        public string ToolTip { get; set; } = null;

        public string TopLabel { get; set; } = null;

        public string Url { get; set; } = null;

        public double Z { get; set; } = -1;

        public string ToDot()
        {
            var pairs = new Dictionary<string, object>();
            if (Font != null)
            {
                pairs["fontname"] = Font.Name;
                pairs["fontsize"] = Font.SizeInPoints;
            }
            if (!FontColor.Equals(GraphvizColor.Black))
            {
                pairs["fontcolor"] = FontColor;
            }
            if (Shape != GraphvizVertexShape.Unspecified)
            {
                pairs["shape"] = Shape;
            }
            if (Style != GraphvizVertexStyle.Unspecified)
            {
                pairs["style"] = Style;
            }
            if (Shape == GraphvizVertexShape.Record)
            {
                pairs["label"] = Record;
            }
            else if (Label != null)
            {
                pairs["label"] = Label;
            }
            if (FixedSize)
            {
                pairs["fixedsize"] = true;
                if (Size.Height > 0f)
                {
                    pairs["height"] = Size.Height;
                }
                if (Size.Width > 0f)
                {
                    pairs["width"] = Size.Width;
                }
            }
            if (!StrokeColor.Equals(GraphvizColor.Black))
            {
                pairs["color"] = StrokeColor;
            }
            if (!FillColor.Equals(GraphvizColor.White))
            {
                pairs["fillcolor"] = FillColor;
            }
            if (Regular)
            {
                pairs["regular"] = Regular;
            }
            if (Url != null)
            {
                pairs["URL"] = Url;
            }
            if (ToolTip != null)
            {
                pairs["tooltip"] = ToolTip;
            }
            if (Comment != null)
            {
                pairs["comment"] = Comment;
            }
            if (Group != null)
            {
                pairs["group"] = Group;
            }
            if (Layer != null)
            {
                pairs["layer"] = Layer.Name;
            }
            if (Orientation > 0)
            {
                pairs["orientation"] = Orientation;
            }
            if (Peripheries >= 0)
            {
                pairs["peripheries"] = Peripheries;
            }
            if (Z > 0)
            {
                pairs["z"] = Z;
            }
            if (Position != null)
            {
                pairs["pos"] = string.Format("{0},{1}!", Position.X, Position.Y);
            }
            if (Style == GraphvizVertexStyle.Diagonals || Shape == GraphvizVertexShape.MCircle || Shape == GraphvizVertexShape.MDiamond || Shape == GraphvizVertexShape.MSquare)
            {
                if (TopLabel != null)
                {
                    pairs["toplabel"] = TopLabel;
                }
                if (BottomLabel != null)
                {
                    pairs["bottomlable"] = BottomLabel;
                }
            }
            if (Shape == GraphvizVertexShape.Polygon)
            {
                if (Sides != 0)
                {
                    pairs["sides"] = Sides;
                }
                if (Skew != 0)
                {
                    pairs["skew"] = Skew;
                }
                if (Distorsion != 0)
                {
                    pairs["distorsion"] = Distorsion;
                }
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
                if (entry.Value is GraphvizVertexShape)
                {
                    writer.Write("{0}={1}", entry.Key, ((GraphvizVertexShape)entry.Value).ToString().ToLower());
                    continue;
                }
                if (entry.Value is GraphvizVertexStyle)
                {
                    writer.Write("{0}={1}", entry.Key, ((GraphvizVertexStyle)entry.Value).ToString().ToLower());
                    continue;
                }
                if (entry.Value is GraphvizColor)
                {
                    var color = (GraphvizColor)entry.Value;
                    writer.Write(
                        "{0}=\"#{1}{2}{3}{4}\"",
                        entry.Key,
                        color.R.ToString("x2").ToUpper(),
                        color.G.ToString("x2").ToUpper(),
                        color.B.ToString("x2").ToUpper(),
                        color.A.ToString("x2").ToUpper());
                    continue;
                }
                var value = entry.Value as GraphvizRecord;
                if (value != null)
                {
                    writer.WriteLine("{0}=\"{1}\"", entry.Key, value.ToDot());
                    continue;
                }
                writer.Write(" {0}={1}", entry.Key, entry.Value.ToString().ToLower());
            }
            return writer.ToString();
        }
    }
}