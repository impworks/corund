﻿using System.Collections.Generic;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours
{
    /// <summary>
    /// A utility object that handles behaviours attached to an object at runtime.
    /// </summary>
    public class BehaviourManager: List<BehaviourBase>
    {
        #region Constructor

        public BehaviourManager(DynamicObject parent)
        {
            _parent = parent;
        }

        #endregion

        #region Fields

        private readonly DynamicObject _parent;

        #endregion

        #region Update

        /// <summary>
        /// Applies all behaviours to the object.
        /// </summary>
        public void Update()
        {
            foreach (var behaviour in this)
                behaviour.UpdateObjectState(_parent);
        }

        #endregion

        #region List manipulation

        /// <summary>
        /// Adds the behaviour to the list of objects.
        /// </summary>
        public void Add(BehaviourBase behaviour)
        {
            behaviour.Bind(_parent);
            base.Add(behaviour);
        }

        /// <summary>
        /// Gets the first behaviour of specified type.
        /// Only exact matches are valid, no interfaces or base classes are supported.
        /// </summary>
        public T Get<T>() where T : BehaviourBase
        {
            var targetType = typeof(T);

            foreach (var behaviour in this)
                if (behaviour.GetType() == targetType)
                    return (T)behaviour;

            return null;
        }

        /// <summary>
        /// Removes the first behaviour of specified type.
        /// </summary>
        public void Remove<T>() where T : BehaviourBase
        {
            var targetType = typeof(T);

            for (var i = 0; i < Count; i++)
            {
                var type = this[i].GetType();
                if (type == targetType)
                {
                    RemoveAt(i);
                    return;
                }
            }
        }

        #endregion
    }
}
