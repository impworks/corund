using System;
using System.Collections.Generic;
using Corund.Frames;
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
            _touches = new List<TouchLocation>(4);
            _handledTouches = new Dictionary<int, InteractiveObject>(4);
        }

        #endregion

        #region Fields

        /// <summary>
        /// Currently active touches.
        /// </summary>
        private List<TouchLocation> _touches;

        /// <summary>
        /// List of touches that have been handled by an object.
        /// </summary>
        private Dictionary<int, InteractiveObject> _handledTouches;

        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the internal collection of touch locations.
        /// </summary>
        public void Update()
        {
            _handledTouches.Clear();
            _touches.Clear();

            foreach(var location in TouchPanel.GetState())
                _touches.Add(location);
        }

        /// <summary>
        /// Checks if the current object can handle this touch.
        /// </summary>
        public bool CanHandleTouch(TouchLocation touch, InteractiveObject obj)
        {
            return !_handledTouches.ContainsKey(touch.Id)
                   || ReferenceEquals(obj, _handledTouches[touch.Id]);
        }

        /// <summary>
        /// Registers the touch location as handled by a specific object.
        /// </summary>
        public void HandleTouch(TouchLocation touch, InteractiveObject obj)
        {
            if (!_handledTouches.ContainsKey(touch.Id))
                _handledTouches[touch.Id] = obj;
        }

        /// <summary>
        /// Translates the touch coordinates to frame.
        /// </summary>
        public TouchLocation TranslateToFrame(TouchLocation touch, FrameBase frame)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
