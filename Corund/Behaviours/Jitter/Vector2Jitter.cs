using Corund.Tools.Helpers;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Jitter
{
    /// <summary>
    /// Float jitter effect.
    /// </summary>
    public class Vector2Jitter<TObject> : PropertyJitterBase<TObject, Vector2>
        where TObject : DynamicObject
    {
        #region Constructor

        /// <summary>
        /// Creates a new FloatJitter effect.
        /// </summary>
        /// <param name="descriptor">Property to affect.</param>
        /// <param name="delay">Time between value changes in seconds.</param>
        /// <param name="xRange">Jitter magnitude for X coordinate.</param>
        /// <param name="yRange">Jitter magnitude for Y coordinate.</param>
        /// <param name="isRelative">Flag indicating that magnitude is a fraction of the actual value, rather than an absolute.</param>
        public Vector2Jitter(IPropertyDescriptor<TObject, Vector2> descriptor, float delay, float xRange, float yRange, bool isRelative = false)
            : base(descriptor, delay)
        {
            _xRange = xRange;
            _yRange = yRange;
            _isRelative = isRelative;
        }

        #endregion

        #region Fields

        private readonly float _xRange;
        private readonly float _yRange;

        /// <summary>
        /// Flag indicating that the range is a fraction of the actual value, rather than an absolute.
        /// </summary>
        private readonly bool _isRelative;

        /// <summary>
        /// Previously applied jitter.
        /// </summary>
        private Vector2 _lastJitter;

        #endregion

        #region Methods

        /// <summary>
        /// Cancels out previous jitter.
        /// </summary>
        protected override Vector2 CancelPrevious(Vector2 value)
        {
            return value - _lastJitter;
        }

        /// <summary>
        /// Applies new jitter.
        /// </summary>
        protected override Vector2 ApplyNew(Vector2 value)
        {
            var rx = _xRange;
            var ry = _yRange;
            if (_isRelative)
            {
                rx *= value.X;
                ry *= value.Y;
            }

            _lastJitter = new Vector2(
                RandomHelper.Float(-rx, rx),
                RandomHelper.Float(-ry, ry)
            );
            return value + _lastJitter;
        }

        #endregion
    }
}
