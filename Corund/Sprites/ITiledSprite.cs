using Microsoft.Xna.Framework;

namespace Corund.Sprites
{
    /// <summary>
    /// Common interface for sprites that support tiling.
    /// </summary>
    public interface ITiledSprite
    {
        /// <summary>
        /// Size of the rectangle to cover with tiles.
        /// </summary>
        Vector2 EffectiveSize { get; set; }

        /// <summary>
        /// Point in the texture that will be placed into left top corner of the rendered rectangle.
        /// </summary>
        Vector2 TextureOffset { get; set; }
    }
}
