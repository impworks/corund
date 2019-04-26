using System.Collections;
using System.Collections.Generic;
using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Visuals
{
    /// <summary>
    /// A meta-object that can contain any number of child objects.
    /// </summary>
    public class ObjectGroup: ObjectGroupBase
    {
        #region Constructors

        public ObjectGroup()
        {
        }

        public ObjectGroup(Vector2 position)
            : this()
        {
            Position = position;
        }

        public ObjectGroup(float x, float y)
            : this()
        {
            Position = new Vector2(x, y);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add an object to the visual list.
        /// </summary>
        /// <param name="obj">Object to insert.</param>
        /// <param name="toTop">Whether to put object on top or on bottom.</param>
        public virtual T Add<T>(T obj, bool toTop = true)
            where T: ObjectBase
        {
            if (obj == null || Children.Contains(obj))
                return obj;

            Attach(obj);

            if (toTop)
                Children.Insert(0, obj);
            else
                Children.Add(obj);

            return obj;
        }

        /// <summary>
        /// Adds many objects to the visual list.
        /// </summary>
        public void AddRange(params ObjectBase[] children)
        {
            AddRange(true, children);
        }

        /// <summary>
        /// Adds many objects to the visual list.
        /// </summary>
        public void AddRange(IEnumerable<ObjectBase> children)
        {
            AddRange(true, children);
        }

        /// <summary>
        /// Adds many objects to the visual list.
        /// </summary>
        /// <param name="toTop">Whether to put objects on top or on bottom.</param>
        /// <param name="children">Objects to insert.</param>
        public void AddRange(bool toTop, IEnumerable<ObjectBase> children)
        {
            foreach(var child in children)
                Add(child, toTop);
        }

        /// <summary>
        /// Inserts the object at the specified position.
        /// </summary>
        public virtual T InsertAt<T>(int idx, T obj)
            where T: ObjectBase
        {
            if (obj == null || Children.Contains(obj))
                return obj;

            Attach(obj);
            Children.Insert(idx, obj);

            return obj;
        }

        #endregion
    }
}
