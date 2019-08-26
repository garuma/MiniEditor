using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Projection;
using Microsoft.VisualStudio.Text.Utilities;

namespace Microsoft.VisualStudio.Text.Editor.Implementation
{
	[Export (typeof (ITextViewFactoryService))]
	public class TextViewFactoryService : ITextViewFactoryService
	{
		[Import]
		public ITextBufferFactoryService BufferFactoryService { get; set; }

		[Import]
		public IBufferGraphFactoryService BufferGraphFactoryService { get; set; }

		[Import]
		public IMultiSelectionBrokerFactory MultiSelectionBrokerFactory { get; set; }

		[Import]
		public ISmartIndentationService SmartIndentationService { get; set; }

		[Import]
		public IEditorOptionsFactoryService2 EditorOptionsFactory { get; set; }

		[ImportMany]
		public List<Lazy<ITextViewCreationListener, IDeferrableContentTypeAndTextViewRoleMetadata>> TextViewCreationListeners { get; set; }

		public ITextView CreateTextView (ITextBuffer buffer)
			=> new TestTextView (buffer, this);

		public ITextView CreateTextView () => CreateTextView (BufferFactoryService.CreateTextBuffer ());
	}
}
