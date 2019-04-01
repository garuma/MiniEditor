using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.MiniEditor
{
	public interface IFileSystemAbstraction
	{
		Stream OpenFile (string filePath, out DateTime lastModifiedTimeUtc, out long fileSize);
		void PerformSave (ITextSnapshot textSnapshot, FileMode fileMode, string filePath, Encoding encoding, bool createFolder);
	}
}
