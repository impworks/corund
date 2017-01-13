using System;
using Corund.Frames;
using Corund.Tools;
using Corund.Visuals.Primitives;

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
            /// Currently drawn frame.
            /// </summary>
            public static FrameBase Frame;

            /// <summary>
            /// Current pause mode.
            /// </summary>
            public static PauseMode PauseMode;

            /// <summary>
            /// Shortcut to frame's timer.
            /// </summary>
            public static TimelineManager Timeline => Frame.Timeline;

            /// <summary>
            /// Z-coordinate sort function.
            /// </summary>
            public static Func<DynamicObject, float> ZOrderFunction;
        }
    }
}
