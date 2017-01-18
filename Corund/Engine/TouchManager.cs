using System.Collections.Generic;
using Corund.Frames;
using Corund.Tools.Helpers;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework.Input.Touch;

namespace Corund.Engine
{
    /// <summary>
    /// The manager class for touch events.
    /// </summary>
    public class TouchManager
    {
        #region Constructor

        public TouchManager()
        {
            Touches = new List<TouchLocation>(4);
            _handledTouches = new Dictionary<int, ObjectBase>(4);
        }

        #endregion

        #region Fields

        /// <summary>
        /// List of touches that have been handled by an object.
        /// </summary>
        private Dictionary<int, ObjectBase> _handledTouches;

        #endregion

        #region Properties

        /// <summary>
        /// Currently active touches.
        /// </summary>
        public readonly List<TouchLocation> Touches;

        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the internal collection of touch locations.
        /// </summary>
        public void Update()
        {
            _handledTouches.Clear();
            Touches.Clear();

            foreach(var location in TouchPanel.GetState())
                Touches.Add(location);
        }

        /// <summary>
        /// Checks if the current object can handle this touch.
        /// </summary>
        public bool CanHandleTouch(TouchLocation touch, ObjectBase obj)
        {
            return !_handledTouches.ContainsKey(touch.Id)
                   || ReferenceEquals(obj, _handledTouches[touch.Id]);
        }

        /// <summary>
        /// Registers the touch location as handled by a specific object.
        /// </summary>
        public void HandleTouch(TouchLocation touch, ObjectBase obj)
        {
            if (!_handledTouches.ContainsKey(touch.Id))
                _handledTouches[touch.Id] = obj;
        }

        /// <summary>
        /// Translates the touch coordinates to frame.
        /// </summary>
        public TouchLocation? TranslateToFrame(TouchLocation touch, FrameBase frame)
        {
            // align point to frame
            var leftTop = frame.Position - frame.HotSpot;
            var rightBottom = leftTop + frame.ViewSize;
            var point = (touch.Position - frame.Position).Rotate(-frame.Angle);

            // point does not fit the viewport
            if (point.X < leftTop.X || point.X > rightBottom.X || point.Y < leftTop.Y || point.Y > rightBottom.Y)
                return null;

            // transform point into frame coordinates
            var cam = frame.Camera;
            var viewPoint = point - leftTop;
            var framePoint = viewPoint.Rotate(-cam.Angle)/cam.ScaleVector - cam.Offset;

            return new TouchLocation(touch.Id, touch.State, framePoint);
        }

        #endregion
    }
}
