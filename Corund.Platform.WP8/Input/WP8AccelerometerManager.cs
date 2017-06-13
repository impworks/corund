using Corund.Engine.Config;
using Windows.Devices.Sensors;

namespace Corund.Platform.WP8.Input
{
    /// <summary>
    /// Windows Phone 8-specific implementation of AccelerometerManager.
    /// </summary>
    public class WP8AccelerometerManager : IAccelerometerManager
    {
        #region Constructor

        public WP8AccelerometerManager()
        {
            _accelerometer = Accelerometer.GetDefault();
        }

        #endregion

        #region Fields

        /// <summary>
        /// Reference to native accelerometer.
        /// </summary>
        private readonly Accelerometer _accelerometer;

        /// <summary>
        /// Started flag (faux).
        /// </summary>
        private bool _isStarted;

        #endregion

        #region IAccelerometerManager implementation

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public bool Start()
        {
            if (_isStarted)
                return false;

            _isStarted = true;
            _accelerometer.ReadingChanged += OnReadingChanged;

            return true;
        }

        public bool Stop()
        {
            if (!_isStarted)
                return false;

            _isStarted = false;
            _accelerometer.ReadingChanged -= OnReadingChanged;

            X = Y = Z = 0;

            return true;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Updated current state when the readings have changed.
        /// </summary>
        private void OnReadingChanged(Accelerometer self, AccelerometerReadingChangedEventArgs args)
        {
            X = args.Reading.AccelerationX;
            Y = args.Reading.AccelerationY;
            Z = args.Reading.AccelerationZ;
        }

        #endregion
    }
}
