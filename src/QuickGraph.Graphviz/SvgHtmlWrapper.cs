namespace QuickGraph.Graphviz
{
    using System.IO;
    using System.Text.RegularExpressions;

    using QuickGraph.Graphviz.Dot;

    public static class SvgHtmlWrapper
    {
        private static readonly Regex SizeRegex = new Regex(
            "<svg width=\"(?<Width>\\d+)px\" height=\"(?<Height>\\d+)px",
            RegexOptions.ExplicitCapture
            | RegexOptions.Multiline
            | RegexOptions.Compiled
        );

        public static string DumpHtml(GraphvizSize size, string svgFileName)
        {
            var outputFile = string.Format("{0}.html", svgFileName);
            using (var stream = new FileStream(outputFile, FileMode.Open, FileAccess.Write))
            {
                using (var html = new StreamWriter(stream))
                {
                    html.WriteLine("<html>");
                    html.WriteLine("<body>");
                    html.WriteLine(
                        "<object data=\"{0}\" type=\"image/svg+xml\" width=\"{1}\" height=\"{2}\">",
                        svgFileName,
                        size.Width,
                        size.Height);
                    html.WriteLine(
                        "  <embed src=\"{0}\" type=\"image/svg+xml\" width=\"{1}\" height=\"{2}\">",
                        svgFileName,
                        size.Width,
                        size.Height);
                    html.WriteLine("If you see this, you need to install a SVG viewer");
                    html.WriteLine("  </embed>");
                    html.WriteLine("</object>");
                    html.WriteLine("</body>");
                    html.WriteLine("</html>");
                }
            }

            return outputFile;
        }

        public static GraphvizSize ParseSize(string svg)
        {
            var m = SizeRegex.Match(svg);
            if (!m.Success)
            {
                return new GraphvizSize(400, 400);
            }

            var size = int.Parse(m.Groups["Width"].Value);
            var height = int.Parse(m.Groups["Height"].Value);
            return new GraphvizSize(size, height);
        }

        /// <summary>Creates a HTML file that wraps the SVG and returns the file name</summary>
        /// <param name="svgFileName"></param>
        /// <returns></returns>
        public static string WrapSvg(string svgFileName)
        {
            GraphvizSize size;
            using (var stream = new FileStream(svgFileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    size = ParseSize(reader.ReadToEnd());
                }
            }
            return DumpHtml(size, svgFileName);
        }
    }
}