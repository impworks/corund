using Microsoft.Xna.Framework;

namespace Corund.Tools
{
    /// <summary>
    /// Information about an object's position relative to the screen.
    /// </summary>
    public struct TransformInfo
    {
        public TransformInfo(Vector2 position, float angle, Vector2 scale)
        {
            Position = position;
            Angle = angle;
            ScaleVector = scale;
        }

        public readonly Vector2 Position;
        public readonly float Angle;
        public readonly Vector2 ScaleVector;
    }
}
