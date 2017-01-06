using System;

namespace Corund.Tools
{
    /// <summary>
    /// A descriptor for property that can get and set values.
    /// </summary>
    public class PropertyDescriptor<TObject, TProperty>
    {
        #region Constructor

        public PropertyDescriptor(Func<TObject, TProperty> getter, Action<TObject, TProperty> setter, string name)
        {
            Getter = getter;
            Setter = setter;
            Name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Compiled getter function.
        /// </summary>
        public readonly Func<TObject, TProperty> Getter;

        /// <summary>
        /// Compiled setter function.
        /// </summary>
        public readonly Action<TObject, TProperty> Setter;

        /// <summary>
        /// Expression as string.
        /// </summary>
        public readonly string Name;

        #endregion
    }
}
