namespace QuickGraph.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>Disjoint-set implementation with path compression and union-by-rank optimizations. optimization</summary>
    /// <typeparam name="T"></typeparam>
    public class ForestDisjointSet<T>
        : IDisjointSet<T>
    {
        private readonly Dictionary<T, Element> _elements;

        public int SetCount { get; private set; }

        public int ElementCount => _elements.Count;

        public ForestDisjointSet(int elementCapacity)
        {
            Contract.Requires(elementCapacity >= 0 && elementCapacity < int.MaxValue);
            _elements = new Dictionary<T, Element>(elementCapacity);
            SetCount = 0;
        }

        public ForestDisjointSet()
        {
            _elements = new Dictionary<T, Element>();
            SetCount = 0;
        }

        public bool AreInSameSet(T left, T right)
        {
            return FindSet(left).Equals(FindSet(right));
        }

        [Pure]
        public bool Contains(T value)
        {
            return _elements.ContainsKey(value);
        }

        public T FindSet(T value)
        {
            return Find(_elements[value]).Value;
        }

        /// <summary>Adds a new set</summary>
        /// <param name="value"></param>
        public void MakeSet(T value)
        {
            var element = new Element(value);
            _elements.Add(value, element);
            SetCount++;
        }

        public bool Union(T left, T right)
        {
            return Union(_elements[left], _elements[right]);
        }

        private static void CompressPath(Element element, Element root)
        {
            Contract.Requires(element != null);
            Contract.Requires(root != null);

            // path compression
            var current = element;
            while (current != root)
            {
                var temp = current;
                current = current.Parent;
                temp.Parent = root;
            }
        }

        /// <summary>Finds the parent element, and applies path compression</summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private Element Find(Element element)
        {
            Contract.Requires(element != null);
            Contract.Ensures(Contract.Result<Element>() != null);

            var root = FindNoCompression(element);
            CompressPath(element, root);
            return root;
        }

        [Pure]
        private Element FindNoCompression(Element element)
        {
            Contract.Requires(element != null);
            Contract.Ensures(Contract.Result<Element>() != null);

            // find root,
            var current = element;
            while (current.Parent != null)
                current = current.Parent;

            return current;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(SetCount >= 0);
            Contract.Invariant(SetCount <= _elements.Count);
        }

        private bool Union(Element left, Element right)
        {
            Contract.Requires(left != null);
            Contract.Requires(right != null);
            Contract.Ensures(
                Contract.Result<bool>()
                    ? Contract.OldValue(SetCount) - 1 == SetCount
                    : Contract.OldValue(SetCount) == SetCount);
            Contract.Ensures(FindNoCompression(left) == FindNoCompression(right));

            // shortcut when already unioned,
            if (left == right)
            {
                return false;
            }

            var leftRoot = Find(left);
            var rightRoot = Find(right);

            // union by rank
            if (leftRoot.Rank > rightRoot.Rank)
            {
                rightRoot.Parent = leftRoot;
            }
            else if (leftRoot.Rank < rightRoot.Rank)
            {
                leftRoot.Parent = rightRoot;
            }
            else if (leftRoot != rightRoot)
            {
                rightRoot.Parent = leftRoot;
                leftRoot.Rank = leftRoot.Rank + 1;
            }
            else
            {
                return false; // do not update the setcount
            }

            SetCount--;
            return true;
        }

#if DEBUG
        [DebuggerDisplay("{Id}:{Rank}->{Parent}")]
#endif
        private class Element
        {
            public readonly T Value;

            public Element Parent;

            public int Rank;

            public Element(T value)
            {
#if DEBUG
                Id = nextId++;
#endif
                Parent = null;
                Rank = 0;
                Value = value;
            }

#if DEBUG
            public readonly int Id;

            private static int nextId;
#endif
        }
    }
}