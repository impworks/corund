using Corund.Engine;
using Corund.Geometry;
using Corund.Tools.Helpers;
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
        /// Gets the geometry matching visible area.
        /// </summary>
        public RectPolygon VisibleArea => GetVisibleArea();

        /// <summary>
        /// Gets the effective top left corner offset for current camera's position.
        /// </summary>
        public Vector2 Offset => GetOffset();

        #endregion

        #region Methods

        /// <summary>
        /// Maps point on screen to point in frame coordinates.
        /// </summary>
        public Vector2 TranslateFromScreen(Vector2 screenPoint)
        {
            var isSimple = Angle.IsAlmostNull()
                           && ScaleVector == Vector2.One;

            if (isSimple)
                return screenPoint + Offset;

            var frameSize = GameEngine.Current.Frame.Size;
            var frameCenter = Offset + frameSize/2;

            var screenCenter = GameEngine.Screen.Size/2;
            var screenRelative = screenPoint - screenCenter;

            return screenRelative.Rotate(Angle) * ScaleVector + frameCenter;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns the currently visible area in frame coordinates.
        /// </summary>
        private RectPolygon GetVisibleArea()
        {
            var x = MathHelper.Min(GameEngine.Screen.Size.X, GameEngine.Current.Frame.Size.X);
            var y = MathHelper.Min(GameEngine.Screen.Size.Y, GameEngine.Current.Frame.Size.Y);

            var isSimple = Angle.IsAlmostNull()
                           && ScaleVector == Vector2.One;

            if (isSimple)
            {
                return new RectPolygon(
                    Position,
                    new Vector2(x, 0) + Position,
                    new Vector2(x, y) + Position,
                    new Vector2(0, y) + Position,
                    0
                );
            }

            return new RectPolygon(
                TranslateFromScreen(new Vector2(0, 0)),
                TranslateFromScreen(new Vector2(x, 0)),
                TranslateFromScreen(new Vector2(x, y)),
                TranslateFromScreen(new Vector2(0, y)),
                Angle
            );
        }

        /// <summary>
        /// Returns the effective offset, clamping scrolling to frame bounds.
        /// </summary>
        private Vector2 GetOffset()
        {
            var screen = GameEngine.Screen.Size;
            var frame = GameEngine.Current.Frame.Size;
            var pos = Position - screen/2;

            if (pos.X + screen.X > frame.X) pos.X = frame.X - screen.X;
            if (pos.Y + screen.Y > frame.Y) pos.Y = frame.Y - screen.Y;
            if (pos.X < 0) pos.X = 0;
            if (pos.Y < 0) pos.Y = 0;

            return pos;
        }

        #endregion
    }
}
