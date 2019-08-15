using System;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Editor.Implementation;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.Projection;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Microsoft.VisualStudio.Text.Editor.Implementation
{
	class TestTextView : ITextView3
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region Private Data Members

		private EventHandler<TextViewLayoutChangedEventArgs> _layoutChanged;
		private EventHandler<MouseHoverEventArgs> _mouseHover;
		private EventHandler _viewportWidthChanged;
		private EventHandler _viewportLeftChanged;
		private EventHandler _viewportHeightChanged;
		private bool _isClosed;

		private ITextBuffer _textBuffer;
		private IMultiSelectionBroker _multiSelectionBroker;
		private ITextViewLineCollection _textViewLines;
		private ITextViewModel _textViewModel;
		private ITextDataModel _textDataModel;

		readonly TextViewFactoryService _factoryService;

		#endregion

		#region Construction
		public TestTextView (ITextBuffer textBuffer, TextViewFactoryService factoryService)
		{
			_textBuffer = textBuffer;
			_factoryService = factoryService;
			Selection = new MockTextSelection(this);

			var listeners = UIExtensionSelector.SelectMatchingExtensions (
				_factoryService.TextViewCreationListeners, _textBuffer.ContentType, null, Roles);
			foreach (var listener in listeners) {
				listener.Value.TextViewCreated (this);
			}

			_textDataModel = new VacuousTextDataModel (textBuffer);
			_textViewModel = new VacuousTextViewModel (_textDataModel);
			_multiSelectionBroker = _factoryService.MultiSelectionBrokerFactory.CreateBroker (this);

			_textViewLines = new TestTextViewLineCollection (this);
		}
		#endregion

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region ITextView Members

		public event EventHandler<TextViewLayoutChangedEventArgs> LayoutChanged {
			add { _layoutChanged += value; }
			remove { _layoutChanged -= value; }
		}

		public event EventHandler<MouseHoverEventArgs> MouseHover {
			add { _mouseHover += value; }
			remove { _mouseHover -= value; }
		}

		public event EventHandler ViewportWidthChanged {
			add { _viewportWidthChanged += value; }
			remove { _viewportWidthChanged -= value; }
		}

		public event EventHandler ViewportLeftChanged {
			add { _viewportLeftChanged += value; }
			remove { _viewportLeftChanged -= value; }
		}

		public event EventHandler ViewportHeightChanged {
			add { _viewportHeightChanged += value; }
			remove { _viewportHeightChanged -= value; }
		}

		public ITextCaret Caret => new MockTextCaret (this);

		public void DisplayTextLineContainingBufferPosition (SnapshotPoint position, double verticalDistance, ViewRelativePosition relativeTo) => throw new NotImplementedException ();

		public void DisplayTextLineContainingBufferPosition (SnapshotPoint position, double verticalDistance, ViewRelativePosition relativeTo, double? width, double? height) => throw new NotImplementedException ();

		public SnapshotSpan GetTextElementSpan (SnapshotPoint position) => throw new NotImplementedException ();

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

		public ITextViewLineCollection TextViewLines => _textViewLines;

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

		public ITextViewModel TextViewModel => _textViewModel;

		public ITextDataModel TextDataModel => _textDataModel;

		public IBufferGraph BufferGraph => _factoryService.BufferGraphFactoryService.CreateBufferGraph (_textBuffer);

		public IViewScroller ViewScroller => throw new NotImplementedException ();

		public double ViewportBottom => throw new NotImplementedException ();

		public double ViewportHeight => throw new NotImplementedException ();

		public double LineHeight => throw new NotImplementedException ();

		public double ViewportLeft {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public double ViewportRight => throw new NotImplementedException ();

		public double ViewportTop => throw new NotImplementedException ();

		public double ViewportWidth => throw new NotImplementedException ();

		public double ZoomLevel {
			get { throw new NotImplementedException (); }
			set { throw new NotImplementedException (); }
		}

		public void Close ()
		{
			_isClosed = true;
			Closed?.Invoke (this, EventArgs.Empty);
		}

		public event EventHandler Closed;
		public event EventHandler IsKeyboardFocusedChanged;
		public event EventHandler MaxTextRightCoordinateChanged;

		public bool IsClosed => _isClosed;

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
			=> _textViewLines.GetTextViewLineContainingBufferPosition (bufferPosition);

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region IPropertyOwner Members

		public PropertyCollection Properties { get; } = new PropertyCollection();

		public ITextViewLineSource FormattedLineSource => throw new NotImplementedException ();

		public bool IsKeyboardFocused => throw new NotImplementedException ();

		public bool InOuterLayout => throw new NotImplementedException ();

		public IMultiSelectionBroker MultiSelectionBroker => _multiSelectionBroker;

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

	class TestTextViewLineCollection : List<ITextViewLine>, ITextViewLineCollection
	{
		private TestTextView testTextView;

		public TestTextViewLineCollection (TestTextView testTextView)
		{
			this.testTextView = testTextView;
		}

		public ITextViewLine FirstVisibleLine => this.FirstOrDefault ();

		public ITextViewLine LastVisibleLine => this.LastOrDefault ();

		public SnapshotSpan FormattedSpan => throw new NotImplementedException ();

		public bool IsValid => true;

		public bool ContainsBufferPosition (SnapshotPoint bufferPosition)
			=> false;

		public TextBounds GetCharacterBounds (SnapshotPoint bufferPosition)
		{
			throw new NotImplementedException ();
		}

		public int GetIndexOfTextLine (ITextViewLine textLine)
			=> -1;

		public Collection<TextBounds> GetNormalizedTextBounds (SnapshotSpan bufferSpan)
		{
			throw new NotImplementedException ();
		}

		public SnapshotSpan GetTextElementSpan (SnapshotPoint bufferPosition)
		{
			throw new NotImplementedException ();
		}

		public ITextViewLine GetTextViewLineContainingBufferPosition (SnapshotPoint bufferPosition)
			=> null;

		public ITextViewLine GetTextViewLineContainingYCoordinate (double y)
			=> null;

		public Collection<ITextViewLine> GetTextViewLinesIntersectingSpan (SnapshotSpan bufferSpan)
			=> null;

		public bool IntersectsBufferSpan (SnapshotSpan bufferSpan)
			=> false;
	}
}
