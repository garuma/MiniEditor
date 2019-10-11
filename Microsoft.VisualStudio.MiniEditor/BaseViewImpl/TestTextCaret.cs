using System;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.MultiSelection;

namespace Microsoft.VisualStudio.Text.Editor.Implementation
{
	internal class TestTextCaret : ITextCaret
	{
		readonly ITextView textView;
		readonly ISmartIndentationService indentService;
		readonly IMultiSelectionBroker multiSelectionBroker;

		public TestTextCaret (ITextView textView, ISmartIndentationService indentService, IMultiSelectionBroker multiSelectionBroker)
		{
			this.textView = textView;
			this.indentService = indentService;
			this.multiSelectionBroker = multiSelectionBroker;
			multiSelectionBroker.MultiSelectionSessionChanged += MultiSelectionSessionChanged;

			var point = new SnapshotPoint (textView.TextBuffer.CurrentSnapshot, 0);
			Position = new CaretPosition (
				new VirtualSnapshotPoint (point),
				textView.BufferGraph.CreateMappingPoint (point, PointTrackingMode.Positive),
				PositionAffinity.Successor
			);
		}

		#region ITextCaret Members

		public bool IsHidden {
			get => throw new NotImplementedException ();
			set => throw new NotImplementedException ();
		}

		public double Bottom => throw new NotImplementedException ();

		public void CapturePreferredYCoordinate () => throw new NotImplementedException ();

		public ITextViewLine ContainingTextViewLine
			=> textView.GetTextViewLineContainingBufferPosition (Position.BufferPosition);

		public void EnsureVisible () 
		{
			// no-op, we don't do scrolling
		}

		public double Height => throw new NotImplementedException ();

		public double Left => throw new NotImplementedException ();

		public CaretPosition MoveTo (SnapshotPoint bufferPosition)
			=> MoveTo (bufferPosition, PositionAffinity.Successor);

		public CaretPosition MoveTo (SnapshotPoint bufferPosition, PositionAffinity caretAffinity)
			=> MoveTo (bufferPosition, caretAffinity, true);

		public CaretPosition MoveTo (SnapshotPoint bufferPosition, PositionAffinity caretAffinity, bool captureHorizontalPosition)
			=> MoveTo (new VirtualSnapshotPoint (bufferPosition), caretAffinity, captureHorizontalPosition);

		public CaretPosition MoveTo (ITextViewLine textLine)
			=> MoveTo (textLine, 0.0, true);

		public CaretPosition MoveTo (ITextViewLine textLine, double xCoordinate)
			=> MoveTo (textLine, xCoordinate, true);

		public CaretPosition MoveTo (ITextViewLine textLine, double xCoordinate, bool captureHorizontalPosition)
		{
			var xCoord = textLine.MapXCoordinate (textView, 0.0, indentService, false);
			var pos = textLine.GetInsertionBufferPositionFromXCoordinate (xCoord);
			return MoveTo (pos, PositionAffinity.Successor, captureHorizontalPosition);
		}

		public CaretPosition MoveTo (VirtualSnapshotPoint position)
			=> MoveTo (position, PositionAffinity.Successor, true);

		public CaretPosition MoveTo (VirtualSnapshotPoint position, PositionAffinity caretAffinity)
			=> MoveTo (position, caretAffinity, true);

		public CaretPosition MoveTo (VirtualSnapshotPoint position, PositionAffinity caretAffinity, bool captureHorizontalPosition)
		{
			multiSelectionBroker.SetSelection (new Selection (position, caretAffinity));
			return Position;
		}

		public CaretPosition MoveToNextCaretPosition ()
		{
			throw new NotImplementedException ();
		}

		public CaretPosition MoveToPreviousCaretPosition ()
		{
			throw new NotImplementedException ();
		}

		public bool OverwriteMode { get; set; }

		public bool InVirtualSpace {
			get {
				return false;
			}
		}

		public CaretPosition Position { get; private set; }

		void MultiSelectionSessionChanged (object sender, EventArgs e)
		{
			Update ();
		}

		public void Update ()
		{
			var oldPosition = Position;

			var snapshot = textView.TextBuffer.CurrentSnapshot;
			bool snapshotChanged = snapshot != oldPosition.BufferPosition.Snapshot;
			var translatedBrokerPos = multiSelectionBroker.PrimarySelection.InsertionPoint.TranslateTo (snapshot);
			var posChanged = oldPosition.VirtualBufferPosition.TranslateTo (snapshot) != translatedBrokerPos;
			var affinityChanged = multiSelectionBroker.PrimarySelection.InsertionPointAffinity != oldPosition.Affinity;

			if (snapshotChanged || posChanged || affinityChanged) {
				Position = new CaretPosition (
					translatedBrokerPos,
					textView.BufferGraph.CreateMappingPoint (translatedBrokerPos.Position, PointTrackingMode.Positive),
					multiSelectionBroker.PrimarySelection.InsertionPointAffinity
				);
			}

			if (posChanged) {
				PositionChanged?.Invoke (this, new CaretPositionChangedEventArgs (this.textView, oldPosition, Position));
			}
		}

		public event EventHandler<CaretPositionChangedEventArgs> PositionChanged;

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
