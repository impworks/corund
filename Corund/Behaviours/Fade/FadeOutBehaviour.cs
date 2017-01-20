﻿using Corund.Engine;
using Corund.Tools.Helpers;
using Corund.Tools.Interpolation;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Fade
{
    /// <summary>
    /// Creates a fade in effect when applied to an object.
    /// </summary>
    public class FadeOutBehaviour: BehaviourBase, IFadeOutEffect
    {
        #region Constructor

        public FadeOutBehaviour(float duration, FadeStyle style = FadeStyle.Fade, InterpolationMethod interpolation = null)
        {
            Duration = duration;

            _style = style;
            _interpolation = interpolation;
        }

        #endregion

        #region Fields

        private readonly FadeStyle _style;
        private readonly InterpolationMethod _interpolation;

        private float? _elapsedTime;

        #endregion

        #region Properties

        public float Duration { get; }
        public float? Progress
        {
            get
            {
                if (!_elapsedTime.HasValue)
                    return null;

                return Duration/_elapsedTime.Value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Animates the properties from current values to those specified by fade style.
        /// </summary>
        public void ActivateFadeOut(DynamicObject obj)
        {
            _elapsedTime = 0;

            if (_style == FadeStyle.Fade || _style == FadeStyle.ZoomAndFade || _style == FadeStyle.InverseZoomAndFade)
                obj.Tween(Property.Opacity, 0, Duration, _interpolation);

            if (_style == FadeStyle.Zoom || _style == FadeStyle.ZoomAndFade || _style == FadeStyle.InverseZoomAndFade)
                obj.Tween(Property.Scale, _style == FadeStyle.InverseZoomAndFade ? 2 : 0, Duration, _interpolation);
        }

        /// <summary>
        /// Tracks progress of elapsed time.
        /// </summary>
        public override void UpdateObjectState(DynamicObject obj)
        {
            if (_elapsedTime.HasValue)
                _elapsedTime += GameEngine.Delta;
        }

        #endregion
    }
}
