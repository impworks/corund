using System;
using System.Collections.Generic;
using Corund.Frames;
using Corund.Tools;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework.Input.Touch;

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
            /// Touch locations translated to current frame coordinates.
            /// </summary>
            public static List<TouchLocation> Touches => Frame.Touches;

            /// <summary>
            /// Z-coordinate sort function.
            /// </summary>
            public static Func<ObjectBase, float> ZOrderFunction;
        }
    }
}
