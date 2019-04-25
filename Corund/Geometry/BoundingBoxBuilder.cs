using Microsoft.Xna.Framework;

namespace Corund.Geometry
{
    /// <summary>
    /// A helper for building a bounding box for a set of points.
    /// </summary>
    public struct BoundingBoxBuilder
    {
        #region Fields

        private bool _hasValues;
        private float _top;
        private float _left;
        private float _right;
        private float _bottom;

        #endregion

        #region Methods

        /// <summary>
        /// Adds a point to the area.
        /// </summary>
        public void AddPoint(Vector2 vec)
        {
            if (!_hasValues)
            {
                _left = _right = vec.X;
                _bottom = _top = vec.Y;
                _hasValues = true;
                return;
            }

            if (vec.X < _top)
                _top = vec.X;

            if (vec.X > _bottom)
                _bottom = vec.X;

            if (vec.Y < _left)
                _left = vec.Y;

            if (vec.Y > _right)
                _right = vec.Y;
        }

        /// <summary>
        /// Gets the bounding box rectangle.
        /// </summary>
        public Rectangle GetRectangle()
        {
            return new Rectangle(
                (int) _left,
                (int) _top,
                (int) (_right - _left),
                (int) (_bottom - _top)
            );
        }

        #endregion
    }
}
