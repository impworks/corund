using AndroidSample.Code.Frames;
using Corund.Engine.Config;
using Corund.Engine;
using Corund.Platform.Android;
using Microsoft.Xna.Framework;

namespace AndroidSample;

public class Game1 : Game
{
    private GraphicsDeviceManager _gdm;

    public Game1()
    {
        _gdm = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        GameEngine.Init(new GameEngineOptions(this, _gdm)
        {
            Orientation = DisplayOrientation.Portrait,
            EnableAntiAliasing = false,
            PlatformAdapter = new AndroidPlatformAdapter()
        });

        GameEngine.Frames.Add(new TestFrame());
        base.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        GameEngine.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GameEngine.Draw(gameTime);
        base.Draw(gameTime);
    }
}