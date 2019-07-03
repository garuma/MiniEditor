using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text.Formatting;

namespace Microsoft.VisualStudio.Text.Editor.Implementation
{
    class MockTextSelection : ITextSelection
    {
        Span selectedSpan;

        public MockTextSelection (ITextView textView)
        {
            this.TextView = textView;
        }

        ITextSnapshot CurrentSnapshot => TextView.TextBuffer.CurrentSnapshot;

        public ITextView TextView { get; private set; }

        public NormalizedSnapshotSpanCollection SelectedSpans
            => new NormalizedSnapshotSpanCollection(new SnapshotSpan(CurrentSnapshot, selectedSpan));

        public ReadOnlyCollection<VirtualSnapshotSpan> VirtualSelectedSpans
            => new ReadOnlyCollection<VirtualSnapshotSpan>(new VirtualSnapshotSpan[] { StreamSelectionSpan });

        public VirtualSnapshotSpan StreamSelectionSpan
            => new VirtualSnapshotSpan(new SnapshotSpan(CurrentSnapshot, selectedSpan));

        public TextSelectionMode Mode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsReversed { get; private set; }

        public bool IsEmpty => selectedSpan.Length == 0;

        public bool IsActive { get; set; }
        public bool ActivationTracksFocus { get; set; }

        public VirtualSnapshotPoint ActivePoint => Start;

        public VirtualSnapshotPoint AnchorPoint => Start;

        public VirtualSnapshotPoint Start
            => new VirtualSnapshotPoint(new SnapshotPoint(CurrentSnapshot, selectedSpan.Start));

        public VirtualSnapshotPoint End
            => new VirtualSnapshotPoint(new SnapshotPoint(CurrentSnapshot, selectedSpan.End));

        public event EventHandler SelectionChanged;

        public void Clear() => Select(new SnapshotSpan(CurrentSnapshot, new Span()), false);

        public VirtualSnapshotSpan? GetSelectionOnTextViewLine(ITextViewLine line)
        {
            throw new NotImplementedException();
        }

        public void Select(SnapshotSpan selectionSpan, bool isReversed)
        {
            IsReversed = isReversed;
            this.selectedSpan = selectionSpan.Span;
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Select(VirtualSnapshotPoint anchorPoint, VirtualSnapshotPoint activePoint)
        {
            throw new NotImplementedException();
        }
    }
}
