using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Corund.Sound;

/// <summary>
/// The info about a known sound effect.
/// </summary>
public class SoundEffectInfo
{
    public SoundEffectInfo(string asset, SoundEffect eff, bool isLooped = false)
    {
        AssetName = asset;
        Effect = eff;
        IsLooped = isLooped;

        _instances = new List<SoundEffectInstance>();
    }

    #region Fields

    /// <summary>
    /// The list of instances.
    /// </summary>
    private List<SoundEffectInstance> _instances;

    #endregion

    #region Properties

    /// <summary>
    /// Asset name of the effect.
    /// </summary>
    public readonly string AssetName;

    /// <summary>
    /// The effect.
    /// </summary>
    public readonly SoundEffect Effect;

    /// <summary>
    /// Gets or sets the sound effect duration.
    /// </summary>
    public float Duration => (float)Effect.Duration.TotalSeconds;

    /// <summary>
    /// Gets or sets looping flag.
    /// </summary>
    public readonly bool IsLooped;

    /// <summary>
    /// Check if the sound is playing.
    /// </summary>
    public bool IsPlaying => RefreshInstances();

    #endregion

    #region Methods

    /// <summary>
    /// Play the sound.
    /// </summary>
    /// <param name="allowOverlap">Allow more than one instance of the sound played simultaneously.</param>
    /// <param name="volume">Volume of the sample (0..1)</param>
    public SoundEffectInstance Play(bool allowOverlap = false, float volume = 1)
    {
        if (IsPlaying && !allowOverlap)
            return _instances[0];

        RefreshInstances();

        var inst = Effect.CreateInstance();
        _instances.Add(inst);

        inst.Volume = volume;
        inst.IsLooped = IsLooped;
        inst.Play();

        return inst;
    }

    /// <summary>
    /// Stop all the instances of the sound.
    /// </summary>
    public void Stop()
    {
        foreach(var curr in _instances)
        {
            if (curr.State == SoundState.Playing)
                curr.Stop();

            curr.Dispose();
        }

        _instances.Clear();
    }

    /// <summary>
    /// Remove all finished instances from the cache.
    /// </summary>
    private bool RefreshInstances()
    {
        _instances.RemoveAll(x => x.State != SoundState.Playing);
        return _instances.Count > 0;
    }

    #endregion
}