using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Movement
{
    /// <summary>
    /// Movement along a path of straight lines.
    /// </summary>
    public class BezierMovementBehaviour : PathBehaviourBase<BezierSegment>
    {
        #region Constructor

        public BezierMovementBehaviour(IEnumerable<Vector2> points, float duration)
            : base(points, duration)
        {

        }

        #endregion

        /// <summary>
        /// Creates segments of the path.
        /// </summary>
        protected override List<BezierSegment> GetSegments(IList<Vector2> points)
        {
            if (points.Count < 2)
                throw new ArgumentException("Path must contain an odd number of points (at least 3).");

            var result = new List<BezierSegment>((points.Count - 1) / 2);

            for (var idx = 0; idx < points.Count - 2; idx+=2)
                result.Add(new BezierSegment(points[idx], points[idx + 1], points[idx + 2]));

            return result;
        }
    }
}
