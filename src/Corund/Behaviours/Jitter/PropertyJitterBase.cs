using Corund.Engine;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Jitter
{
    /// <summary>
    /// Base class for property jitter behaviours.
    /// </summary>
    public abstract class PropertyJitterBase<TObject, TPropBase, TProperty>: BehaviourBase, IPropertyJitter
        where TObject: DynamicObject, TPropBase
    {
        #region Constructor

        public PropertyJitterBase(IPropertyDescriptor<TPropBase, TProperty> descriptor, float delay)
        {
            _descriptor = descriptor;
            _delay = delay;
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

            var clearValue = CancelPrevious(_descriptor.Getter((TObject)obj));
            _descriptor.Setter((TObject) obj, ApplyNew(clearValue));
        }

        /// <summary>
        /// Reverts last jitter.
        /// </summary>
        public override void Unbind(DynamicObject obj)
        {
            var clearValue = CancelPrevious(_descriptor.Getter((TObject)obj));
            _descriptor.Setter((TObject)obj, clearValue);
        }

        /// <summary>
        /// Cancels out previous jitter.
        /// </summary>
        protected abstract TProperty CancelPrevious(TProperty value);

        /// <summary>
        /// Applies new jitter.
        /// </summary>
        protected abstract TProperty ApplyNew(TProperty value);

        #endregion
    }
}
