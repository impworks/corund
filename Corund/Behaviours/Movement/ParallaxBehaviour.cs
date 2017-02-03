using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Movement
{
    /// <summary>
    /// Positions object relative to camera with a coefficient.
    /// </summary>
    public class ParallaxBehaviour: BehaviourBase
    {
        #region Constructor

        /// <summary>
        /// Creates a new ParallaxBehaviour.
        /// </summary>
        /// <param name="coefficient">
        /// Parallax coefficient.
        /// 0 = object is static, never moves.
        /// 1 = object moves as usual.
        /// 2 = object moves twice as fast.
        /// </param>
        public ParallaxBehaviour(float coefficient = 1)
        {
            _coefficient = coefficient;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Parallax coefficient.
        /// </summary>
        private readonly float _coefficient;

        #endregion

        #region Methods

        public override void UpdateObjectState(DynamicObject obj)
        {
            // todo
        }

        #endregion
    }
}
