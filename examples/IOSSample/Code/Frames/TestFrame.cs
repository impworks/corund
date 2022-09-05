using Corund.Engine;
using Corund.Frames;
using Corund.Tools.UI;
using Corund.Visuals;
using iOSSample.Code.Objects;
using Microsoft.Xna.Framework;

namespace iOSSample.Code.Frames;

internal class TestFrame: Frame
{
    public TestFrame()
    {
        BackgroundColor = Color.DarkSlateBlue;

        Add(new Alien { Position = GameEngine.Screen.Size / 2 });
        Add(new TextObject("Hello world!")
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Position = GameEngine.Screen.Size / 2 + new Vector2(0, 200),
            Scale = 4
        });
    }
}