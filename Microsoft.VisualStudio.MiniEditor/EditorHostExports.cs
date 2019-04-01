using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.MiniEditor
{
	/// <summary>
	/// This class contains substitute exports for instances typically provided by the Visual Studio shell.
	/// This class does not need to be exported. The constructor of this class will be invoked
	/// when any of the provided MEF parts is imported.
	/// </summary>
	public class EditorHostExports
	{
		/// <summary>
		/// Implementation of <see cref="IExtensionErrorHandler"/>.
		/// Visual Studio provides error handler which writes to activity log and displays messages.
		/// This implementation forwards the exception to subscribers of <see cref="ExceptionHandled"/> event
		/// </summary>
		[Export]
		[Export (typeof (IExtensionErrorHandler))]
		public class CustomErrorHandler : IExtensionErrorHandler
		{
			public int ErrorsHandled { get; private set; }
			public event EventHandler<ExceptionEventArgs> ExceptionHandled;

			public void HandleError (object sender, Exception exception)
			{
				ErrorsHandled++;

				if (ExceptionHandled != null)
					ExceptionHandled.Invoke (sender, new ExceptionEventArgs (exception));
			}

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
}
