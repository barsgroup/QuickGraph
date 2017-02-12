namespace QuickGraph.Graphviz.Dot
{
    using System.Collections.Generic;

    public class GraphvizEdgeLabel
    {
        public double Angle { get; set; } = -25;

        public double Distance { get; set; } = 1;

        public bool Float { get; set; } = true;

        public GraphvizFont Font { get; set; } = null;

        public GraphvizColor FontColor { get; set; } = GraphvizColor.Black;

        public string Value { get; set; } = null;

        public void AddParameters(IDictionary<string, object> dic)
        {
            if (Value != null)
            {
                dic["label"] = Value;
                if (Angle != -25)
                {
                    dic["labelangle"] = Angle;
                }
                if (Distance != 1)
                {
                    dic["labeldistance"] = Distance;
                }
                if (!Float)
                {
                    dic["labelfloat"] = Float;
                }
                if (Font != null)
                {
                    dic["labelfontname"] = Font.Name;
                    dic["labelfontsize"] = Font.SizeInPoints;
                }
            }
        }
    }
}