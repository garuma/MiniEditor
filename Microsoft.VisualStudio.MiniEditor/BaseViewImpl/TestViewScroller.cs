namespace Microsoft.VisualStudio.Text.Editor.Implementation
{
	internal class TestViewScroller : IViewScroller
	{
		public static readonly TestViewScroller Instance = new TestViewScroller();

		public void EnsureSpanVisible(SnapshotSpan span)
		{
		}

		public void EnsureSpanVisible(SnapshotSpan span, EnsureSpanVisibleOptions options)
		{
		}

		public void EnsureSpanVisible(VirtualSnapshotSpan span, EnsureSpanVisibleOptions options)
		{
		}

		public void ScrollViewportHorizontallyByPixels(double distanceToScroll)
		{
		}

		public void ScrollViewportVerticallyByLine(ScrollDirection direction)
		{
		}

		public void ScrollViewportVerticallyByLines(ScrollDirection direction, int count)
		{
		}

		public bool ScrollViewportVerticallyByPage(ScrollDirection direction) => true;

		public void ScrollViewportVerticallyByPixels(double distanceToScroll)
		{
		}
	}
}