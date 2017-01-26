using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Tools.Helpers
{
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
            var arr = new Color[rect.Width * rect.Height];

            for (var idx = 0; idx < arr.Length; idx++)
                arr[idx] = color;

            tex.SetData(0, rect, arr, 0, arr.Length);
        }

        /// <summary>
        /// Fills the entire texture with color.
        /// </summary>
        public static void Fill(this Texture2D tex, Color color)
        {
            var rect = new Rectangle(0, 0, tex.Width, tex.Height);
            tex.FillRect(rect, color);
        }
    }
}
