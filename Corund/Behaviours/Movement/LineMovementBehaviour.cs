using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Movement
{
    /// <summary>
    /// Movement along a path of straight lines.
    /// </summary>
    public class LineMovementBehaviour: PathBehaviourBase<LineSegment>
    {
        #region Constructor

        public LineMovementBehaviour(IEnumerable<Vector2> points, float duration)
            : base(points, duration)
        {

        }

        #endregion

        /// <summary>
        /// Creates segments of the path.
        /// </summary>
        protected override List<LineSegment> GetSegments(IList<Vector2> points)
        {
            if(points.Count < 2)
                throw new ArgumentException("Line must contain at least 2 points.");

            var result = new List<LineSegment>(points.Count - 1);

            for (var idx = 0; idx < points.Count - 1; idx++)
                result.Add(new LineSegment(points[idx], points[idx + 1]));

            return result;
        }
    }
}
