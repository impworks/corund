using Corund.Engine;
using Corund.Engine.Config;
using Corund.Platform.iOS;
using iOSSample.Code.Frames;
using Microsoft.Xna.Framework;

namespace iOSSample;

public class Game1 : Game
{
    private GraphicsDeviceManager _gdm;

    public Game1()
    {
        _gdm = new GraphicsDeviceManager(this);
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        GameEngine.Init(new GameEngineOptions(this, _gdm)
        {
            Orientation = DisplayOrientation.Portrait,
            EnableAntiAliasing = false,
            PlatformAdapter = new iOSPlatformAdapter(),

            // workaround for bug: https://github.com/MonoGame/MonoGame/issues/7897
            Content = new EmbeddedContentManager(Services, new EmbeddedContentProvider(GetType().Assembly, "iOSSample.Content"))
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