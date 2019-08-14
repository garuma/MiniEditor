using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.MiniEditor.BaseViewImpl
{
	[Export (typeof (ISmartIndentationService))]
	class MockSmartIndentationService : ISmartIndentationService, ISmartIndent
	{
		[ImportMany]
		internal List<Lazy<ISmartIndentProvider, IContentTypeMetadata>> Providers { get; set; }

		[Import]
		internal IGuardedOperations GuardedOperations { get; set; }

		[Import]
		internal IContentTypeRegistryService ContentTypeRegistry { get; set; }

		public int? GetDesiredIndentation (ITextView textView, ITextSnapshotLine line)
		{
			var indenter = textView.Properties
				.GetOrCreateSingletonProperty (typeof (MockSmartIndentationService), () =>
					GuardedOperations.InvokeBestMatchingFactory (
						Providers,
						textView.TextBuffer.ContentType,
						p => p.CreateSmartIndent (textView),
						ContentTypeRegistry,
						this)
					?? this
			);
			return indenter.GetDesiredIndentation (line);
		}

		// null implementation so we can always return one
		int? ISmartIndent.GetDesiredIndentation (ITextSnapshotLine line) => null;
		void IDisposable.Dispose () {}
	}
}
