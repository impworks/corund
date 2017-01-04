using Corund.Frames;
using Corund.Managers;

namespace Corund.Engine
{
    public static partial class GameEngine
    {
        /// <summary>
        /// A collection of shortcuts for various engine parts.
        /// </summary>
        public static class Current
        {
            /// <summary>
            /// Shortcut to frame.
            /// </summary>
            public static FrameBase Frame => Frames.Current;

            /// <summary>
            /// Shortcut to frame's timer.
            /// </summary>
            public static TimelineManager Timeline => Frames.Current.Timeline;
        }
    }
}
