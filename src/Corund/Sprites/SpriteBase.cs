﻿using Corund.Geometry;
using Corund.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Sprites;

/// <summary>
/// Base class for static and animated sprites.
/// </summary>
public abstract class SpriteBase
{
    #region Constructors

    protected SpriteBase(Texture2D texture)
    {
        Texture = texture;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the sprite texture.
    /// </summary>
    public readonly Texture2D Texture;

    /// <summary>
    /// Gets or sets the size of your sprite.
    /// </summary>
    public Vector2 Size;

    /// <summary>
    /// Gets or sets the location in the sprite that corresponds to the object's coordinates.
    /// </summary>
    public Vector2 HotSpot;

    /// <summary>
    /// Gets or sets the geometry of the current sprite.
    /// </summary>
    public IGeometry Geometry;

    #endregion

    #region Interface

    /// <summary>
    /// Updates the sprite's internal state.
    /// </summary>
    public virtual void Update()
    {
        // nothing here
    }

    /// <summary>
    /// Resets the sprite to initial values.
    /// </summary>
    public virtual void Reset()
    {
        // nothing here as well
    }

    /// <summary>
    /// Renders the sprite to current render target.
    /// </summary>
    public abstract void Draw(TransformInfo transform, BlendState blend, Color tint, float zOrder);

    /// <summary>
    /// Returns the raw texture color data for the specified region.
    /// </summary>
    public abstract Color[] GetTextureRegion(Rectangle rect);

    #endregion
}