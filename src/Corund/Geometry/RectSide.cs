using System;

namespace Corund.Geometry
{
    /// <summary>
    /// The sides of a rectangle.
    /// </summary>
    [Flags]
    public enum RectSide
    {
        Left = 0x01,
        Top = 0x02,
        Right = 0x04,
        Bottom = 0x10,

        Vertical = Top | Bottom,
        Horizontal = Left | Right,

        Any = Vertical | Horizontal
    }
}
