using Corund.Geometry;
using Corund.Sprites;
using Corund.Tools.UI;
using Microsoft.Xna.Framework;

namespace Corund.Tools.Helpers
{
    /// <summary>
    /// Helpers for working with sprites.
    /// </summary>
    public static class SpriteHelper
    {
        /// <summary>
        /// Adds default geometry and hotspot to the sprite.
        /// </summary>
        public static T WithDefaultGeometry<T>(
            this T sprite,
            HorizontalAlignment halign = HorizontalAlignment.Center,
            VerticalAlignment valign = VerticalAlignment.Center
        )
            where T: SpriteBase
        {
            var align = VectorHelper.GetAlignmentVector(halign, valign);
            var size = sprite.Size;
            var hotSpot = new Vector2(size.X * align.X, size.Y * align.Y);
            var geo = new GeometryRect(-hotSpot.X, -hotSpot.Y, size.X, size.Y);

            sprite.HotSpot = hotSpot;
            sprite.Geometry = geo;

            return sprite;
        }
    }
}
