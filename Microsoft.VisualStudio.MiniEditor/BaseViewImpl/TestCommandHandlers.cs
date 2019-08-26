using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Commanding;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Editor.Commanding.Commands;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.MiniEditor.BaseViewImpl
{
	[Name (PredefinedCompletionNames.CompletionCommandHandler)]
	[ContentType ("text")]
	[TextViewRole (PredefinedTextViewRoles.Interactive)]
	[Export (typeof (ICommandHandler))]
	public class TestCommandHandlers :
		ICommandHandler<TypeCharCommandArgs>,
		ICommandHandler<ReturnKeyCommandArgs>
	{
		[Import]
		IEditorOperationsFactoryService OperationsServiceFactory { get; set; }
		IEditorOperations3 GetOperations (ITextView tv) =>
			(IEditorOperations3) OperationsServiceFactory.GetEditorOperations (tv);

		public string DisplayName => "Test Editor Commands";

		public bool ExecuteCommand (TypeCharCommandArgs args, CommandExecutionContext executionContext)
			=> GetOperations (args.TextView).InsertText (args.TypedChar.ToString ());

		public CommandState GetCommandState (TypeCharCommandArgs args)
			=> args.TypedChar == '\0'? CommandState.Unavailable : CommandState.Available;

		public CommandState GetCommandState (ReturnKeyCommandArgs args)
			=> CommandState.Available;

		public bool ExecuteCommand (ReturnKeyCommandArgs args, CommandExecutionContext executionContext)
			=> GetOperations (args.TextView).InsertNewLine ();
	}
}
