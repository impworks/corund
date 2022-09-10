namespace Corund.Visuals.Primitives
{
    /// <summary>
    /// A wrapper for objects that can be optional or need to be often replaced, keeping the layers intact.
    /// </summary>
    public class Placeholder<TObject>: ObjectBase
        where TObject: ObjectBase
    {
        public Placeholder(TObject value = null)
        {
            Value = value;
        }

        /// <summary>
        /// Current contents of the wrapper.
        /// </summary>
        public TObject Value;

        public override void Update() => Value?.Update();
        public override void Draw() => Value?.Draw();
    }
}
