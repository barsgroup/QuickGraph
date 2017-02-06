namespace QuickGraph.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public sealed class BinaryQueue<TVertex, TDistance> :
        IPriorityQueue<TVertex>
    {
        private readonly Func<TVertex, TDistance> _distances;

        private readonly BinaryHeap<TDistance, TVertex> _heap;

        public int Count => _heap.Count;

        public BinaryQueue(
            Func<TVertex, TDistance> distances
        )
            : this(distances, Comparer<TDistance>.Default.Compare)
        {
        }

        public BinaryQueue(
            Func<TVertex, TDistance> distances,
            Func<TDistance, TDistance, int> distanceComparison
        )
        {
            Contract.Requires(distances != null);
            Contract.Requires(distanceComparison != null);

            _distances = distances;
            _heap = new BinaryHeap<TDistance, TVertex>(distanceComparison);
        }

        public bool Contains(TVertex value)
        {
            return _heap.IndexOf(value) > -1;
        }

        public TVertex Dequeue()
        {
            return _heap.RemoveMinimum().Value;
        }

        public void Enqueue(TVertex value)
        {
            _heap.Add(_distances(value), value);
        }

        public TVertex Peek()
        {
            return _heap.Minimum().Value;
        }

        public TVertex[] ToArray()
        {
            return _heap.ToValueArray();
        }

        public void Update(TVertex v)
        {
            _heap.Update(_distances(v), v);
        }
    }
}