using System.Diagnostics;
using Corund.Tools.Helpers;

namespace Corund.Tools.Jitter
{
    /// <summary>
    /// A structure that represents a value and jitter around it.
    /// </summary>
    [DebuggerDisplay("Jitter({Median}, {Jitter})")]
    public struct JitteryValue
    {
        #region Constructor

        /// <summary>
        /// Create a new jittery value instance.
        /// </summary>
        /// <param name="median">Median.</param>
        /// <param name="jitter">Value spread around the median.</param>
        public JitteryValue(float median, float jitter = 0)
        {
            Median = median;
            Jitter = jitter;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Center of the value spread.
        /// </summary>
        public float Median;

        /// <summary>
        /// Spread coefficient.
        /// </summary>
        public float Jitter;

        #endregion

        #region Methods

        /// <summary>
        /// Get a new value that falls into allowed jittery range.
        /// </summary>
        public float GetValue()
        {
            if (Jitter.IsAlmostZero())
                return Median;

            return Median + RandomHelper.Float(-Jitter, Jitter);
        }

        /// <summary>
        /// Assign the value just as a float if no jitter is needed.
        /// </summary>
        /// <param name="value">Median value.</param>
        public static implicit operator JitteryValue(float value)
        {
            return new JitteryValue(value);
        }

        /// <summary>
        /// Assign the value just as a float if no jitter is needed.
        /// </summary>
        /// <param name="value">Median value.</param>
        public static implicit operator JitteryValue(int value)
        {
            return new JitteryValue(value);
        }

        #endregion
    }
}
