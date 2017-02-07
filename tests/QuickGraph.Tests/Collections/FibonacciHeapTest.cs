namespace QuickGraph.Tests.Collections
{
    using System;
    using System.Collections.Generic;

    using QuickGraph.Collections;

    using Xunit;

    public partial class FibonacciHeapTests
    {
        [Fact]
        public void ChangeKeyToSelf()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Decreasing);
            var count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (var i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            heap.ChangeKey(cells[0], 0);
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                if (lastValue < value.Key)
                {
                    Assert.False(true,"Heap condition has been violated");
                }
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(count, 0);
        }

        [Fact]
        public void CreateHeap()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            Assert.NotNull(heap);
        }

        [Fact]
        public void DecreaseKeyOnDecreasing()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Decreasing);
            var count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (var i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            heap.ChangeKey(cells[9], -1);
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                if (lastValue < value.Key)
                {
                    Assert.False(true,"Heap condition has been violated");
                }
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(count, 0);
        }

        [Fact]
        public void DecreaseKeyOnIncreasing()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (var i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            heap.ChangeKey(cells[9], -1);
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                if (lastValue > value.Key)
                {
                    Assert.False(true,"Heap condition has been violated");
                }
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(count, 0);
        }

        [Fact]
        public void DeleteKey()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (var i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            heap.Dequeue();
            var deletedCell = cells[8];
            heap.Delete(deletedCell);
            count -= 2;
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                Assert.False(lastValue > value.Key, "Heap condition has been violated");
                //Assert.NotEqual(deletedCell, value);
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(count, 0);
        }

        [Fact]
        public void EnumeratorIncreasing()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var count = 0;
            for (var i = 10; i >= 0; i--)
            {
                heap.Enqueue(i, i.ToString());
                count++;
            }
            int? lastValue = null;
            foreach (var value in heap)
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                if (lastValue > value.Key)
                {
                    Assert.False(true,"Heap condition has been violated");
                }
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(count, 0);
        }

        [Fact]
        public void IncreaseKeyOnDecreasing()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Decreasing);
            var count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (var i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            heap.ChangeKey(cells[0], 100);
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                if (lastValue < value.Key)
                {
                    Assert.False(true,"Heap condition has been violated");
                }
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(count, 0);
        }

        [Fact]
        public void IncreaseKeyOnIncreasing()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (var i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            heap.ChangeKey(cells[0], 100);
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                if (lastValue > value.Key)
                {
                    Assert.False(true,"Heap condition has been violated");
                }
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(count, 0);
        }

        [Fact]
        public void IncreasingDecreaseKeyCascadeCut()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (var i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            lastValue = heap.Top.Priority;
            heap.Dequeue();
            count--;
            heap.ChangeKey(cells[6], 3);
            heap.ChangeKey(cells[7], 2);
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                if (lastValue > value.Key)
                {
                    Assert.False(true,"Heap condition has been violated");
                }
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(count, 0);
        }

        [Fact]
        public void IncreasingIncreaseKeyCascadeCut()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (var i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            lastValue = heap.Top.Priority;
            heap.Dequeue();
            count--;
            heap.ChangeKey(cells[5], 10);
            var s = heap.DrawHeap();
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                if (lastValue > value.Key)
                {
                    Assert.False(true, $"Heap condition has been violated: {lastValue} > {value.Key}");
                }
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(count, 0);
        }

        //[PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
        //public void InsertAndMinimum<TPriority, TValue>(
        //    [PexAssumeUnderTest] FibonacciHeap<TPriority, TValue> target,
        //    KeyValuePair<TPriority, TValue>[] kvs)
        //{
        //    PexAssume.True(kvs.Length > 0);

        //    var count = target.Count;
        //    var minimum = default(TPriority);
        //    for (var i = 0; i < kvs.Length; ++i)
        //    {
        //        var kv = kvs[i];
        //        if (i == 0)
        //        {
        //            minimum = kv.Key;
        //        }
        //        else
        //        {
        //            minimum = target.PriorityComparison(kv.Key, minimum) < 0
        //                          ? kv.Key
        //                          : minimum;
        //        }
        //        target.Enqueue(kv.Key, kv.Value);

        //        // check minimum
        //        var kvMin = target.Top.Priority;
        //        Assert.Equal(minimum, kvMin);
        //    }
        //    AssertInvariant(target);
        //}

        //public void InsertAndRemoveMinimum<TPriority, TValue>(
        //    [PexAssumeUnderTest] FibonacciHeap<TPriority, TValue> target,
        //    KeyValuePair<TPriority, TValue>[] kvs)
        //{
        //    var count = target.Count;
        //    foreach (var kv in kvs)
        //        target.Enqueue(kv.Key, kv.Value);

        //    var minimum = default(TPriority);
        //    for (var i = 0; i < kvs.Length; ++i)
        //    {
        //        if (i == 0)
        //        {
        //            minimum = target.Dequeue().Key;
        //        }
        //        else
        //        {
        //            var m = target.Dequeue().Key;
        //            Assert.True(target.PriorityComparison(minimum, m) <= 0);
        //            minimum = m;
        //        }
        //        AssertInvariant(target);
        //    }

        //    Assert.Equal(0, target.Count);
        //}

        [Fact]
        public void MergeTest()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var heap2 = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var count = 0;
            for (var i = 11; i > 0; i--)
            {
                heap.Enqueue(i, i.ToString());
                heap2.Enqueue(i * 11, i.ToString());
                count += 2;
            }
            heap2.Merge(heap);
            int? lastValue = null;
            foreach (var value in heap2.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                if (lastValue > value.Key)
                {
                    Assert.False(true,"Heap condition has been violated");
                }
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(count, 0);
        }

        [Fact]
        public void NextCutOnGreaterThan()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var heap2 = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var toCutNodes = new List<FibonacciHeapCell<int, string>>();
            var count = 0;
            heap.Enqueue(1, "1");
            toCutNodes.Add(heap.Enqueue(5, "5"));
            toCutNodes.Add(heap.Enqueue(6, "6"));
            toCutNodes.Add(heap.Enqueue(7, "7"));
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            heap.Enqueue(0, "0");
            heap2.Enqueue(-1, "-1");
            heap2.Enqueue(5, "5");
            heap2.Enqueue(-10, "-10");
            heap2.Dequeue();
            heap.Merge(heap2);
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            toCutNodes.ForEach(x => heap.ChangeKey(x, -5));
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            count = 7;
            int? lastValue = null;
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                if (lastValue > value.Key)
                {
                    Assert.False(true,"Heap condition has been violated");
                }
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(count, 0);
        }

        [Fact]
        public void NextCutOnLessThan()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var heap2 = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var toCutNodes = new List<FibonacciHeapCell<int, string>>();
            var count = 0;
            heap.Enqueue(0, "0");
            toCutNodes.Add(heap.Enqueue(5, "5"));
            toCutNodes.Add(heap.Enqueue(6, "6"));
            toCutNodes.Add(heap.Enqueue(7, "7"));
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            heap.Enqueue(1, "1");
            heap2.Enqueue(4, "4");
            heap2.Enqueue(5, "5");
            heap2.Enqueue(-10, "-10");
            heap2.Dequeue();
            heap.Merge(heap2);
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            toCutNodes.ForEach(x => heap.ChangeKey(x, -5));
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            count = 7;
            int? lastValue = null;
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                if (lastValue > value.Key)
                {
                    Assert.False(true,"Heap condition has been violated");
                }
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(count, 0);
        }

        //[PexMethod(MaxConstraintSolverTime = 2)]
        //[PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
        //public void Operations<TPriority, TValue>(
        //    [PexAssumeUnderTest] FibonacciHeap<TPriority, TValue> target,
        //    KeyValuePair<bool, TPriority>[] values)
        //{
        //    foreach (var value in values)
        //    {
        //        if (value.Key)
        //        {
        //            target.Enqueue(value.Value, default(TValue));
        //        }
        //        else
        //        {
        //            var min = target.Dequeue();
        //        }
        //        AssertInvariant(target);
        //    }
        //}

        [Fact]
        public void RandomTest()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var rand = new Random(10);
            var numberOfRecords = 10000;
            var rangeMultiplier = 10;
            var count = 0;
            var cells = new List<FibonacciHeapCell<int, string>>();
            for (var i = 0; i < numberOfRecords; i++)
            {
                cells.Add(heap.Enqueue(rand.Next(0, numberOfRecords * rangeMultiplier), i.ToString()));
                count++;
            }
            while (!heap.IsEmpty)
            {
                var action = rand.Next(1, 6);
                var i = 0;
                while (action == 1 && i < 2)
                {
                    action = rand.Next(1, 6);
                    i++;
                }
                var lastValue = int.MinValue;
                switch (action)
                {
                    case 1:
                        cells.Add(heap.Enqueue(rand.Next(0, numberOfRecords * rangeMultiplier), "SomeValue"));
                        count++;
                        break;
                    case 2:
                        Assert.False(lastValue > heap.Top.Priority, "Heap Condition Violated");
                        lastValue = heap.Top.Priority;
                        cells.Remove(heap.Top);
                        heap.Dequeue();
                        count--;
                        break;
                    case 3:
                        var deleteIndex = rand.Next(0, cells.Count);
                        heap.Delete(cells[deleteIndex]);
                        cells.RemoveAt(deleteIndex);
                        count--;
                        break;
                    case 4:
                        var decreaseIndex = rand.Next(0, cells.Count);
                        var newValue = rand.Next(0, cells[decreaseIndex].Priority);
                        if (newValue < lastValue)
                        {
                            lastValue = newValue;
                        }
                        heap.ChangeKey(cells[decreaseIndex], newValue);
                        break;
                    case 5:
                        var increaseIndex = rand.Next(0, cells.Count);
                        heap.ChangeKey(cells[increaseIndex], rand.Next(cells[increaseIndex].Priority, numberOfRecords * rangeMultiplier));
                        break;
                    default:
                        break;
                }
            }
            Assert.Equal(count, 0);
        }

        [Fact]
        public void SimpleEnqueDequeDecreasing()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Decreasing);
            var count = 0;
            for (var i = 0; i < 10; i++)
            {
                heap.Enqueue(i, i.ToString());
                count++;
            }
            int? lastValue = null;

            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                if (lastValue < value.Key)
                {
                    Assert.False(true,"Heap condition has been violated");
                }
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(count, 0);
        }

        [Fact]
        public void SimpleEnqueDequeIncreasing()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var count = 0;
            for (var i = 0; i < 10; i++)
            {
                heap.Enqueue(i, i.ToString());
                count++;
            }
            int? lastValue = null;
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                if (lastValue > value.Key)
                {
                    Assert.False(true, $"Heap condition has been violated: {lastValue} > {value.Key}");
                }
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(count, 0);
        }

        /// <summary>Checks heap invariant</summary>
        private static void AssertInvariant<TPriority, TValue>(
            FibonacciHeap<TPriority, TValue> target
        )
        {
            Assert.True(target.Count >= 0);
        }
    }
}