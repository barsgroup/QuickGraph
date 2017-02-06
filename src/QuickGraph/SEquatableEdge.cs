namespace QuickGraph
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;

    /// <summary>An struct based <see cref="IEdge&lt;TVertex&gt;" /> implementation.</summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    [DebuggerDisplay(EdgeExtensions.DebuggerDisplayEdgeFormatString)]
    [StructLayout(LayoutKind.Auto)]
    public struct SEquatableEdge<TVertex>
        : IEdge<TVertex>
          ,
          IEquatable<SEquatableEdge<TVertex>>
    {
        /// <summary>Initializes a new instance of the <see cref="SEdge&lt;TVertex&gt;" /> class.</summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public SEquatableEdge(TVertex source, TVertex target)
        {
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Ensures(Contract.ValueAtReturn(out this).Source.Equals(source));
            Contract.Ensures(Contract.ValueAtReturn(out this).Target.Equals(target));

            Source = source;
            Target = target;
        }

        /// <summary>Gets the source vertex</summary>
        /// <value></value>
        public TVertex Source { get; }

        /// <summary>Gets the target vertex</summary>
        /// <value></value>
        public TVertex Target { get; }

        /// <summary>Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.</summary>
        /// <returns>A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.</returns>
        public override string ToString()
        {
            return string.Format(
                EdgeExtensions.EdgeFormatString,
                Source,
                Target);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(SEquatableEdge<TVertex> other)
        {
            Contract.Ensures(
                Contract.Result<bool>() ==
                (Source.Equals(other.Source) &&
                 Target.Equals(other.Target))
            );

            return
                Source.Equals(other.Source) &&
                Target.Equals(other.Target);
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        ///     true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise,
        ///     false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return
                obj is SEquatableEdge<TVertex> &&
                Equals((SEquatableEdge<TVertex>)obj);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return HashCodeHelper.Combine(
                Source.GetHashCode(),
                Target.GetHashCode());
        }
    }
}