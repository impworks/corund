namespace Corund.Tools.Helpers
{
    /// <summary>
    /// A collection of helper methods for numbers.
    /// </summary>
    public static class NumericHelper
    {
        /// <summary>
        /// The mininum value to take into account when modifying object properties.
        /// Prevents coordinates, angle, etc from jitter degradation over time.
        /// </summary>
        public const float Epsilon = 0.0001f;

        /// <summary>
        /// Check whether a number is too small to account for.
        /// </summary>
        public static bool IsAlmostNull(this float number)
        {
            return number < Epsilon
                   && number > -Epsilon;
        }

        /// <summary>
        /// Check whether two floating point numbers are almost equal (to a given precision).
        /// </summary>
        /// <param name="number">Number</param>
        /// <param name="compareTo">Other number</param>
        /// <param name="precision">Precision.</param>
        public static bool IsAlmost(this float number, float compareTo, float precision = Epsilon)
        {
            return (number <= compareTo + precision) && (number >= compareTo - precision);
        }
    }
}
