using Corund.Geometry;
using Corund.Sprites;
using Corund.Tools.UI;

namespace Corund.Tools.Helpers;

/// <summary>
/// Helpers for working with sprites.
/// </summary>
public static class SpriteHelper
{
    /// <summary>
    /// Adds default geometry and hotspot to the sprite.
    /// </summary>
    public static T AddGeometry<T>(
        this T sprite,
        HorizontalAlignment halign = HorizontalAlignment.Center,
        VerticalAlignment valign = VerticalAlignment.Center
    )
        where T: SpriteBase
    {
        var align = VectorHelper.GetAlignmentVector(halign, valign);
        var size = sprite.Size;
        var hotSpot = size * align;
        var geo = new GeometryRect(-hotSpot.X, -hotSpot.Y, size.X, size.Y);

        sprite.HotSpot = hotSpot;
        sprite.Geometry = geo;

        return sprite;
    }
}