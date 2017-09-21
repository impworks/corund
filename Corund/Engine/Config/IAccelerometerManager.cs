namespace Corund.Engine.Config
{
    /// <summary>
    /// Interface for platform-specific accelerometer implementations.
    /// </summary>
    public interface IAccelerometerManager
    {
        /// <summary>
        /// Activates the accelerometer.
        /// </summary>
        /// <returns>True if successful.</returns>
        bool Start();

        /// <summary>
        /// Deactivates the accelerometer.
        /// </summary>
        /// <returns>True if successful.</returns>
        bool Stop();

        /// <summary>
        /// Flag indicating that the accelerometer is currently active.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Current X coordinate.
        /// </summary>
        double X { get; }

        /// <summary>
        /// Current Y coordinate.
        /// </summary>
        double Y { get; }

        /// <summary>
        /// Current Z coordinate.
        /// </summary>
        double Z { get; }
    }
}
