using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Frames
{
    /// <summary>
    /// The camera settings for the frame.
    /// </summary>
    public class Camera: DynamicObject
    {
        #region Properties

        /// <summary>
        /// Scrolling offset (from the top left corner).
        /// </summary>
        public virtual Vector2 Offset => Position;

        #endregion
    }
}
