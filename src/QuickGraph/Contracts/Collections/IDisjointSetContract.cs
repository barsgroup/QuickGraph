namespace QuickGraph.Collections.Contracts
{
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IDisjointSet<>))]
    internal abstract class DisjointSetContract<T>
        : IDisjointSet<T>
    {
        int IDisjointSet<T>.SetCount => default(int);

        int IDisjointSet<T>.ElementCount => default(int);

        bool IDisjointSet<T>.AreInSameSet(T left, T right)
        {
            IDisjointSet<T> ithis = this;
            Contract.Requires(left != null);
            Contract.Requires(right != null);
            Contract.Requires(ithis.Contains(left));
            Contract.Requires(ithis.Contains(right));

            return default(bool);
        }

        [Pure]
        bool IDisjointSet<T>.Contains(T value)
        {
            return default(bool);
        }

        T IDisjointSet<T>.FindSet(T value)
        {
            IDisjointSet<T> ithis = this;
            Contract.Requires(value != null);
            Contract.Requires(ithis.Contains(value));

            return default(T);
        }

        void IDisjointSet<T>.MakeSet(T value)
        {
            IDisjointSet<T> ithis = this;
            Contract.Requires(value != null);
            Contract.Requires(!ithis.Contains(value));
            Contract.Ensures(ithis.Contains(value));
            Contract.Ensures(ithis.SetCount == Contract.OldValue(ithis.SetCount) + 1);
            Contract.Ensures(ithis.ElementCount == Contract.OldValue(ithis.ElementCount) + 1);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            IDisjointSet<T> ithis = this;
            Contract.Invariant(0 <= ithis.SetCount);
            Contract.Invariant(ithis.SetCount <= ithis.ElementCount);
        }

        bool IDisjointSet<T>.Union(T left, T right)
        {
            IDisjointSet<T> ithis = this;
            Contract.Requires(left != null);
            Contract.Requires(ithis.Contains(left));
            Contract.Requires(right != null);
            Contract.Requires(ithis.Contains(right));

            return default(bool);
        }
    }
}