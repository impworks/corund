using System;

namespace Corund.Tools
{
    /// <summary>
    /// Known directions.
    /// </summary>
    [Flags]
    public enum Direction
    {
        Left        = 0x001,
        Right       = 0x002,
        Up          = 0x004,
        Down        = 0x008,

        LeftUp      = 0x010,
        LeftDown    = 0x020,
        RightUp     = 0x040,
        RightDown   = 0x080,

        Horizontal  = Left | Right,
        Vertical    = Up | Down,
        Orthogonal  = Horizontal | Vertical,
        Diagonal    = LeftUp | LeftDown | RightUp | RightDown,
        All         = Orthogonal | Diagonal
    }
}
