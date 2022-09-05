using Corund.Engine.Config;
using Microsoft.Devices.Sensors;

namespace Corund.Platform.Android.Tools;

/// <summary>
/// Android-specific implementation of IAccelerometerManager.
/// </summary>
public class AndroidAccelerometerManager: IAccelerometerManager
{
    #region Constructor

    public AndroidAccelerometerManager()
    {
        _accelerometer = new Accelerometer();
        _accelerometer.CurrentValueChanged += OnReadingChanged;
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
        _accelerometer.Start();

        return true;
    }

    public bool Stop()
    {
        if (!IsActive)
            return false;

        IsActive = false;
        _accelerometer.Stop();

        X = Y = Z = 0;

        return true;
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Updated current state when the readings have changed.
    /// </summary>
    private void OnReadingChanged(object self, SensorReadingEventArgs<AccelerometerReading> args)
    {
        X = args.SensorReading.Acceleration.X;
        Y = args.SensorReading.Acceleration.Y;
        Z = args.SensorReading.Acceleration.Z;
    }

    #endregion
}