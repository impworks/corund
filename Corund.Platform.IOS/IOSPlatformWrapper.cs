using Corund.Engine.Config;
using Corund.Platform.IOS.Content;
using Corund.Platform.IOS.Input;

namespace Corund.Platform.IOS
{
    /// <summary>
    /// IOS-specific wrapper provider.
    /// </summary>
    public class UWPPlatformAdapter : IPlatformAdapter
    {
        public IContentProvider GetEmbeddedContentProvider() => new IOSContentProvider();
        public IAccelerometerManager GetAccelerometerManager() => new IOSAccelerometerManager();
    }
}
