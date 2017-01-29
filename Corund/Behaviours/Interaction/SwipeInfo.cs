using Corund.Tools;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Interaction
{
    /// <summary>
    /// Information about a detected swipe.
    /// </summary>
    public struct SwipeInfo
    {
        public SwipeInfo(Vector2 start, Vector2 end, float duration, Direction dir)
        {
            StartPosition = start;
            EndPosition = end;
            Duration = duration;
            Direction = dir;
        }

        /// <summary>
        /// The point where the swipe has originated (in frame coordinates).
        /// </summary>
        public readonly Vector2 StartPosition;

        /// <summary>
        /// The point where the swipe has finished (in frame coordinates).
        /// </summary>
        public readonly Vector2 EndPosition;

        /// <summary>
        /// The duration of the swipe (in seconds).
        /// </summary>
        public readonly float Duration;

        /// <summary>
        /// The detected duration of the swipe.
        /// </summary>
        public readonly Direction Direction;
    }
}
