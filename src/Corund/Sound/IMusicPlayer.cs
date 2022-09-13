namespace Corund.Sound
{
    /// <summary>
    /// Interface for platform-specific music players.
    /// </summary>
    public interface IMusicPlayer
    {
        /// <summary>
        /// Flag indicating system allows the app to play music in current state.
        /// </summary>
        bool CanPlayMusic { get; }

        /// <summary>
        /// Flag indicating that music should be played.
        /// </summary>
        bool MusicEnabled { get; set; }

        /// <summary>
        /// Flag indicating that music is currently playing.
        /// </summary>
        bool IsMusicPlaying { get; }

        /// <summary>
        /// Plays the specified asset (or a previously paused song).
        /// </summary>
        void PlayMusic(string assetName = null, bool force = false);

        /// <summary>
        /// Stops currently playing music.
        /// </summary>
        void StopMusic();
    }
}
