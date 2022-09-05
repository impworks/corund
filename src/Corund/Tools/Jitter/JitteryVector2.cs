using System.Diagnostics;
using Corund.Tools.Helpers;
using Microsoft.Xna.Framework;

namespace Corund.Tools.Jitter
{
    /// <summary>
    /// A structure that represents a value and jitter around it.
    /// </summary>
    [DebuggerDisplay("JitteryVector({Median}, {Jitter})")]
    public struct JitteryVector2
    {
        #region Constructor

        /// <summary>
        /// Create a new jittery vector instance.
        /// </summary>
        /// <param name="medianX">X median.</param>
        /// <param name="medianY">Y median.</param>
        /// <param name="jitter">Value spread around the median (on both axes).</param>
        public JitteryVector2(float medianX, float medianY, float jitter = 0)
        {
            Median = new Vector2(medianX, medianY);
            Jitter = new Vector2(jitter);
        }

        /// <summary>
        /// Create a new jittery vector instance.
        /// </summary>
        /// <param name="median">Median.</param>
        /// <param name="jitter">Value spread around the median.</param>
        public JitteryVector2(Vector2 median, Vector2 jitter)
        {
            Median = median;
            Jitter = jitter;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Median of the vector.
        /// </summary>
        public Vector2 Median;

        /// <summary>
        /// Spread coefficient.
        /// </summary>
        public Vector2 Jitter;

        #endregion

        #region Methods

        /// <summary>
        /// Gets a new vector that falls into allowed jittery range.
        /// </summary>
        public Vector2 GetValue()
        {
            if (Jitter.Length().IsAlmostZero())
                return Median;

            var jX = RandomHelper.Float(-Jitter.X, Jitter.X);
            var jY = RandomHelper.Float(-Jitter.Y, Jitter.Y);
            return new Vector2(Median.X + jX, Median.Y + jY);
        }

        /// <summary>
        /// Creates a non-jittery vector.
        /// </summary>
        public static implicit operator JitteryVector2(Vector2 value)
        {
            return new JitteryVector2(value, Vector2.Zero);
        }

        #endregion
    }
}
