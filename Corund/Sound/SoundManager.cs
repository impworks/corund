using System.Collections.Generic;
using Corund.Engine;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Corund.Sound
{
    /// <summary>
    /// The class that handles sound playback.
    /// </summary>
    public class SoundManager
    {
        #region Constructor

        public SoundManager()
        {
            _soundCache = new Dictionary<string, SoundEffectInfo>();
        }

        #endregion

        #region Fields

        private bool _soundEnabled;
        private bool _musicEnabled;

        /// <summary>
        /// List of sound effect instances currently initialized.
        /// </summary>
        private readonly Dictionary<string, SoundEffectInfo> _soundCache;

        /// <summary>
        /// The music effect.
        /// </summary>
        private Song _music;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the flag indicating that game is in focus and can play sounds.
        /// </summary>
        public bool CanPlayMusic => MediaPlayer.GameHasControl;

        /// <summary>
        /// Gets or sets sound playback availability.
        /// </summary>
        public bool SoundEnabled
        {
            get { return _soundEnabled; }
            set
            {
                _soundEnabled = value;
                if (!value)
                    StopAllSounds();
            }
        }

        /// <summary>
        /// Gets or sets background music availability.
        /// </summary>
        public bool MusicEnabled
        {
            get { return _musicEnabled; }
            set
            {
                _musicEnabled = value;

                if (value)
                    PlayMusic();
                else
                    StopMusic();
            }
        }

        #endregion

        #region Sounds

        /// <summary>
        /// Load a sound to the current
        /// </summary>
        /// <param name="assetName">Sound asset name.</param>
        public void LoadSound(string assetName)
        {
            if (_soundCache.ContainsKey(assetName))
                return;

            var effect = GameEngine.Content.Load<SoundEffect>(assetName);
            var sound = new SoundEffectInfo(assetName, effect);
            _soundCache.Add(assetName, sound);
        }

        /// <summary>
        /// Play the sound.
        /// </summary>
        /// <param name="assetName">Sound effect's asset name.</param>
        /// <param name="allowOverlap">Whether many instances of the same sound can be played simultaneously or not.</param>
        /// <param name="volume">Volume of the sample (0..1).</param>
        public void PlaySound(string assetName, bool allowOverlap = false, float volume = 1)
        {
            if (!SoundEnabled)
                return;

            LoadSound(assetName);
            var sound = _soundCache[assetName];

            sound.Play(allowOverlap, volume);
        }

        /// <summary>
        /// Checks if sound is playing.
        /// </summary>
        public bool IsSoundPlaying(string assetName)
        {
            return _soundCache.ContainsKey(assetName) && _soundCache[assetName].IsPlaying;
        }

        /// <summary>
        /// Checks if any sound is playing.
        /// </summary>
        public bool IsAnySoundPlaying()
        {
            if (_soundCache.Count > 0)
                foreach (var curr in _soundCache.Values)
                    if (curr.IsPlaying)
                        return true;

            return false;
        }

        /// <summary>
        /// Stop a sound playing.
        /// </summary>
        public void StopSound(string assetName)
        {
            if (!_soundCache.ContainsKey(assetName))
                return;

            var sound = _soundCache[assetName];
            sound.Stop();
        }

        /// <summary>
        /// Stop all sounds playing.
        /// </summary>
        public void StopAllSounds()
        {
            foreach (var sound in _soundCache.Values)
                sound.Stop();
        }

        #endregion

        #region Music

        /// <summary>
        /// Load the music.
        /// </summary>
        public void LoadMusic(string assetName)
        {
            _music = GameEngine.Content.Load<Song>(assetName);
        }

        /// <summary>
        /// Play a background music.
        /// </summary>
        public void PlayMusic(bool force = false)
        {
            if (!MusicEnabled || _music == null)
                return;

            if (!CanPlayMusic && !force)
                return;

            if(IsMusicPlaying() && !force)
                return;

            MediaPlayer.Play(_music);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 1;
        }

        /// <summary>
        /// Stop music playing.
        /// </summary>
        public void StopMusic()
        {
            if (CanPlayMusic && IsMusicPlaying())
                MediaPlayer.Pause();
        }

        /// <summary>
        /// Checks if music is playing.
        /// </summary>
        public bool IsMusicPlaying()
        {
            return MediaPlayer.State == MediaState.Playing;
        }

        #endregion
    }
}
