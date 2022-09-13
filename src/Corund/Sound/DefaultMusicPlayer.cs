using Corund.Engine;
using Microsoft.Xna.Framework.Media;

namespace Corund.Sound;

/// <summary>
/// Default implementation of MusicPlayer based on MonoGame's MediaPlayer.
/// </summary>
public class DefaultMusicPlayer: IMusicPlayer
{
    #region Fields

    private Song _song;
    private string _songAsset;
    private bool _musicEnabled;

    #endregion

    #region Properties

    public bool CanPlayMusic => MediaPlayer.GameHasControl;

    public bool MusicEnabled
    {
        get => _musicEnabled;
        set
        {
            if (_musicEnabled == value)
                return;

            _musicEnabled = value;
            if(value)
                PlayMusic();
            else
                StopMusic();
        }
    }

    public bool IsMusicPlaying => MediaPlayer.State == MediaState.Playing;

    #endregion

    #region Public methods

    public void PlayMusic(string assetName = null, bool force = false)
    {
        if(!string.IsNullOrEmpty(assetName))
            LoadAsset(assetName);

        if (_song == null || !MusicEnabled)
            return;

        if (!force && (IsMusicPlaying || !CanPlayMusic))
            return;

        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 1;
        MediaPlayer.Play(_song);
    }

    public void StopMusic()
    {
        if(CanPlayMusic && IsMusicPlaying)
            MediaPlayer.Pause();
    }

    #endregion

    #region Private helpers
        
    /// <summary>
    /// Loads the song, unloading the previous one if necessary.
    /// </summary>
    private void LoadAsset(string asset)
    {
        if (_songAsset != null)
        {
            GameEngine.Content.UnloadAsset(_songAsset);
            _song?.Dispose();
        }

        _songAsset = asset;
        _song = GameEngine.Content.Load<Song>(asset);
    }

    #endregion
}