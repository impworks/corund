using Corund.Engine.Config;
using Corund.Platform.Android.Content;
using Corund.Platform.Android.Input;

namespace Corund.Platform.Android
{
    /// <summary>
    /// Android-specific wrapper provider.
    /// </summary>
    public class AndroidPlatformAdapter : IPlatformAdapter
    {
        public IContentProvider GetEmbeddedContentProvider() => new AndroidContentProvider();
        public IAccelerometerManager GetAccelerometerManager() => new AndroidAccelerometerManager();
    }
}