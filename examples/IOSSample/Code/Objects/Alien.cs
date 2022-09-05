using Corund.Behaviours.Interaction;
using Corund.Shaders;
using Corund.Tools;
using Corund.Tools.Helpers;
using Corund.Tools.Interpolation;
using Corund.Tools.Properties;
using Corund.Visuals;
using Microsoft.Xna.Framework;

namespace iOSSample.Code.Objects
{
    internal class Alien: SpriteObject
    {
        public Alien()
            : base("alien")
        {
            Scale = 4;
            Rotation = 0.2f;
            Behaviours.Add(
                new DoubleTapBehaviour(() => {
                    Shader = Shader == null ? new GaussBlurShader(new Vector2(10, 1)) : null;
                }
            ));
            Behaviours.Add(
                new SwipeBehaviour(
                    KnownDirection.All,
                    sw => this.Tween(Property.Position, Position + sw.Vector, 1f, Interpolate.EaseOutSoft)
                )
            );
        }
    }
}
