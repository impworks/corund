using Windows.Devices.Sensors;
using Corund.Engine.Config;

namespace Corund.Platform.UWP.Input
{
    /// <summary>
    /// Windows Phone 8-specific implementation of AccelerometerManager.
    /// </summary>
    public class UWPAccelerometerManager : IAccelerometerManager
    {
        #region Constructor

        public UWPAccelerometerManager()
        {
            _accelerometer = Accelerometer.GetDefault();
        }

        #endregion

        #region Fields

        /// <summary>
        /// Reference to native accelerometer.
        /// </summary>
        private readonly Accelerometer _accelerometer;

        #endregion

        #region IAccelerometerManager implementation

        public bool IsActive { get; private set; }

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public bool Start()
        {
            if (IsActive)
                return false;

            IsActive = true;
            _accelerometer.ReadingChanged += OnReadingChanged;

            return true;
        }

        public bool Stop()
        {
            if (!IsActive)
                return false;

            IsActive = false;
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
