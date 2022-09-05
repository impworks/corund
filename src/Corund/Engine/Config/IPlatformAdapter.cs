using Corund.Engine.Prompts;

namespace Corund.Engine.Config;

/// <summary>
/// Interface for adapters that wrap platform-specific logic in a unified interface.
/// </summary>
public interface IPlatformAdapter
{
    /// <summary>
    /// Returns the content provider that returns default resources (shaders, fonts, etc).
    /// </summary>
    IContentProvider GetEmbeddedContentProvider();

    /// <summary>
    /// Returns the accelerometer wrapper.
    /// </summary>
    IAccelerometerManager GetAccelerometerManager();

    /// <summary>
    /// Returns the platform-specific prompt manager.
    /// </summary>
    IPromptManager GetPromptManager();
}