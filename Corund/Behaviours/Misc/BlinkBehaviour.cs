using Corund.Engine;
using Corund.Tools.Helpers;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Misc
{
    /// <summary>
    /// Blink the object via opacity during a specified timespan.
    /// </summary>
    public class BlinkBehaviour : BehaviourBase, IEffect
    {
        #region Constructor

        public BlinkBehaviour(int blinkCount, float duration)
        {
            _blinkCount = blinkCount;
            Duration = duration;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Number of times the object must hide and appear.
        /// </summary>
        private readonly int _blinkCount;

        /// <summary>
        /// Saved opacity value.
        /// </summary>
        private float _originalOpacity;

        /// <summary>
        /// Time elapsed since the effect start.
        /// </summary>
        private float _elapsedTime;

        /// <summary>
        /// Timer event keys (for cancellation).
        /// </summary>
        private int[] _timerKeys;

        #endregion

        #region Properties

        /// <summary>
        /// Duration of the effect.
        /// </summary>
        public float Duration { get; }

        /// <summary>
        /// Current progress of the effect (0..1).
        /// </summary>
        public float? Progress => _elapsedTime/Duration;

        #endregion

        #region Methods

        /// <summary>
        /// Applies animation.
        /// </summary>
        public override void Bind(DynamicObject obj)
        {
            base.Bind(obj);

            var span = Duration/_blinkCount;

            _originalOpacity = obj.Opacity;
            _timerKeys = new int[_blinkCount];
            for (var i = 0; i < _blinkCount; i++)
            {
                _timerKeys[i] = GameEngine.Current.Timeline.Add(
                    i*span,
                    () => obj.Tween(Property.Opacity, 0, span/2, null, true)
                );
            }
        }

        /// <summary>
        /// Updates effect state.
        /// </summary>
        public override void UpdateObjectState(DynamicObject obj)
        {
            _elapsedTime += GameEngine.Delta;
        }

        /// <summary>
        /// Cancels all pending timer keyframes and sets value to original.
        /// </summary>
        public override void Unbind(DynamicObject obj)
        {
            for(var i = 0; i < _blinkCount; i++)
                GameEngine.Current.Timeline.Remove(_timerKeys[i]);

            obj.StopTweening(Property.Opacity);
            obj.Opacity = _originalOpacity;
        }

        #endregion
    }
}
