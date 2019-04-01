using System;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Microsoft.VisualStudio.Text.Editor.Implementation
{
	internal class MockTextCaret : ITextCaret
	{
		private ITextView textView;

		public MockTextCaret (ITextView textView)
		{
			this.textView = textView;
		}

		#region ITextCaret Members

		public bool IsHidden {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public double Bottom {
			get { throw new NotImplementedException (); }
		}

		public void CapturePreferredYCoordinate ()
		{
			throw new NotImplementedException ();
		}

		public ITextViewLine ContainingTextViewLine {
			get { throw new NotImplementedException (); }
		}

		public void EnsureVisible ()
		{
			throw new NotImplementedException ();
		}

		public double Height {
			get { throw new NotImplementedException (); }
		}

		public double Left {
			get { throw new NotImplementedException (); }
		}

		public CaretPosition MoveTo (VirtualSnapshotPoint position)
		{
			throw new NotImplementedException ();
		}

		public CaretPosition MoveTo (VirtualSnapshotPoint position, PositionAffinity caretAffinity)
		{
			throw new NotImplementedException ();
		}

		public CaretPosition MoveTo (VirtualSnapshotPoint position, PositionAffinity caretAffinity, bool captureHorizontalPosition)
		{
			throw new NotImplementedException ();
		}

		public CaretPosition MoveTo (SnapshotPoint bufferPosition)
		{
			throw new NotImplementedException ();
		}

		public CaretPosition MoveTo (SnapshotPoint bufferPosition, PositionAffinity caretAffinity)
		{
			throw new NotImplementedException ();
		}

		public CaretPosition MoveTo (ITextViewLine textLine, double xCoordinate)
		{
			throw new NotImplementedException ();
		}

		public CaretPosition MoveTo (SnapshotPoint bufferPosition, PositionAffinity caretAffinity, bool captureHorizontalPosition)
		{
			throw new NotImplementedException ();
		}

		public CaretPosition MoveTo (ITextViewLine textLine, double xCoordinate, bool captureHorizontalPosition)
		{
			throw new NotImplementedException ();
		}

		public CaretPosition MoveTo (ITextViewLine textLine)
		{
			throw new NotImplementedException ();
		}

		public CaretPosition MoveToNextCaretPosition ()
		{
			throw new NotImplementedException ();
		}

		public CaretPosition MoveToPreviousCaretPosition ()
		{
			throw new NotImplementedException ();
		}

		public bool OverwriteMode {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool InVirtualSpace {
			get {
				return false;
			}
		}

		public CaretPosition Position {
			get { return (new CaretPosition (new VirtualSnapshotPoint (this.textView.TextSnapshot, 0), new MockMappingPoint (this.textView.TextBuffer, 0), PositionAffinity.Predecessor)); }
		}

		public event EventHandler<CaretPositionChangedEventArgs> PositionChanged {
			add { }
			remove { }
		}

		public double PreferredYCoordinate {
			get { throw new NotImplementedException (); }
		}

		public double Right {
			get { throw new NotImplementedException (); }
		}

		public double Top {
			get { throw new NotImplementedException (); }
		}

		public double Width {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public CaretPosition MoveToPreferredCoordinates ()
		{
			throw new NotImplementedException ();
		}

		#endregion

	}
}
