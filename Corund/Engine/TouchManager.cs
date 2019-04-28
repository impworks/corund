using System.Collections.Generic;
using Corund.Frames;
using Corund.Tools.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
            _handledTouches = new Dictionary<int, object>(4);
            _mouseButtonState = ButtonState.Released;
        }

        #endregion

        #region Fields

        /// <summary>
        /// List of touches that have been handled by an object.
        /// </summary>
        private Dictionary<int, object> _handledTouches;

        /// <summary>
        /// Mouse button's last state for touch emulation.
        /// </summary>
        private ButtonState _mouseButtonState;

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

            if (GameEngine.Options.EnableMouse && Touches.Count == 0)
            {
                var loc = ConvertMouseToTouch();
                if(loc != null)
                    Touches.Add(loc.Value);
            }
        }

        /// <summary>
        /// Checks if the current object can handle this touch.
        /// </summary>
        public bool CanHandle(TouchLocation touch, object obj)
        {
            _handledTouches.TryGetValue(touch.Id, out var handler);
            return handler == null || ReferenceEquals(obj, handler);
        }

        /// <summary>
        /// Registers the touch location as handled by a specific object.
        /// </summary>
        public void Handle(TouchLocation touch, object obj)
        {
            if (!_handledTouches.ContainsKey(touch.Id))
                _handledTouches[touch.Id] = obj;
        }

        /// <summary>
        /// Translates the touch coordinates to frame.
        /// </summary>
        public TouchLocation? TranslateToFrame(TouchLocation touch, FrameBase frame)
        {
            // account for resolution adaptation
            var tx = frame.ResolutionAdaptationTransform;
            var pos = tx.TranslateBack(touch.Position);

            // align point to frame rendertarget
            var normalisedPt = pos - frame.Position;
            var viewPt = normalisedPt.Rotate(-frame.Angle)/frame.ScaleVector + frame.HotSpot;

            // point does not fit the viewport
            var size = frame.ViewSize;
            if (viewPt.X < 0 || viewPt.X > size.X || viewPt.Y < 0 || viewPt.Y > size.Y)
                return null;

            // translate point into frame coordinates
            var cam = frame.Camera;
            var framePt = viewPt.Rotate(-cam.Angle)/cam.ScaleVector + cam.Offset;

            return new TouchLocation(touch.Id, touch.State, framePt);
        }

        /// <summary>
        /// Returns a touch location by its ID.
        /// </summary>
        public TouchLocation? Find(int id)
        {
            for (var idx = 0; idx < Touches.Count; idx++)
            {
                var touch = Touches[idx];
                if (touch.Id == id)
                    return touch;
            }

            return null;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Returns a virtual touch location at the mouse cursor.
        /// </summary>
        private TouchLocation? ConvertMouseToTouch()
        {
            var mouse = Mouse.GetState();
            var state = GetEmulatedTouchState(mouse.LeftButton, _mouseButtonState);
            _mouseButtonState = mouse.LeftButton;

            if (state == null)
                return null;

            return new TouchLocation(1, state.Value, new Vector2(mouse.X, mouse.Y));
        }

        /// <summary>
        /// Returns the virtual touch state represented by the mouse state.
        /// </summary>
        private TouchLocationState? GetEmulatedTouchState(ButtonState current, ButtonState previous)
        {
            if (current == ButtonState.Released)
            {
                return previous == ButtonState.Released
                    ? (TouchLocationState?) null
                    : TouchLocationState.Released;
            }

            return previous == ButtonState.Released
                ? TouchLocationState.Pressed
                : TouchLocationState.Moved;
        }

        #endregion
    }
}
