using Corund.Engine;
using Corund.Engine.Config;
using Corund.Engine.Prompts;
using Corund.Platform.Android.Tools;
using Corund.Sound;

namespace Corund.Platform.Android;

/// <summary>
/// Android-specific wrapper provider.
/// </summary>
public class AndroidPlatformAdapter : IPlatformAdapter
{
    public EmbeddedContentProvider GetEmbeddedContentProvider() => new EmbeddedContentProvider(GetType().Assembly, "Corund.Platform.Android.Content.Resources");
    public IAccelerometerManager GetAccelerometerManager() => new AndroidAccelerometerManager();
    public IPromptManager GetPromptManager() => new AndroidPromptManager();
    public IMusicPlayer GetMusicPlayer() => null; // uses default one
}