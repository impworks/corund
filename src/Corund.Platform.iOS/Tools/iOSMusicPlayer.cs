using Corund.Engine;
using Corund.Sound;
using Microsoft.Xna.Framework.Audio;

namespace Corund.Platform.iOS.Tools
{
    /// <summary>
    /// Temporary workaround: play music as a sound effect instance.
    /// </summary>
    public class iOSMusicPlayer: IMusicPlayer
    {
        private string _assetName;
        private SoundEffectInstance _music;
        private bool _musicEnabled;

        public bool CanPlayMusic => true;
        public bool MusicEnabled
        {
            get => _musicEnabled;
            set
            {
                if (_musicEnabled == value)
                    return;

                _musicEnabled = value;
                if (value)
                    PlayMusic();
                else
                    StopMusic();
            }
        }

        public bool IsMusicPlaying => _music.State == SoundState.Playing;

        public void PlayMusic(string assetName = null, bool force = false)
        {
            if (!string.IsNullOrEmpty(assetName))
            {
                if (_music == null || assetName != _assetName)
                {
                    _music = GameEngine.Content.Load<SoundEffect>(assetName).CreateInstance();
                    _assetName = assetName;
                }
            }

            if (_music == null || !MusicEnabled)
                return;

            if (!force && (IsMusicPlaying || !CanPlayMusic))
                return;

            _music.Volume = 1;
            _music.IsLooped = true;
            _music.Play();
        }

        public void StopMusic()
        {
            if (CanPlayMusic && IsMusicPlaying)
                _music.Stop();
        }
    }
}
