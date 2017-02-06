﻿namespace QuickGraph.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public sealed class SoftHeap<TKey, TValue>
        : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly Head _header;

        private readonly Head _tail;

        public int MinRank { get; }

        public Func<TKey, TKey, int> Comparison { get; }

        public double ErrorRate { get; }

        public int Count { get; private set; }

        public TKey KeyMaxValue { get; }

        public SoftHeap(double maximumErrorRate, TKey keyMaxValue)
            : this(maximumErrorRate, keyMaxValue, Comparer<TKey>.Default.Compare)
        {
        }

        public SoftHeap(double maximumErrorRate, TKey keyMaxValue, Func<TKey, TKey, int> comparison)
        {
            Contract.Requires(comparison != null);
            Contract.Requires(0 < maximumErrorRate && maximumErrorRate <= 0.5);

            Comparison = comparison;
            KeyMaxValue = keyMaxValue;
            _header = new Head();
            _tail = new Head();
            _tail.Rank = int.MaxValue;
            _header.Next = _tail;
            _tail.Prev = _header;
            ErrorRate = maximumErrorRate;
            MinRank = 2 + 2 * (int)Math.Ceiling(Math.Log(1.0 / ErrorRate, 2.0));
            Count = 0;
        }

        public void Add(TKey key, TValue value)
        {
            Contract.Requires(Comparison(key, KeyMaxValue) < 0);

            var l = new Cell(key, value);
            var q = new Node(l);

            Meld(q);
            Count++;
        }

        public KeyValuePair<TKey, TValue> DeleteMin()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException();
            }

            var h = _header.Next.SuffixMin;
            while (h.Queue.Il == null)
            {
                var tmp = h.Queue;
                var childCount = 0;
                while (tmp.Next != null)
                {
                    tmp = tmp.Next;
                    childCount++;
                }

                if (childCount < h.Rank / 2)
                {
                    h.Prev.Next = h.Next;
                    h.Next.Prev = h.Prev;
                    FixMinLinst(h.Prev);
                    tmp = h.Queue;
                    while (tmp.Next != null)
                    {
                        Meld(tmp.Child);
                        tmp = tmp.Next;
                    }
                }
                else
                {
                    h.Queue = Shift(h.Queue);
                    if (Comparison(h.Queue.CKey, KeyMaxValue) == 0)
                    {
                        h.Prev.Next = h.Next;
                        h.Next.Prev = h.Prev;
                        h = h.Prev;
                    }
                }

                h = _header.Next.SuffixMin;
            } // end of outer while loop

            var min = h.Queue.Il.Key;
            var value = h.Queue.Il.Value;
            h.Queue.Il = h.Queue.Il.Next;
            if (h.Queue.Il == null)
            {
                h.Queue.IlTail = null;
            }

            Count--;
            return new KeyValuePair<TKey, TValue>(min, value);
        }

        private void FixMinLinst(Head h)
        {
            Contract.Requires(h != null);

            Head tmpmin;
            if (h.Next == _tail)
            {
                tmpmin = h;
            }
            else
            {
                tmpmin = h.Next.SuffixMin;
            }

            while (h != _header)
            {
                if (Comparison(tmpmin.Queue.CKey, h.Queue.CKey) > 0)
                {
                    tmpmin = h;
                }

                h.SuffixMin = tmpmin;
                h = h.Prev;
            }
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Count > -1);
            Contract.Invariant(_header != null);
            Contract.Invariant(_tail != null);
        }

        private void Meld(Node q)
        {
            Contract.Requires(q != null);

            var toHead = _header.Next;
            while (q.Rank > toHead.Rank)
            {
                Contract.Assert(toHead.Next != null);
                toHead = toHead.Next;
            }
            var prevHead = toHead.Prev;
            while (q.Rank == toHead.Rank)
            {
                Node top, bottom;
                if (Comparison(toHead.Queue.CKey, q.CKey) > 0)
                {
                    top = q;
                    bottom = toHead.Queue;
                }
                else
                {
                    top = toHead.Queue;
                    bottom = q;
                }
                q = new Node(top.CKey, top.Rank + 1, bottom, top, top.Il, top.IlTail);
                toHead = toHead.Next;
            }

            Head h;
            if (prevHead == toHead.Prev)
            {
                h = new Head();
            }
            else
            {
                h = prevHead.Next;
            }
            h.Queue = q;
            h.Rank = q.Rank;
            h.Prev = prevHead;
            h.Next = toHead;
            prevHead.Next = h;
            toHead.Prev = h;

            FixMinLinst(h);
        }

        private Node Shift(Node v)
        {
            Contract.Requires(v != null);

            v.Il = null;
            v.IlTail = null;
            if (v.Next == null && v.Child == null)
            {
                v.CKey = KeyMaxValue;
                return v;
            }

            v.Next = Shift(v.Next);

            // restore heap ordering that might be broken by shifting
            if (Comparison(v.Next.CKey, v.Child.CKey) > 0)
            {
                var tmp = v.Child;
                v.Child = v.Next;
                v.Next = tmp;
            }

            v.Il = v.Next.Il;
            v.IlTail = v.Next.IlTail;
            v.CKey = v.Next.CKey;

            // softening the heap
            if (v.Rank > MinRank &&
                (v.Rank % 2 == 1 || v.Child.Rank < v.Rank - 1))
            {
                v.Next = Shift(v.Next);

                // restore heap ordering that might be broken by shifting
                if (Comparison(v.Next.CKey, v.Child.CKey) > 0)
                {
                    var tmp = v.Child;
                    v.Child = v.Next;
                    v.Next = tmp;
                }

                if (Comparison(v.Next.CKey, KeyMaxValue) != 0 && v.Next.Il != null)
                {
                    v.Next.IlTail.Next = v.Il;
                    v.Il = v.Next.Il;
                    if (v.IlTail == null)
                    {
                        v.IlTail = v.Next.IlTail;
                    }
                    v.CKey = v.Next.CKey;
                }
            } // end second shift

            if (Comparison(v.Child.CKey, KeyMaxValue) == 0)
            {
                if (Comparison(v.Next.CKey, KeyMaxValue) == 0)
                {
                    v.Child = null;
                    v.Next = null;
                }
                else
                {
                    v.Child = v.Next.Child;
                    v.Next = v.Next.Next;
                }
            }

            return v;
        } // Shift

        private sealed class Cell
        {
            public readonly TKey Key;

            public readonly TValue Value;

            public Cell Next;

            public Cell(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        private sealed class Head
        {
            public Head Next;

            public Head Prev;

            public Node Queue;

            public int Rank;

            public Head SuffixMin;
        }

        private sealed class Node
        {
            public readonly int Rank;

            public Node Child;

            public TKey CKey;

            public Cell Il;

            public Cell IlTail;

            public Node Next;

            public Node(Cell cell)
            {
                Rank = 0;
                CKey = cell.Key;
                Il = cell;
                IlTail = cell;
            }

            public Node(TKey cKey, int rank, Node next, Node child, Cell il, Cell iltail)
            {
                CKey = cKey;
                Rank = rank;
                Next = next;
                Child = child;
                Il = il;
                IlTail = iltail;
            }
        }

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private struct Enumerator
            : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly SoftHeap<TKey, TValue> _owner;

            public Enumerator(SoftHeap<TKey, TValue> owner)
            {
                Contract.Requires(owner != null);
                _owner = owner;
                Current = new KeyValuePair<TKey, TValue>();
            }

            public bool MoveNext()
            {
                // TODO
                return false;
            }

            public KeyValuePair<TKey, TValue> Current { get; }

            public void Dispose()
            {
            }

            object IEnumerator.Current => Current;

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        #endregion
    }
}