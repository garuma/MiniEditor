using System;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.Projection;
using Microsoft.VisualStudio.Text.Utilities;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.Text.Editor.Implementation
{
	class TestTextView : ITextView3
	{
		//we pretend each char is a simple square
		const double charSize = 20;

		//enormous viewport makes things simpler
		const double viewportSize = 20000.0;

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region Private Data Members


		private ITextBuffer _textBuffer;
		readonly TextViewFactoryService _factoryService;

		#endregion

		#region Construction
		public TestTextView (ITextBuffer textBuffer, TextViewFactoryService factoryService)
		{
			_textBuffer = textBuffer;
			_factoryService = factoryService;

			var listeners = UIExtensionSelector.SelectMatchingExtensions (
				_factoryService.TextViewCreationListeners, _textBuffer.ContentType, null, Roles);
			foreach (var listener in listeners) {
				listener.Value.TextViewCreated (this);
			}

			TextDataModel = new VacuousTextDataModel (textBuffer);
			TextViewModel = new VacuousTextViewModel (TextDataModel);
			MultiSelectionBroker = _factoryService.MultiSelectionBrokerFactory.CreateBroker (this);

			CreateLines ();

			Selection = new MockTextSelection (this, MultiSelectionBroker);
			Caret = new MockTextCaret (this, _factoryService.SmartIndentationService, MultiSelectionBroker);
		}
		#endregion

		void CreateLines ()
		{
			ITextSnapshot snapshot = TextBuffer.CurrentSnapshot;
			int topLine = 0;
			int bottomLine = Math.Min (snapshot.LineCount - 1, 20);

			var lines = new TestTextViewLineCollection (this);
			for (int i = topLine; i <= bottomLine; i++) {
				var l = snapshot.GetLineFromLineNumber (i);
				double top = charSize * i;
				var line = new TestTextViewLine (this, l, 0, top, l.Length * charSize, top + LineHeight);
				lines.Add (line);
			}

			TextViewLines = lines;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region ITextView Members

		public event EventHandler<TextViewLayoutChangedEventArgs> LayoutChanged;

		public event EventHandler<MouseHoverEventArgs> MouseHover;

		public event EventHandler ViewportWidthChanged;

		public event EventHandler ViewportLeftChanged;

		public event EventHandler ViewportHeightChanged;

		public ITextCaret Caret { get; }

		public void DisplayTextLineContainingBufferPosition (SnapshotPoint position, double verticalDistance, ViewRelativePosition relativeTo) => throw new NotImplementedException ();

		public void DisplayTextLineContainingBufferPosition (SnapshotPoint position, double verticalDistance, ViewRelativePosition relativeTo, double? width, double? height) => throw new NotImplementedException ();

		public SnapshotSpan GetTextElementSpan (SnapshotPoint position) => TextViewLines.GetTextElementSpan (position);

		public bool InLayout => throw new NotImplementedException ();

		public double MaxTextRightCoordinate => throw new NotImplementedException ();

		public ITrackingSpan ProvisionalTextHighlight {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

        public ITextViewLineCollection TextViewLines { get; private set; }

        public ITextSelection Selection { get; }

		public int TabSize {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public ITextViewRoleSet Roles => new TextViewRoleSet (new string[] {
			PredefinedTextViewRoles.Analyzable,
			PredefinedTextViewRoles.Document,
			PredefinedTextViewRoles.Editable,
			PredefinedTextViewRoles.Interactive,
			PredefinedTextViewRoles.Structured,
			PredefinedTextViewRoles.Zoomable
		});

		public ITextBuffer TextBuffer => (_textBuffer);

		public ITextSnapshot TextSnapshot => TextBuffer.CurrentSnapshot;

		public ITextSnapshot VisualSnapshot => TextBuffer.CurrentSnapshot;

        public ITextViewModel TextViewModel { get; }

        public ITextDataModel TextDataModel { get; }

        public IBufferGraph BufferGraph => _factoryService.BufferGraphFactoryService.CreateBufferGraph (_textBuffer);

		public IViewScroller ViewScroller => throw new NotImplementedException ();

		public double ViewportBottom => ViewportTop + ViewportHeight;

		public double ViewportHeight => viewportSize;

		public double LineHeight => charSize;

		public double ViewportLeft {
			get => 0.0;
			set => throw new NotImplementedException ();
		}

		public double ViewportRight => ViewportLeft + ViewportWidth;

		public double ViewportTop => 0.0;

		public double ViewportWidth => viewportSize;

		public double ZoomLevel {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
		}

		public void Close ()
		{
			IsClosed = true;
			Closed?.Invoke (this, EventArgs.Empty);
		}

		public event EventHandler Closed;
		public event EventHandler IsKeyboardFocusedChanged;
		public event EventHandler MaxTextRightCoordinateChanged;

        public bool IsClosed { get; private set; }

        public IEditorOptions Options => throw new NotImplementedException ();

		public bool IsMouseOverViewOrAdornments => throw new NotImplementedException ();

		public bool HasAggregateFocus => throw new NotImplementedException ();

		public event EventHandler LostAggregateFocus {
			add { }
			remove { }
		}

		public event EventHandler GotAggregateFocus {
			add { }
			remove { }
		}

		public void QueueSpaceReservationStackRefresh () => throw new NotImplementedException ();
		#endregion

		public ITextViewLine GetTextViewLineContainingBufferPosition (SnapshotPoint bufferPosition)
			=> TextViewLines.GetTextViewLineContainingBufferPosition (bufferPosition);

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region IPropertyOwner Members

		public PropertyCollection Properties { get; } = new PropertyCollection();

		public ITextViewLineSource FormattedLineSource => throw new NotImplementedException ();

		public bool IsKeyboardFocused => throw new NotImplementedException ();

		public bool InOuterLayout => throw new NotImplementedException ();

        public IMultiSelectionBroker MultiSelectionBroker { get; }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Internal Surface

        internal void FireClosed () => this.Closed?.Invoke (this, EventArgs.Empty);

		public IXPlatAdornmentLayer GetXPlatAdornmentLayer (string name)
		{
			throw new NotImplementedException ();
		}

		public void Focus ()
		{
			throw new NotImplementedException ();
		}

		public void QueuePostLayoutAction (Action action)
		{
			throw new NotImplementedException ();
		}

		public bool TryGetTextViewLines (out ITextViewLineCollection textViewLines)
		{
			throw new NotImplementedException ();
		}

		public bool TryGetTextViewLineContainingBufferPosition (SnapshotPoint bufferPosition, out ITextViewLine textViewLine)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}
