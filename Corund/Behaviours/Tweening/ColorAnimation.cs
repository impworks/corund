﻿using System;
using Corund.Tools.Interpolation;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Tweening
{
    /// <summary>
    /// Animation for color properties.
    /// </summary>
    public class ColorAnimation: PropertyAnimationBase<Color>
    {
        public ColorAnimation(Color initial, Color target, float duration, Action<Color> setter, InterpolationMethod interpolation = null)
            : base(initial, target, duration, setter, interpolation)
        { }

        protected override Color getValue()
        {
            return new Color(
                getFloat(_initialValue.R, _targetValue.R),
                getFloat(_initialValue.G, _targetValue.G),
                getFloat(_initialValue.B, _targetValue.B),
                getFloat(_initialValue.A, _targetValue.A)
            );
        }
    }
}
