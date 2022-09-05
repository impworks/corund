﻿using Corund.Engine;
using Corund.Engine.Config;
using Corund.Platform.Android.Tools;

namespace Corund.Platform.Android;

/// <summary>
/// Android-specific wrapper provider.
/// </summary>
public class AndroidPlatformAdapter : IPlatformAdapter
{
    public IContentProvider GetEmbeddedContentProvider() => new EmbeddedContentProvider(GetType().Assembly, "Corund.Platform.Android.Content.Resources");
    public IAccelerometerManager GetAccelerometerManager() => new AndroidAccelerometerManager();
}