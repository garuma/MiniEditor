using System;
using Microsoft.VisualStudio.Text.Formatting;
using System.Collections.ObjectModel;

namespace Microsoft.VisualStudio.Text.Editor.Implementation
{
    class TestTextViewLine : ITextViewLine
    {
        const double padding = 0.2;

        readonly ITextSnapshotLine bufferLine;
        readonly TestTextView view;

        public TestTextViewLine(TestTextView view, ITextSnapshotLine bufferLine, double left, double top, double right, double bottom)
        {
            Left = left;
            Right = right;
            Bottom = bottom;
            Top = top;
            this.bufferLine = bufferLine;
        }

        public object IdentityTag => this;

        public ITextSnapshot Snapshot => bufferLine.Snapshot;

        public bool IsFirstTextViewLineForSnapshotLine => true;

        public bool IsLastTextViewLineForSnapshotLine => true;

        public double Baseline => throw new NotImplementedException();

        public SnapshotSpan Extent => new SnapshotSpan(bufferLine.Start, bufferLine.End);

        IMappingSpan extentAsMappingSpan;
        public IMappingSpan ExtentAsMappingSpan => extentAsMappingSpan ??
            (extentAsMappingSpan = view.BufferGraph.CreateMappingSpan(Extent, SpanTrackingMode.EdgeInclusive));

        public SnapshotSpan ExtentIncludingLineBreak => new SnapshotSpan(bufferLine.Start, bufferLine.EndIncludingLineBreak);

        IMappingSpan extentIncludingLineBreakAsMappingSpan;
        public IMappingSpan ExtentIncludingLineBreakAsMappingSpan => extentIncludingLineBreakAsMappingSpan ??
            (extentIncludingLineBreakAsMappingSpan = view.BufferGraph.CreateMappingSpan(ExtentIncludingLineBreak, SpanTrackingMode.EdgeInclusive));

        public SnapshotPoint Start => bufferLine.Start;

        public int Length => bufferLine.Length;

        public int LengthIncludingLineBreak => bufferLine.LengthIncludingLineBreak;

        public SnapshotPoint End => bufferLine.End;

        public SnapshotPoint EndIncludingLineBreak => bufferLine.EndIncludingLineBreak;

        public int LineBreakLength => bufferLine.LineBreakLength;

        public double Left { get; }

        public double Top { get; }

        public double Height => Bottom - Top;

        public double TextTop => Top + padding;

        public double TextBottom => Bottom - padding;

        public double TextHeight => Bottom - Top - padding - padding;

        public double TextLeft => Left + padding;

        //FIXME
        public double TextRight => Right - padding;

        //FIXME
        public double TextWidth => Right - Left - padding - padding;

        public double Width => Right - Left;

        public double Bottom { get; }

        public double Right { get; }

        public double EndOfLineWidth => 0;

        public double VirtualSpaceWidth => 0;

        public bool IsValid => true;

        public LineTransform LineTransform => default;

        public LineTransform DefaultLineTransform => default;

        public VisibilityState VisibilityState => VisibilityState.FullyVisible;

        public double DeltaY => throw new NotImplementedException();

        public TextViewLineChange Change => throw new NotImplementedException();

        public bool ContainsBufferPosition(SnapshotPoint bufferPosition) => ExtentIncludingLineBreak.Contains(bufferPosition);

        public TextBounds? GetAdornmentBounds(object identityTag) => throw new NotImplementedException();

        public ReadOnlyCollection<object> GetAdornmentTags(object providerTag) => throw new NotImplementedException();

        public SnapshotPoint? GetBufferPositionFromXCoordinate(double xCoordinate, bool textOnly)
        {
            throw new NotImplementedException();
        }

        public SnapshotPoint? GetBufferPositionFromXCoordinate(double xCoordinate)
        {
            throw new NotImplementedException();
        }

        public TextBounds GetCharacterBounds(SnapshotPoint bufferPosition)
			=> GetCharacterBounds (new VirtualSnapshotPoint (bufferPosition));

        public TextBounds GetCharacterBounds(VirtualSnapshotPoint bufferPosition)
		{
			var col = bufferPosition.Position - bufferLine.Start;

			//pretend each character is a perfect square
			return new TextBounds (Left + col * Height, Top, Height, Height, Top, Height);
        }

        public TextBounds GetExtendedCharacterBounds(SnapshotPoint bufferPosition)
			=> GetCharacterBounds (bufferPosition);

		public TextBounds GetExtendedCharacterBounds (VirtualSnapshotPoint bufferPosition)
			=> GetCharacterBounds (bufferPosition);

        public VirtualSnapshotPoint GetInsertionBufferPositionFromXCoordinate(double xCoordinate)
        {
            throw new NotImplementedException();
        }

        public Collection<TextBounds> GetNormalizedTextBounds(SnapshotSpan bufferSpan)
        {
            throw new NotImplementedException();
        }

        //HACK just return the line as we don't currently split it up into elements
        public SnapshotSpan GetTextElementSpan(SnapshotPoint bufferPosition) => Extent;

        public VirtualSnapshotPoint GetVirtualBufferPositionFromXCoordinate(double xCoordinate)
        {
            throw new NotImplementedException();
        }

        public bool IntersectsBufferSpan(SnapshotSpan bufferSpan) => Extent.IntersectsWith(bufferSpan);
    }
}
