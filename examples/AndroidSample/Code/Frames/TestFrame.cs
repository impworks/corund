using AndroidSample.Code.Objects;
using Corund.Engine;
using Corund.Engine.Prompts;
using Corund.Frames;
using Corund.Tools;
using Corund.Tools.Helpers;
using Corund.Tools.UI;
using Corund.Visuals;
using Microsoft.Xna.Framework;

namespace AndroidSample.Code.Frames;

internal class TestFrame: Frame
{
    public TestFrame()
    {
        BackgroundColor = Color.DarkSlateBlue;

        Add(new Alien { Position = GameEngine.Screen.Size / 2 });
        _text = Add(new TextObject("Hello world!")
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Position = GameEngine.Screen.Size / 2 + new Vector2(0, 200),
            Scale = 4
        });
    }

    private TextObject _text;

    public override void Update()
    {
        base.Update();

        if (_text.HasTouch())
        {
            GameEngine.Prompt.TextInput(
                new TextInputPromptOptions
                {
                    Header = "Input",
                    Description = "Enter new value",
                    DefaultValue = _text.Text,
                    OnConfirm = str => _text.Text = str,
                    PauseMode = PauseMode.All
                }
            );
        }
    }
}