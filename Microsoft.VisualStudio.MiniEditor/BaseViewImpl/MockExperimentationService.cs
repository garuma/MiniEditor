using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Utilities;

namespace Microsoft.VisualStudio.MiniEditor
{
	[Export (typeof (IExperimentationServiceInternal))]
	class MockExperimentationService : IExperimentationServiceInternal
	{
		public bool IsCachedFlightEnabled (string flightName) => true;
	}
}
