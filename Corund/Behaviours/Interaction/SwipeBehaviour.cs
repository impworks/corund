using System;
using Corund.Engine;
using Corund.Tools;
using Corund.Tools.Helpers;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework.Input.Touch;

namespace Corund.Behaviours.Interaction
{
    /// <summary>
    /// A callback to be executed when a swipe has been detected.
    /// </summary>
    public class SwipeBehaviour: BehaviourBase
    {
        #region Constants

        /// <summary>
        /// Direction detector strictness.
        /// 1 = absolutely strict.
        /// 0 = any direction fits.
        /// </summary>
        private const float STRICTNESS = 0.5f;

        /// <summary>
        /// Maximum duration after which the swipe candidate is discarded.
        /// </summary>
        private const float MAX_DURATION = 0.3f;

        /// <summary>
        /// Minimum distance between swipe start and end.
        /// </summary>
        private const float MIN_DISTANCE = 200;

        #endregion

        #region Constructor

        public SwipeBehaviour(Direction direction, Action<SwipeInfo> callback)
        {
            _callback = callback;
            _direction = direction;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Function to execute when the swipe has been registered.
        /// </summary>
        private readonly Action<SwipeInfo> _callback;

        /// <summary>
        /// Allowed direction(s) to detect.
        /// </summary>
        private readonly Direction _direction;

        /// <summary>
        /// Origin of the swipe gesture.
        /// </summary>
        private TouchLocation? _start;

        /// <summary>
        /// Duration of the swipe.
        /// </summary>
        private float _duration;

        #endregion

        #region Methods

        public override void Bind(DynamicObject obj)
        {
            if(!(obj is InteractiveObject))
                throw new ArgumentException("Object must derive from InteractiveObject to handle swipes.");
        }

        public override void UpdateObjectState(DynamicObject obj)
        {
            if (_start == null)
            {
                // find swipe
                var iobj = obj as InteractiveObject;
                var touch = iobj.TryGetTouch();
                if (touch?.State == TouchLocationState.Pressed)
                {
                    _start = touch;
                    _duration = 0;
                }
            }
            else
            {
                // update
                _duration += GameEngine.Delta;
                if (_duration > MAX_DURATION)
                {
                    _start = null;
                    return;
                }

                var touch = FindTouchById(_start.Value.Id);
                if (touch.State == TouchLocationState.Moved)
                    return;

                if (touch.State == TouchLocationState.Released)
                {
                    var dist = touch.Position - _start.Value.Position;
                    if (dist.Length() > MIN_DISTANCE)
                    {
                        var dir = dist.GetDirection(STRICTNESS);
                        if (dir != null && _direction.HasFlag(dir.Value))
                        {
                            _callback(new SwipeInfo(
                                _start.Value.Position,
                                touch.Position,
                                _duration,
                                dir.Value
                            ));
                        }
                    }
                }

                _start = null;
            }
        }

        /// <summary>
        /// Gets a touch from current scene by its ID.
        /// Throws an exception otherwise.
        /// </summary>
        private TouchLocation FindTouchById(int id)
        {
            foreach (var touch in GameEngine.Current.Touches)
            {
                if (touch.Id == id)
                    return touch;
            }

            throw new ArgumentException($"Touch #{id} was not found.");
        }

        #endregion
    }
}
