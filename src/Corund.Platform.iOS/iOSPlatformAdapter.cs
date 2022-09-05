using Corund.Engine;
using Corund.Engine.Config;
using Corund.Engine.Prompts;
using Corund.Platform.iOS.Tools;

namespace Corund.Platform.iOS;

/// <summary>
/// IOS-specific wrapper provider.
/// </summary>
public class iOSPlatformAdapter : IPlatformAdapter
{
    public IContentProvider GetEmbeddedContentProvider() => new EmbeddedContentProvider(GetType().Assembly, "Corund.Platform.iOS.Content.Resources");
    public IAccelerometerManager GetAccelerometerManager() => new iOSAccelerometerManager();
    public IPromptManager GetPromptManager() => new iOSPromptManager();
}