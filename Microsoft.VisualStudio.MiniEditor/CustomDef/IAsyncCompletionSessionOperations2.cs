namespace Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion
{
	public interface IAsyncCompletionSessionOperations2 : IAsyncCompletionSessionOperations
	{
		bool CanToggleFilter (string accessKey);
		void ToggleFilter (string accessKey);
	}
}