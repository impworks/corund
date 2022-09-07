using Microsoft.Xna.Framework;

namespace Corund.Visuals.Particles.Origins
{
    /// <summary>
    /// The definition of a shape where particles originate.
    /// </summary>
    public interface IParticleOrigin
    {
        Vector2 GetPosition();
    }
}
