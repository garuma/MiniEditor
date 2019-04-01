using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.MiniEditor.BaseViewImpl
{
	[Export (typeof (IFeatureServiceFactory))]
	class MockFeatureServiceFactory : IFeatureServiceFactory
	{
		public IFeatureService GlobalFeatureService { get; } = new MockFeatureService ();

		public IFeatureService GetOrCreate (IPropertyOwner scope) => GlobalFeatureService;

		class MockFeatureService : IFeatureService
		{
			public event EventHandler<FeatureUpdatedEventArgs> StateUpdated;

			public IFeatureDisableToken Disable (string featureName, IFeatureController controller)
			{
				throw new NotImplementedException ();
			}

			public IFeatureCookie GetCookie (string featureName)
				=> new MockFeatureCookie (featureName);

			public bool IsEnabled (string featureName) => true;
		}

		class MockFeatureCookie : IFeatureCookie
		{
			public MockFeatureCookie (string featureName) => FeatureName = featureName;

			public bool IsEnabled => true;

			public string FeatureName { get; }

			public event EventHandler<FeatureChangedEventArgs> StateChanged;
		}
	}
}
