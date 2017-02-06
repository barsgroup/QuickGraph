namespace QuickGraph.Collections
{
    public interface IQueue<T>
    {
        int Count { get; }

        bool Contains(T value);

        T Dequeue();

        void Enqueue(T value);

        T Peek();

        T[] ToArray();
    }
}