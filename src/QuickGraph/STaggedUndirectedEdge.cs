﻿namespace QuickGraph
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;

    /// <summary>An struct based <see cref="IUndirectedEdge&lt;TVertex&gt;" /> implementation.</summary>
    /// <typeparam name="TVertex">type of the vertex.</typeparam>
    /// <typeparam name="TTag">type of the tag</typeparam>
    [DebuggerDisplay(EdgeExtensions.DebuggerDisplayTaggedUndirectedEdgeFormatString)]
    [StructLayout(LayoutKind.Auto)]
    public struct SUndirectedTaggedEdge<TVertex, TTag>
        : IUndirectedEdge<TVertex>
          ,
          ITagged<TTag>
    {
        private TTag tag;

        /// <summary>Initializes a new instance of the <see cref="SUndirectedTaggedEdge&lt;TVertex, TTag&gt;" /> class.</summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="tag">The tag.</param>
        public SUndirectedTaggedEdge(TVertex source, TVertex target, TTag tag) : this()
        {
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Requires(Comparer<TVertex>.Default.Compare(source, target) <= 0);
            Contract.Ensures(Contract.ValueAtReturn(out this).Source.Equals(source));
            Contract.Ensures(Contract.ValueAtReturn(out this).Target.Equals(target));

            Source = source;
            Target = target;
            this.tag = tag;
            TagChanged = null;
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
                EdgeExtensions.TaggedUndirectedEdgeFormatString,
                Source,
                Target,
                Tag);
        }

        public event EventHandler TagChanged;

        private void OnTagChanged(EventArgs e)
        {
            var eh = TagChanged;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        public TTag Tag
        {
            get { return tag; }
            set
            {
                if (!Equals(tag, value))
                {
                    tag = value;
                    OnTagChanged(EventArgs.Empty);
                }
            }
        }
    }
}