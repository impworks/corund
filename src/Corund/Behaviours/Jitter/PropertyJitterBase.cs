using Corund.Engine;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Jitter;

/// <summary>
/// Base class for property jitter behaviours.
/// </summary>
public abstract class PropertyJitterBase<TObject, TPropBase, TProperty, TRange>: BehaviourBase, IPropertyJitter
    where TObject: DynamicObject, TPropBase
{
    #region Constructor

    public PropertyJitterBase(IPropertyDescriptor<TPropBase, TProperty> descriptor, float delay, TRange range, bool isRelative)
    {
        _descriptor = descriptor;
        _delay = delay;
        _range = range;
        _isRelative = isRelative;
    }

    #endregion

    #region Fields

    /// <summary>
    /// Property getter/setter.
    /// </summary>
    private readonly IPropertyDescriptor<TPropBase, TProperty> _descriptor;

    /// <summary>
    /// Delay between jitter applications (in seconds).
    /// </summary>
    private readonly float _delay;

    /// <summary>
    /// Time elapsed since last jitter application.
    /// </summary>
    private float _elapsedTime;

    /// <summary>
    /// Flag indicating that the range is a fraction of the actual value, rather than an absolute.
    /// </summary>
    protected readonly bool _isRelative;

    /// <summary>
    /// Jitter range.
    /// Works as a radius: the new value is modified in the range of [-x, x] for each of the components.
    /// </summary>
    protected readonly TRange _range;

    /// <summary>
    /// Previous jitter value (to cancel out before applying a new one).
    /// </summary>
    protected TRange _lastJitter;

    #endregion

    #region Properties

    /// <summary>
    /// Name of the property handled by current jitter.
    /// </summary>
    public string PropertyName => _descriptor.Name;

    #endregion

    #region Methods

    /// <summary>
    /// Advances jitter.
    /// </summary>
    public override void UpdateObjectState(DynamicObject obj)
    {
        _elapsedTime += GameEngine.Delta;
        if (_elapsedTime < _delay)
            return;

        _elapsedTime -= _delay;

        var currentValue = _descriptor.Getter((TObject) obj);
        var clearValue = Subtract(currentValue, _lastJitter);
        _lastJitter = Generate(clearValue);
        var newValue = Add(clearValue, _lastJitter);
        _descriptor.Setter((TObject) obj, newValue);
    }

    /// <summary>
    /// Reverts last jitter.
    /// </summary>
    public override void Unbind(DynamicObject obj)
    {
        var clearValue = Subtract(_descriptor.Getter((TObject)obj), _lastJitter);
        _descriptor.Setter((TObject)obj, clearValue);
    }

    /// <summary>
    /// Combines values.
    /// </summary>
    protected abstract TProperty Add(TProperty a, TRange b);

    /// <summary>
    /// Subtracts values.
    /// </summary>
    protected abstract TProperty Subtract(TProperty a, TRange b);

    /// <summary>
    /// Creates a new jitter value.
    /// </summary>
    protected abstract TRange Generate(TProperty currentValue);

    #endregion
}