namespace QuickGraph
{
    using System;
    using System.Diagnostics.Contracts;

    public class TaggedEdge<TVertex, TTag>
        : Edge<TVertex>
          ,
          ITagged<TTag>
    {
        private TTag _tag;

        public TTag Tag
        {
            get { return _tag; }
            set
            {
                if (!Equals(_tag, value))
                {
                    _tag = value;
                    OnTagChanged(EventArgs.Empty);
                }
            }
        }

        public TaggedEdge(TVertex source, TVertex target, TTag tag)
            : base(source, target)
        {
            Contract.Ensures(Equals(Tag, tag));

            _tag = tag;
        }

        protected virtual void OnTagChanged(EventArgs e)
        {
            var eh = TagChanged;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        public event EventHandler TagChanged;
    }
}