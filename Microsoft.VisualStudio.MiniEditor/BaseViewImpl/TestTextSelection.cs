using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Text.Formatting;

namespace Microsoft.VisualStudio.Text.Editor.Implementation
{
	class TestTextSelection : ITextSelection
    {
		readonly IMultiSelectionBroker multiSelectionBroker;

		public TestTextSelection (ITextView textView, IMultiSelectionBroker multiSelectionBroker)
        {
            TextView = textView;
			this.multiSelectionBroker = multiSelectionBroker;
			multiSelectionBroker.MultiSelectionSessionChanged += MultiSelectionSessionChanged;
        }

		void MultiSelectionSessionChanged (object sender, EventArgs e)
		{
			SelectionChanged?.Invoke (this, e);
		}

        public ITextView TextView { get; private set; }

        public NormalizedSnapshotSpanCollection SelectedSpans
            => new NormalizedSnapshotSpanCollection (StreamSelectionSpan.SnapshotSpan);

        public ReadOnlyCollection<VirtualSnapshotSpan> VirtualSelectedSpans
            => new ReadOnlyCollection<VirtualSnapshotSpan>(new VirtualSnapshotSpan[] { StreamSelectionSpan });

		public VirtualSnapshotSpan StreamSelectionSpan => multiSelectionBroker.PrimarySelection.Extent;

        public TextSelectionMode Mode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public bool IsReversed => multiSelectionBroker.PrimarySelection.IsReversed;

        public bool IsEmpty => multiSelectionBroker.PrimarySelection.IsEmpty;

        public bool IsActive { get; set; }
        public bool ActivationTracksFocus { get; set; }

        public VirtualSnapshotPoint ActivePoint => multiSelectionBroker.PrimarySelection.ActivePoint;

        public VirtualSnapshotPoint AnchorPoint => multiSelectionBroker.PrimarySelection.AnchorPoint;

		public VirtualSnapshotPoint Start => multiSelectionBroker.PrimarySelection.Start;

        public VirtualSnapshotPoint End => multiSelectionBroker.PrimarySelection.End;

		public event EventHandler SelectionChanged;

		public void Clear ()
			=> multiSelectionBroker.SetSelection (new Selection (multiSelectionBroker.PrimarySelection.InsertionPoint));

        public VirtualSnapshotSpan? GetSelectionOnTextViewLine(ITextViewLine line)
        {
            throw new NotImplementedException();
        }

		public void Select (SnapshotSpan selectionSpan, bool isReversed)
			=> multiSelectionBroker.SetSelection (new Selection (selectionSpan, isReversed));

		public void Select (VirtualSnapshotPoint anchorPoint, VirtualSnapshotPoint activePoint)
			=> multiSelectionBroker.SetSelection (new Selection (anchorPoint, activePoint));
    }
}
