using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.MiniEditor
{
	/// <summary>
	/// Implementation of <see cref="IExtensionErrorHandler"/>.
	/// Visual Studio provides error handler which writes to activity log and displays messages.
	/// This implementation forwards the exception to subscribers of <see cref="ExceptionHandled"/> event
	/// </summary>
	[Export (typeof (IExtensionErrorHandler))]
	public class CustomErrorHandler : IExtensionErrorHandler
	{
		// GuardedOperations imports IExtensionErrorHandler via Lazy<IExtensionErrorHandler> and
		// hence gets its own private instance. to access the event from the host we have to make it static
		public static event EventHandler<ExceptionEventArgs> ExceptionHandled;

		public void HandleError (object sender, Exception exception)
			=> ExceptionHandled?.Invoke (sender, new ExceptionEventArgs (exception));

		public class ExceptionEventArgs : EventArgs
		{
			public Exception Exception { get; }
			public ExceptionEventArgs (Exception ex)
			{
				Exception = ex;
			}
		}
	}
}
