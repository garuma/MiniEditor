using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.MiniEditor.BaseViewImpl
{
	[Export (typeof (ITextStructureNavigatorProvider))]
	[ContentType ("any")]
	class MockTextStructureNavigatorProvider : ITextStructureNavigatorProvider
	{
		public ITextStructureNavigator CreateTextStructureNavigator (ITextBuffer textBuffer)
			=> new MockTextStructureNavigator (textBuffer);
	}

	class MockTextStructureNavigator : ITextStructureNavigator
	{
		readonly ITextBuffer textBuffer;

		public MockTextStructureNavigator (ITextBuffer textBuffer)
		{
			this.textBuffer = textBuffer;
		}

		public IContentType ContentType => textBuffer.ContentType;

		public TextExtent GetExtentOfWord (SnapshotPoint currentPosition)
		{
			throw new NotImplementedException ();
		}

		public SnapshotSpan GetSpanOfEnclosing (SnapshotSpan activeSpan)
		{
			throw new NotImplementedException ();
		}

		public SnapshotSpan GetSpanOfFirstChild (SnapshotSpan activeSpan)
		{
			throw new NotImplementedException ();
		}

		public SnapshotSpan GetSpanOfNextSibling (SnapshotSpan activeSpan)
		{
			throw new NotImplementedException ();
		}

		public SnapshotSpan GetSpanOfPreviousSibling (SnapshotSpan activeSpan)
		{
			throw new NotImplementedException ();
		}
	}
}
