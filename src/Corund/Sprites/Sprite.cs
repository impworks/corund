using Corund.Engine;
using Corund.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Sprites;

/// <summary>
/// Simple sprite with a single texture.
/// </summary>
public class Sprite: SpriteBase
{
    #region Constructor

    public Sprite(string res)
        : this(GameEngine.Content.Load<Texture2D>(res))
    {

    }

    public Sprite(Texture2D texture)
        : base(texture)
    {
        Size = new Vector2(texture.Width, texture.Height);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Draws the sprite to the render target.
    /// </summary>
    public override void Draw(TransformInfo transform, Color tint, float zOrder)
    {
        GameEngine.Render.TryBeginBatch(BlendState);
        GameEngine.Render.SpriteBatch.Draw(
            Texture,
            transform.Position,
            null,
            tint,
            transform.Angle,
            HotSpot,
            transform.ScaleVector,
            SpriteEffects.None,
            zOrder
        );
    }

    #endregion
}