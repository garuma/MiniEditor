using System;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Editor.Implementation;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.Projection;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.Text.Editor.Implementation
{
	class TestTextView : ITextView
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

		readonly IBufferGraphFactoryService bufferGraphFactory;

		#endregion

		#region Construction
		public TestTextView (ITextBuffer textBuffer, IBufferGraphFactoryService bufferGraphFactory)
		{
			_textBuffer = textBuffer;
			this.bufferGraphFactory = bufferGraphFactory;
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

		public ITextViewLineCollection TextViewLines => throw new NotImplementedException ();

		public ITextSelection Selection => throw new NotImplementedException ();

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

		public ITextViewModel TextViewModel => throw new NotImplementedException ();

		public ITextDataModel TextDataModel => throw new NotImplementedException ();

		public IBufferGraph BufferGraph => bufferGraphFactory.CreateBufferGraph (_textBuffer);

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

		public ITextViewLine GetTextViewLineContainingBufferPosition (SnapshotPoint bufferPosition) => throw new NotImplementedException ();

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region IPropertyOwner Members

		public PropertyCollection Properties { get; } = new PropertyCollection();

		#endregion

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region Internal Surface

		internal void FireClosed () => this.Closed?.Invoke (this, EventArgs.Empty);

		#endregion
	}
}
