using Corund.Tools.Helpers;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Visuals.UI
{
    /// <summary>
    /// A container that aligns items by a coordinate axis.
    /// </summary>
    public class StackPanel: ObjectGroup, IPanel
    {
        #region Constructor

        public StackPanel(StackOrientation orientation = StackOrientation.Down)
        {
            _orientation = orientation;
            _lastPosition = Vector2.Zero;
        }

        #endregion

        #region Fields

        private float _padding;
        private StackOrientation _orientation;
        private Vector2 _lastPosition;

        #endregion

        #region Properties

        /// <summary>
        /// Distance between neighbour elements.
        /// </summary>
        public float Padding
        {
            get => _padding;
            set
            {
                if (_padding.IsAlmost(value))
                    return;

                _padding = value;
                RefreshLayout();
            }
        }

        /// <summary>
        /// Direction of the stack's growth.
        /// </summary>
        public StackOrientation Orientation
        {
            get => _orientation;
            set
            {
                if (value == _orientation)
                    return;

                _orientation = value;
                RefreshLayout();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Recalculates the positions of all objects.
        /// </summary>
        public void RefreshLayout()
        {
            _lastPosition = Vector2.Zero;

            for (var idx = 0; idx < Children.Count; idx++)
                PlaceObject(Children[idx]);
        }

        /// <summary>
        /// Puts the object to current position.
        /// </summary>
        protected void PlaceObject(ObjectBase obj)
        {
            var iobj = obj as InteractiveObject;
            if (iobj?.Geometry == null)
                return;

            iobj.Position = _lastPosition;

            var box = iobj.Geometry.GetBoundingBox(iobj.GetTransformInfo(false));
            if (Orientation == StackOrientation.Down)
                _lastPosition.Y += (box.Height + _padding);
            else if (Orientation == StackOrientation.Right)
                _lastPosition.X += (box.Width + _padding);
            else if (Orientation == StackOrientation.Up)
                _lastPosition.Y -= (box.Height + _padding);
            else if (Orientation == StackOrientation.Left)
                _lastPosition.X -= (box.Width + _padding);
        }

        #endregion

        #region ObjectGroup overrides

        public override T Add<T>(T obj, bool toTop = true)
        {
            base.Add(obj, toTop);

            if (toTop)
                PlaceObject(obj);
            else
                RefreshLayout();

            return obj;
        }

        public override T InsertAt<T>(int idx, T elem)
        {
            base.InsertAt(idx, elem);
            RefreshLayout();
            return elem;
        }

        public override void Remove(ObjectBase obj)
        {
            var idx = Children.IndexOf(obj);
            base.Remove(obj);
            if (idx != -1)
                RefreshLayout();
        }

        public override void RemoveAt(int idx)
        {
            base.RemoveAt(idx);
            RefreshLayout();
        }

        public override void Clear()
        {
            base.Clear();
            _lastPosition = Vector2.Zero;
        }

        public override ObjectBase this[int id]
        {
            get => base[id];
            set
            {
                if (base[id] == value)
                    return;

                base[id] = value;
                RefreshLayout();
            }
        }

        #endregion
    }
}
