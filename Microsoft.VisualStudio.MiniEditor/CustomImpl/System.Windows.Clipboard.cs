using System;
using System.Linq;

namespace System.Windows
{
    static class Clipboard
    {
		static IDataObject dataObject;

		public static IDataObject GetDataObject () => dataObject;

		public static void SetDataObject (IDataObject data, bool copy)
			=> dataObject = data;

		public static bool ContainsText () =>
			dataObject.GetFormats ()?.Any (f => f == DataFormats.UnicodeText) ?? false;
	}
}
