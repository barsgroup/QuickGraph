namespace QuickGraph
{
    using System.Collections.Generic;

    public interface IHyperEdge<TVertex>
    {
        int EndPointCount { get; }

        IEnumerable<TVertex> EndPoints { get; }
    }
}