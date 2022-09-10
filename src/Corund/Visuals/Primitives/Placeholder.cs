namespace Corund.Visuals.Primitives
{
    /// <summary>
    /// A wrapper for objects that can be optional or need to be often replaced, keeping the layers intact.
    /// </summary>
    public class Placeholder<TObject>: ObjectBase
        where TObject: ObjectBase
    {
        private TObject _value;

        public Placeholder(TObject value = null)
        {
            Value = Attach(value);
        }

        /// <summary>
        /// Current contents of the wrapper.
        /// </summary>
        public TObject Value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;

                if(_value != null)
                    _value.Parent = null;

                _value = Attach(value);
            }
        }

        public override void Update() => Value?.Update();
        public override void Draw() => Value?.Draw();
    }
}
