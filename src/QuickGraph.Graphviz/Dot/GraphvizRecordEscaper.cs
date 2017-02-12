namespace QuickGraph.Graphviz.Dot
{
    using System.Diagnostics.Contracts;
    using System.Text.RegularExpressions;

    public sealed class GraphvizRecordEscaper
    {
        private readonly Regex _escapeRegExp = new Regex("(?<Eol>\\n)|(?<Common>\\[|\\]|\\||<|>|\"| )", RegexOptions.ExplicitCapture | RegexOptions.Multiline);

        public string Escape(string text)
        {
            Contract.Requires(text != null);

            return _escapeRegExp.Replace(text, MatchEvaluator);
        }

        public string MatchEvaluator(Match m)
        {
            if (m.Groups["Common"] != null)
            {
                return string.Format(@"\{0}", m.Value);
            }
            return @"\n";
        }
    }
}