using System;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace Corund.Engine;

/// <summary>
/// A manager for built-in platform-specific resources.
/// </summary>
public class EmbeddedContentManager : ContentManager
{
    #region Constructor

    public EmbeddedContentManager(IServiceProvider serviceProvider, EmbeddedContentProvider provider)
        : base(serviceProvider)
    {
        Provider = provider;
    }

    #endregion

    #region Fields

    /// <summary>
    /// Reference to platform-specific content provider.
    /// </summary>
    public readonly EmbeddedContentProvider Provider;

    #endregion

    #region Methods

    protected override Stream OpenStream(string assetName) => Provider.GetResource(assetName);

    #endregion
}