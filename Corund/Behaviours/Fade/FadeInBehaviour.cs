using Corund.Engine;
using Corund.Tools.Helpers;
using Corund.Tools.Interpolation;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Fade
{
    /// <summary>
    /// Creates a fade in effect when applied to an object.
    /// </summary>
    public class FadeInBehaviour: BehaviourBase, IFadeInEffect
    {
        #region Constructor

        public FadeInBehaviour(float time, FadeStyle style = FadeStyle.Fade, InterpolationMethod interpolation = null)
        {
            Duration = time;

            _style = style;
            _interpolation = interpolation ?? Interpolate.Linear;
        }

        #endregion

        #region Fields

        private readonly FadeStyle _style;
        private readonly InterpolationMethod _interpolation;

        private float _elapsedTime;

        #endregion

        #region Properties

        public float Duration { get; }
        public float? Progress => _elapsedTime/Duration;

        #endregion

        #region Methods

        /// <summary>
        /// Animates object properties from effect-defined to actual.
        /// </summary>
        public override void Bind(DynamicObject obj)
        {
            var opacity = obj.Opacity;
            var scale = obj.Scale;

            if (_style == FadeStyle.Fade || _style == FadeStyle.ZoomAndFade || _style == FadeStyle.InverseZoomAndFade)
            {
                obj.Opacity = 0;
                obj.Tween(Property.Opacity, opacity, Duration, _interpolation);
            }

            if (_style == FadeStyle.Zoom || _style == FadeStyle.ZoomAndFade || _style == FadeStyle.InverseZoomAndFade)
            {
                obj.Scale = _style == FadeStyle.InverseZoomAndFade ? 2 : 0;
                obj.Tween(Property.Scale, scale, Duration, _interpolation);
            }
        }

        /// <summary>
        /// Tracks effect progress.
        /// </summary>
        public override void UpdateObjectState(DynamicObject obj)
        {
            _elapsedTime += GameEngine.Delta;
        }

        #endregion
    }
}
