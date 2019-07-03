using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.Commanding
{
    /// <summary>
    /// A command handler that can opt out of <see cref="ICommandHandler{T}.ExecuteCommand(T, CommandExecutionContext)"/>.
    /// </summary>
    internal interface IDynamicCommandHandler<T> where T : CommandArgs
    {
        /// <summary>
        /// Determines whether <see cref="ICommandHandler{T}.ExecuteCommand(T, CommandExecutionContext)"/> should be called.
        /// </summary>
        bool CanExecuteCommand(T args);
    }
}
