using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Tools.Helpers;

/// <summary>
/// A collection of texture manipulation methods.
/// </summary>
public static class TextureHelper
{
    /// <summary>
    /// Fills the specified texture region with color.
    /// </summary>
    public static void FillRect(this Texture2D tex, Rectangle rect, Color color)
    {
        var pixels = new Color[rect.Width * rect.Height];

        for (var idx = 0; idx < pixels.Length; idx++)
            pixels[idx] = color;

        tex.SetData(0, rect, pixels, 0, pixels.Length);
    }

    /// <summary>
    /// Fills the entire texture with color.
    /// </summary>
    public static void Fill(this Texture2D tex, Color color)
    {
        var rect = new Rectangle(0, 0, tex.Width, tex.Height);
        tex.FillRect(rect, color);
    }

    /// <summary>
    /// Fills a circle in the texture.
    /// </summary>
    public static void FillCircle(this Texture2D tex, float xCenter, float yCenter, float radius, Color color)
    {
        var rect = new Rectangle((int)(xCenter - radius), (int)(yCenter - radius), (int)(radius*2), (int)(radius*2));
        var pixels = new Color[rect.Width * rect.Height];

        tex.GetData(0, rect, pixels, 0, pixels.Length);

        for (var x = 0; x < rect.Width; x++)
        {
            for (var y = 0; y < rect.Height; y++)
            {
                var xRel = x + rect.Left - xCenter;
                var yRel = y + rect.Top - yCenter;

                if (xRel*xRel + yRel*yRel <= radius)
                    pixels[y*rect.Width + x] = color;
            }
        }

        tex.SetData(0, rect, pixels, 0, pixels.Length);
    }
}