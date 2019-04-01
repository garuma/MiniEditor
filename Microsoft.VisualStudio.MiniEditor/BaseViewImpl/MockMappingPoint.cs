using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Projection;

namespace Microsoft.VisualStudio.Text.Editor.Implementation
{
	internal class MockMappingPoint : IMappingPoint
	{
		private int position;
		private ITextSnapshot snapshot;

		public MockMappingPoint (ITextBuffer textBuffer, int position)
		{
			this.position = position;
			this.snapshot = textBuffer.CurrentSnapshot;
		}

		public SnapshotPoint? GetPoint (Predicate<ITextBuffer> match, PositionAffinity affinity)
		{
			return new SnapshotPoint (this.snapshot, this.position);
		}

		public SnapshotPoint? GetPoint (ITextBuffer textBuffer, PositionAffinity affinity)
		{
			return new SnapshotPoint (this.snapshot, this.position);
		}

		public SnapshotPoint? GetPoint (ITextSnapshot textSnapshot, PositionAffinity affinity)
		{
			return new SnapshotPoint (this.snapshot, this.position);
		}

		public SnapshotPoint? GetInsertionPoint (Predicate<ITextBuffer> match)
		{
			return new SnapshotPoint (this.snapshot, this.position);
		}

		public ITextBuffer AnchorBuffer {
			get { throw new NotImplementedException (); }
		}

		public IBufferGraph BufferGraph {
			get { throw new NotImplementedException (); }
		}
	}
}
