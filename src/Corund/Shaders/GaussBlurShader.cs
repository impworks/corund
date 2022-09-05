using System;
using Corund.Engine;
using Corund.Tools.Render;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Shaders
{
    /// <summary>
    /// Applies a blur effect to an object.
    /// </summary>
    public class GaussBlurShader : ShaderBase
    {
        #region Constructor

        public GaussBlurShader()
            : this(new Vector2(2))
        {
            
        }

        public GaussBlurShader(Vector2 blurAmount)
        {
            if (GameEngine.EmbeddedContent == null)
                throw new ArgumentException("ContentProvider is not specified in GameEngine configuration!");

            _effect = GameEngine.EmbeddedContent.Load<Effect>("Effects/gauss-blur");

            Amount = blurAmount;
        }

        #endregion

        #region Fields

        /// <summary>
        ///  Amount of blur to be applied.
        /// </summary>
        private Vector2 _amount;

        /// <summary>
        /// Blur parameters for 1st pass (blur X).
        /// </summary>
        private BlurParameters _horizontalParameters;

        /// <summary>
        /// Blur parameters for 2ns pass (blur Y).
        /// </summary>
        private BlurParameters _verticalParameters;

        #endregion

        #region Properties

        /// <summary>
        /// Amount of blur to apply.
        /// </summary>
        public Vector2 Amount
        {
            get => _amount;
            set
            {
                var size = GameEngine.Render.Device.Viewport;
                _amount = value;
                _horizontalParameters = CreateBlurParameters(1.0f / size.Width, 0);
                _verticalParameters = CreateBlurParameters(0, 1.0f / size.Height);
            }
        }

        #endregion

        #region Methods

        public override void DrawWrapper(DynamicObject obj, Action innerDraw)
        {
            using (var rt1 = GameEngine.Render.LeaseRenderTarget())
            using (var rt2 = GameEngine.Render.LeaseRenderTarget())
            {
                // PASS 1: inner -> RT1
                {
                    using (new RenderContext(rt1.RenderTarget, Color.Transparent))
                        innerDraw();
                }

                // PASS 2: RT1 -> RT2, blur X
                {
                    using (new RenderContext(rt2.RenderTarget, Color.Transparent))
                    {
                        _effect.Parameters["WorldViewProjection"].SetValue(GameEngine.Render.WorldViewProjection);
                        _effect.Parameters["SampleWeights"].SetValue(_horizontalParameters.Weights);
                        _effect.Parameters["SampleOffsets"].SetValue(_horizontalParameters.Offsets);
                
                        GameEngine.Render.SpriteBatch.Begin(0, BlendState.AlphaBlend, GameEngine.Render.GetSamplerState(false), null, null, _effect);
                        GameEngine.Render.SpriteBatch.Draw(rt1.RenderTarget, RenderTargetRect, Color.White);
                        GameEngine.Render.SpriteBatch.End();
                    }
                }

                // PASS 3: RT -> base, blur Y
                {
                    _effect.Parameters["SampleWeights"].SetValue(_verticalParameters.Weights);
                    _effect.Parameters["SampleOffsets"].SetValue(_verticalParameters.Offsets);
                
                     GameEngine.Render.SpriteBatch.Begin(0, BlendState.AlphaBlend, GameEngine.Render.GetSamplerState(false), null, null, _effect);
                     GameEngine.Render.SpriteBatch.Draw(rt2.RenderTarget, RenderTargetRect, Color.White);
                     GameEngine.Render.SpriteBatch.End();
                }
            }
        }

        #endregion

        #region Private helpers

        private BlurParameters CreateBlurParameters(float dx, float dy)
        {
            // Look up how many samples our gaussian blur effect supports.
            var weightsParameter = _effect.Parameters["SampleWeights"];
            var sampleCount = weightsParameter.Elements.Count;

            // Create temporary arrays for computing our filter settings.
            var sampleWeights = new float[sampleCount];
            var sampleOffsets = new Vector2[sampleCount];

            // The first sample always has a zero offset.
            sampleWeights[0] = ComputeWeight(0, dx);
            sampleOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            var totalWeights = sampleWeights[0];

            // Add pairs of additional sample taps, positioned
            // along a line in both directions from the center.
            for (var i = 0; i < sampleCount / 2; i++)
            {
                // Store weights for the positive and negative taps.
                var weight = ComputeWeight(i + 1, dx);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                // To get the maximum amount of blurring from a limited number of
                // pixel shader samples, we take advantage of the bilinear filtering
                // hardware inside the texture fetch unit. If we position our texture
                // coordinates exactly halfway between two texels, the filtering unit
                // will average them for us, giving two samples for the price of one.
                // This allows us to step in units of two texels per sample, rather
                // than just one at a time. The 1.5 offset kicks things off by
                // positioning us nicely in between two texels.
                var sampleOffset = i * 2 + 1.5f;

                var delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (var i = 0; i < sampleWeights.Length; i++)
                sampleWeights[i] /= totalWeights;

            return new BlurParameters(sampleWeights, sampleOffsets);
        }

        /// <summary>
        /// Calculates the blur weight coefficient for a particular point.
        /// </summary>
        private float ComputeWeight(float n, float dx)
        {
            var theta = dx > 0 ? _amount.X : _amount.Y;
            return (float)(1.0 / Math.Sqrt(2 * Math.PI * theta) * Math.Exp(-(n * n) / (2 * theta * theta)));
        }

        #endregion

        #region BlurConfiguration structure

        private struct BlurParameters
        {
            public BlurParameters(float[] weights, Vector2[] offsets)
            {
                Weights = weights;
                Offsets = offsets;
            }

            public readonly float[] Weights;
            public readonly Vector2[] Offsets;
        }

        #endregion
    }
}
