using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Adornments;

namespace Microsoft.VisualStudio.Text.Editor.Implementation
{
    [Export(typeof(IToolTipService))]
    class MockTooltipService : IToolTipService
    {
        public IToolTipPresenter CreatePresenter(ITextView textView, ToolTipParameters parameters = null)
        {
            return new MockToolTipPresenter();
        }
    }

    class MockToolTipPresenter : IToolTipPresenter
    {
        public event EventHandler Dismissed;

        public void Dismiss()
        {
            Dismissed?.Invoke(this, EventArgs.Empty);
        }

        public void StartOrUpdate(ITrackingSpan applicableToSpan, IEnumerable<object> content)
        {
        }
    }
}
