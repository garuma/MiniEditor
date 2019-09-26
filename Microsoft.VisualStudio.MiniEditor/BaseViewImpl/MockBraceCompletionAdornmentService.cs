using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Text.Editor;

namespace Microsoft.VisualStudio.Text.BraceCompletion.Implementation
{
	class MockBraceCompletionAdornmentService : IBraceCompletionAdornmentService
	{
		public ITrackingPoint Point {
			get => throw new System.NotImplementedException ();
			set { }
		}
	}

	[Export(typeof (IBraceCompletionAdornmentServiceFactory))]
	class MockBraceCompletionAdornmentServiceFactory : IBraceCompletionAdornmentServiceFactory
	{
		IBraceCompletionAdornmentService service = new MockBraceCompletionAdornmentService ();

		public IBraceCompletionAdornmentService GetOrCreateService (ITextView textView) => service;
	}
}
