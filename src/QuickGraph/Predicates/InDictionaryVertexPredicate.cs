namespace QuickGraph.Predicates
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public sealed class InDictionaryVertexPredicate<TVertex, TValue>
    {
        private readonly IDictionary<TVertex, TValue> _dictionary;

        public InDictionaryVertexPredicate(
            IDictionary<TVertex, TValue> dictionary)
        {
            Contract.Requires(dictionary != null);
            _dictionary = dictionary;
        }

        [Pure]
        public bool Test(TVertex v)
        {
            Contract.Requires(v != null);

            return _dictionary.ContainsKey(v);
        }
    }
}