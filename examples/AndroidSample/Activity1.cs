using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;

namespace AndroidSample;

[Activity(
    Label = "@string/app_name",
    MainLauncher = true,
    Icon = "@drawable/icon",
    AlwaysRetainTaskState = true,
    LaunchMode = LaunchMode.SingleInstance,
    ScreenOrientation = ScreenOrientation.FullUser,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
)]
public class Activity1 : AndroidGameActivity
{
    private Game1 _game;
    private View _view;

    protected override void OnCreate(Bundle bundle)
    {
        base.OnCreate(bundle);

        _game = new Game1();
        _view = _game.Services.GetService(typeof(View)) as View;

        SetContentView(_view);
        _game.Run();
    }

    public override void OnWindowFocusChanged(bool hasFocus)
    {
        if(hasFocus)
            SetImmersive();

        base.OnWindowFocusChanged(hasFocus);
    }

    private void SetImmersive()
    {
        if (_view == null)
            return;

        _view.SystemUiVisibility = (StatusBarVisibility)
            (SystemUiFlags.LayoutStable |
             SystemUiFlags.LayoutHideNavigation |
             SystemUiFlags.LayoutFullscreen |
             SystemUiFlags.HideNavigation |
             SystemUiFlags.Fullscreen |
             SystemUiFlags.ImmersiveSticky);
    }
}