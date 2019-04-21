using Corund.Engine.Config;
using Corund.Platform.UWP.Content;
using Corund.Platform.UWP.Input;

namespace Corund.Platform.UWP
{
    /// <summary>
    /// UWP-specific wrapper provider.
    /// </summary>
    public class UWPPlatformAdapter : IPlatformAdapter
    {
        public IContentProvider GetEmbeddedContentProvider() => new UWPContentProvider();
        public IAccelerometerManager GetAccelerometerManager() => new UWPAccelerometerManager();
    }
}
