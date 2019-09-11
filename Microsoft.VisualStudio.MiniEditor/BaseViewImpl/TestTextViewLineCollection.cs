using System;
using Microsoft.VisualStudio.Text.Formatting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Microsoft.VisualStudio.Text.Editor.Implementation
{
    class TestTextViewLineCollection : List<ITextViewLine>, ITextViewLineCollection
    {
        readonly TestTextView testTextView;

        public TestTextViewLineCollection(TestTextView testTextView)
        {
            this.testTextView = testTextView;
        }

        public ITextViewLine FirstVisibleLine => this.FirstOrDefault();

        public ITextViewLine LastVisibleLine => this.LastOrDefault();

        public SnapshotSpan FormattedSpan => throw new NotImplementedException();

        public bool IsValid => true;

        public bool ContainsBufferPosition(SnapshotPoint bufferPosition)
            => false;

        public TextBounds GetCharacterBounds(SnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }

        public int GetIndexOfTextLine(ITextViewLine textLine)
            => -1;

        public Collection<TextBounds> GetNormalizedTextBounds(SnapshotSpan bufferSpan)
        {
            throw new NotImplementedException();
        }

        public SnapshotSpan GetTextElementSpan(SnapshotPoint bufferPosition)
        {
            var line = GetTextViewLineContainingBufferPosition(bufferPosition);
            if (line == null)
            {
                throw new ArgumentException();
            }
            return line.GetTextElementSpan(bufferPosition);
        }

        public ITextViewLine GetTextViewLineContainingBufferPosition(SnapshotPoint bufferPosition)
        {
            //FIXME: binary search
            foreach (var l in this)
            {
                if (l.ContainsBufferPosition(bufferPosition))
                    return l;
            }

            // ContainsBufferPosition includes the start position of the line but not the
            // position after the last char on the line, so we have to explicitly handle
            // the case where the caret is after the last char in the buffer
            var last = this.LastOrDefault ();
            if (last != null && bufferPosition == last.EndIncludingLineBreak) {
                return last;
            }

            return null;
        }

        public ITextViewLine GetTextViewLineContainingYCoordinate(double y)
            => null;

        public Collection<ITextViewLine> GetTextViewLinesIntersectingSpan(SnapshotSpan bufferSpan)
            => null;

        public bool IntersectsBufferSpan(SnapshotSpan bufferSpan)
            => false;
    }
}
