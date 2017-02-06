namespace QuickGraph.Algorithms
{
    using System;

    public delegate void AlgorithmEventHandler<TGraph>(
        IAlgorithm<TGraph> sender,
        EventArgs e);
}