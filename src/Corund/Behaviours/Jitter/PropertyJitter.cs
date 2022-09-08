using Corund.Engine;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Jitter;

/// <summary>
/// Base class for property jitter behaviours.
/// </summary>
public abstract class PropertyJitter<TObject, TPropBase, TProperty, TRange>: IBehaviour, IBindableBehaviour, IPropertyJitter
    where TObject: DynamicObject, TPropBase
{
    #region Constructor

    public PropertyJitter(IPropertyDescriptor<TPropBase, TProperty> descriptor, float rate, TRange range, bool isRelative)
    {
        _descriptor = descriptor;
        _isRelative = isRelative;

        Rate = rate;
        Range = range;
    }

    #endregion

    #region Fields

    /// <summary>
    /// Property getter/setter.
    /// </summary>
    private readonly IPropertyDescriptor<TPropBase, TProperty> _descriptor;

    /// <summary>
    /// Time elapsed since last jitter application.
    /// </summary>
    private float _elapsedTime;

    /// <summary>
    /// Flag indicating that the range is a fraction of the actual value, rather than an absolute.
    /// </summary>
    protected readonly bool _isRelative;

    /// <summary>
    /// Previous jitter value (to cancel out before applying a new one).
    /// </summary>
    protected TRange _lastJitter;

    #endregion

    #region Properties

    /// <summary>
    /// Number of jits per second.
    /// </summary>
    public float Rate;

    /// <summary>
    /// Jitter range.
    /// Works as a radius: the new value is modified in the range of [-x, x] for each of the components.
    /// </summary>
    public TRange Range;

    /// <summary>
    /// Name of the property handled by current jitter.
    /// </summary>
    public string PropertyName => _descriptor.Name;

    #endregion

    #region Methods

    /// <summary>
    /// Advances jitter.
    /// </summary>
    public void UpdateObjectState(DynamicObject obj)
    {
        _elapsedTime += GameEngine.Delta;
        var delay = 1 / Rate;
        if (_elapsedTime < delay)
            return;

        _elapsedTime -= delay;

        var currentValue = _descriptor.Getter((TObject) obj);
        var clearValue = Subtract(currentValue, _lastJitter);
        _lastJitter = Generate(clearValue);
        var newValue = Add(clearValue, _lastJitter);
        _descriptor.Setter((TObject) obj, newValue);
    }

    /// <summary>
    /// Attaches to the object.
    /// </summary>
    public void Bind(DynamicObject obj)
    {
        // does nothing
    }

    /// <summary>
    /// Reverts last jitter.
    /// </summary>
    public void Unbind(DynamicObject obj)
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