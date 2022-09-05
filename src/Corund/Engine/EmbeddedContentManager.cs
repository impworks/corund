using System;
using System.IO;
using System.Reflection;
using Corund.Engine.Config;
using Microsoft.Xna.Framework.Content;

namespace Corund.Engine;

/// <summary>
/// A manager for built-in platform-specific resources.
/// </summary>
public class EmbeddedContentManager : ContentManager
{
    #region Constructor

    public EmbeddedContentManager(IServiceProvider serviceProvider, IContentProvider contentProvider)
        : base(serviceProvider)
    {
        _contentProvider = contentProvider;
    }

    #endregion

    #region Fields

    /// <summary>
    /// Reference to platform-specific content provider.
    /// </summary>
    private readonly IContentProvider _contentProvider;

    #endregion

    #region Methods

    /// <summary>
    /// Returns the stream from the platform-specific assembly resource.
    /// </summary>
    protected override Stream OpenStream(string assetName)
    {
        return _contentProvider.GetResource(assetName);
    }

    #endregion
}