using Corund.Tools;
using Microsoft.Xna.Framework;

namespace Corund.Geometry
{
    /// <summary>
    /// A set of rectangles around comprising a single shape.
    /// </summary>
    public class GeometryRectGroup: IGeometry
    {
        #region Constructor

        public GeometryRectGroup(params GeometryRect[] rects)
        {
            Rectangles = rects;
        }

        #endregion

        #region Fields

        /// <summary>
        /// List of rectangles in the group.
        /// </summary>
        public readonly GeometryRect[] Rectangles;

        #endregion

        #region IGeometry implementation

        /// <summary>
        /// Checks if any of the rectangles in the group contain the point.
        /// </summary>
        public bool ContainsPoint(Vector2 point, TransformInfo selfTransform)
        {
            for(var idx = 0; idx < Rectangles.Length; idx++)
            {
                if (Rectangles[idx].ContainsPoint(point, selfTransform))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the group overlaps another geometry.
        /// </summary>
        public bool Overlaps(IGeometry other, TransformInfo selfTransform, TransformInfo otherTransform)
        {
            var otherRect = other as GeometryRect;
            if (otherRect != null)
                return Overlaps(otherRect, selfTransform, otherTransform);

            var otherGroup = other as GeometryRectGroup;
            if (otherGroup != null)
                return Overlaps(otherGroup, selfTransform, otherTransform);

            return other.Overlaps(this, otherTransform, selfTransform);
        }

        /// <summary>
        /// Checks if the group overlaps another rectangle.
        /// </summary>
        public bool Overlaps(GeometryRect other, TransformInfo selfTransform, TransformInfo otherTransform)
        {
            var otherPoly = other.CreateRectPolygon(otherTransform);

            for (var idx = 0; idx < Rectangles.Length; idx++)
            {
                var poly = Rectangles[idx].CreateRectPolygon(selfTransform);
                if (CollisionDetector.AreRectsOverlapping(poly, otherPoly))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the group overlaps another rectangle.
        /// </summary>
        public bool Overlaps(GeometryRectGroup other, TransformInfo selfTransform, TransformInfo otherTransform)
        {
            // pre-transform all rects
            var polys = new RectPolygon[Rectangles.Length];

            for (var idx = 0; idx < Rectangles.Length; idx++)
                polys[idx] = Rectangles[idx].CreateRectPolygon(selfTransform);

            // check any-x-any overlap
            for (var idx = 0; idx < other.Rectangles.Length; idx++)
            {
                var otherPoly = other.Rectangles[idx].CreateRectPolygon(otherTransform);
                for (var idx2 = 0; idx2 < polys.Length; idx2++)
                {
                    if (CollisionDetector.AreRectsOverlapping(polys[idx2], otherPoly))
                        return true;
                }
            }

            return false;
        }

        #endregion
    }
}
