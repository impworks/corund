using System.Collections.Generic;
using Corund.Frames;
using Corund.Geometry;
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
            GlobalTouches = new List<TouchLocation>(4);
            _handledTouches = new Dictionary<int, object>(4);
            _capturedTouches = new Dictionary<int, object>(4);
            _mouseButtonState = ButtonState.Released;
        }

        #endregion

        #region Fields

        /// <summary>
        /// List of touches that have been handled by an object.
        /// </summary>
        private Dictionary<int, object> _handledTouches;

        /// <summary>
        /// Touches bound to a particular object.
        /// </summary>
        private Dictionary<int, object> _capturedTouches;

        /// <summary>
        /// Mouse button's last state for touch emulation.
        /// </summary>
        private ButtonState _mouseButtonState;

        #endregion

        #region Properties

        /// <summary>
        /// Currently active touches.
        /// </summary>
        public readonly List<TouchLocation> GlobalTouches;

        /// <summary>
        /// Touches bound to currently executing frame.
        /// </summary>
        public List<TouchLocation> LocalTouches => GameEngine.Current.Frame.LocalTouches;

        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the internal collection of touch locations.
        /// </summary>
        public void Update()
        {
            _handledTouches.Clear();
            GlobalTouches.Clear();

            foreach(var location in TouchPanel.GetState())
                GlobalTouches.Add(location);

            RefreshCaptures();

            if (GameEngine.Options.EnableMouse && GlobalTouches.Count == 0)
            {
                var loc = ConvertMouseToTouch();
                if(loc != null)
                    GlobalTouches.Add(loc.Value);
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
        /// Exclusively binds the touch to a particular object.
        /// </summary>
        public void Capture(TouchLocation touch, object obj)
        {
            if(!_capturedTouches.ContainsKey(touch.Id))
                _capturedTouches.Add(touch.Id, obj);
        }

        /// <summary>
        /// Releases the exclusive binding to a particular object.
        /// </summary>
        public void Release(TouchLocation touch)
        {
            _capturedTouches.Remove(touch.Id);
        }

        /// <summary>
        /// Attempts to get a touch location for current object.
        /// </summary>
        /// <param name="obj">Object with a geometry definition.</param>
        /// <param name="tapThrough">Flag indicating that touch should be available for underlying objects as well.</param>
        public TouchLocation? TryGetTouch(IGeometryObject obj, bool tapThrough = false)
        {
            if (obj.Geometry == null)
                return null;

            var transform = obj.GetTransformInfo(false);
            foreach (var touch in LocalTouches)
            {
                if (_capturedTouches.TryGetValue(touch.Id, out var captured))
                {
                    if (ReferenceEquals(obj, captured))
                        return touch;

                    continue;
                }

                if (!CanHandle(touch, obj))
                    continue;

                if (obj.Geometry.ContainsPoint(touch.Position, transform))
                {
                    if (!tapThrough)
                        Handle(touch, obj);

                    return touch;
                }
            }

            return null;
        }

        /// <summary>
        /// Attempts to get all touch locations for current object.
        /// </summary>
        /// <param name="obj">Object with a geometry definition.</param>
        /// <param name="tapThrough">Flag indicating that touch should be available for underlying objects as well.</param>
        public IList<TouchLocation> TryGetTouches(IGeometryObject obj, bool tapThrough = false)
        {
            if (obj.Geometry == null)
                return null;

            List<TouchLocation> result = null;
            var transform = obj.GetTransformInfo(false);
            foreach (var touch in LocalTouches)
            {
                _capturedTouches.TryGetValue(touch.Id, out var captured);
                if (captured != null && !ReferenceEquals(captured, obj))
                    continue;

                if (captured == null)
                {
                    if (!CanHandle(touch, obj))
                        continue;

                    if (!obj.Geometry.ContainsPoint(touch.Position, transform))
                        continue;
                }

                if (!tapThrough)
                    Handle(touch, obj);

                if (result == null)
                    result = new List<TouchLocation>();

                result.Add(touch);
            }

            return result;
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
        /// <param name="id">Unique ID of the touch.</param>
        /// <param name="inFrame">If true, touch position will be in Frame-based coordinates. Otherwise - in screen-based.</param>
        public TouchLocation? FindById(int id, bool inFrame = true)
        {
            var source = inFrame ? LocalTouches : GlobalTouches;
            for (var idx = 0; idx < source.Count; idx++)
            {
                var touch = GlobalTouches[idx];
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

        /// <summary>
        /// Releases captures for touch locations that are no longer active.
        /// </summary>
        private void RefreshCaptures()
        {
            if (_capturedTouches.Count == 0)
                return;

            List<int> keys = null;

            foreach (var key in _capturedTouches.Keys)
            {
                var exists = false;
                foreach (var touch in GlobalTouches)
                {
                    if (touch.Id == key)
                    {
                        exists = true;
                        break;
                    }
                }

                if (exists)
                    continue;

                if(keys == null)
                    keys = new List<int>();

                keys.Add(key);
            }

            if(keys != null)
                foreach (var key in keys)
                    _capturedTouches.Remove(key);
        }

        #endregion
    }
}
