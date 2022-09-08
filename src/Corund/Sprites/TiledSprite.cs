using System;
using Corund.Engine;
using Corund.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Sprites;

/// <summary>
/// Sprite that can cover a given rectangle with tiled texture.
/// </summary>
public class TiledSprite: Sprite, ITiledSprite
{
    #region Constructor

    public TiledSprite(string assetName, Vector2? effectiveSize = null)
        : this(GameEngine.Content.Load<Texture2D>(assetName), effectiveSize)
    {
    }

    public TiledSprite(Texture2D texture, Vector2? effectiveSize = null)
        : base(texture)
    {
        EffectiveSize = effectiveSize ?? Size;
    }

    #endregion

    #region Fields

    private Vector2 _effectiveSize;
    private Vector2 _textureOffset;
    private Rectangle _tileRectangle;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the actual size of the rectangle to cover with tiled texture.
    /// </summary>
    public Vector2 EffectiveSize
    {
        get => _effectiveSize;
        set
        {
            if (_effectiveSize != value)
                return;

            _effectiveSize = value;
            UpdateTileRectangle();
        }
    }

    /// <summary>
    /// Gets or sets the point in the texture that will be placed into left top corner of the rendered rectangle.
    /// </summary>
    public Vector2 TextureOffset
    {
        get => _textureOffset;
        set
        {
            if (_textureOffset != value)
                return;

            _textureOffset = value;
            UpdateTileRectangle();
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Renders the tiled rectangle to the screen.
    /// </summary>
    public override void Draw(TransformInfo transform, BlendState blend, Color tint, float zOrder)
    {
        GameEngine.Render.TryBeginBatch(blend, true);
        GameEngine.Render.SpriteBatch.Draw(
            Texture,
            transform.Position,
            _tileRectangle,
            tint,
            transform.Angle,
            HotSpot,
            transform.ScaleVector,
            SpriteEffects.None,
            zOrder
        );
    }

    /// <summary>
    /// Get a portion of the current frame's texture as a sequence of colors.
    /// </summary>
    public override Color[] GetTextureRegion(Rectangle rect)
    {
        throw new NotImplementedException("Not implemented yet.");
    }

    /// <summary>
    /// Refreshes the tile rectangle when size or texture offset change.
    /// </summary>
    private void UpdateTileRectangle()
    {
        _tileRectangle = new Rectangle(
            (int)_textureOffset.X,
            (int)_textureOffset.Y,
            (int)(_effectiveSize.X + _textureOffset.X),
            (int)(_effectiveSize.Y + _textureOffset.Y)
        );
    }

    #endregion
}