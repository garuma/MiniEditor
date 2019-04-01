using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Projection;

namespace Microsoft.VisualStudio.Text.Editor.Implementation
{
	[Export (typeof (ITextViewFactoryService))]
	public class TextViewFactoryService : ITextViewFactoryService
	{
		[Import]
		public ITextBufferFactoryService BufferFactoryService { get; set; }

		[Import]
		public IBufferGraphFactoryService BufferGraphFactoryService { get; set; }

		public ITextView CreateTextView (ITextBuffer buffer)
			=> new TestTextView (buffer, BufferGraphFactoryService);

		public ITextView CreateTextView () => CreateTextView (BufferFactoryService.CreateTextBuffer ());
	}
}
