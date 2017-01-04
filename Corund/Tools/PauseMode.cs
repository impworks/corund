using System;

namespace Corund.Tools
{
    /// <summary>
    /// The object parts that can be paused.
    /// </summary>
    [Flags]
    public enum PauseMode
    {
        /// <summary>
        /// Movement and rotation.
        /// </summary>
        Momentum = 0x01,

        /// <summary>
        /// User-defined behaviours and tweened properties.
        /// </summary>
        Behaviours = 0x02,

        /// <summary>
        /// Timed actions.
        /// Only applicable to a frame!
        /// </summary>
        Timeline = 0x04,

        /// <summary>
        /// Animation of sprites.
        /// </summary>
        SpriteAnimation = 0x10,

        None = 0,
        All = Momentum | Behaviours | SpriteAnimation
    }
}
