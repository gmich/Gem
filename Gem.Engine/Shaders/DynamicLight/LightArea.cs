using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;


namespace Gem.Engine.Shaders
{
    
    internal class LightArea
    {
        #region Fields and Properties

        private readonly GraphicsDevice graphicsDevice;
        public RenderTarget2D RenderTarget { get; }
        public Func<Vector2> Position { get; set; }
        public Vector2 AreaSize { get; set; }
        public Func<Color> LightColor { get; set; }
        public List<CastedItem> CastedItems { get; set; } = new List<CastedItem>();

        #endregion

        #region Ctor

        public LightArea(GraphicsDevice graphicsDevice, ShadowmapSize size, Func<Vector2> lightPosition, Func<Color> color)
        {
            LightColor = color;
            Position = lightPosition;
            int baseSize = 2 << (int)size;
            AreaSize = new Vector2(baseSize);
            RenderTarget = new RenderTarget2D(graphicsDevice, baseSize, baseSize);
            this.graphicsDevice = graphicsDevice;
        }

        #endregion

        #region Private Helpers

        private void BeginDrawingShadowCasters()
        {
            graphicsDevice.SetRenderTarget(RenderTarget);
            graphicsDevice.Clear(Color.Transparent);
        }

        private void EndDrawingShadowCasters()
        {
            graphicsDevice.SetRenderTarget(null);
        }

        #endregion

        #region Public Methods

        public Vector2 ToRelativePosition(Vector2 worldPosition)
        {
            return worldPosition - (Position() - AreaSize * 0.5f);
        }

        public void Cast(SpriteBatch batch, ShadowmapResolver resolver)
        {
            BeginDrawingShadowCasters();
            batch.Begin();
            CastedItems.ForEach(item =>
            {
                batch.Draw(item.Texture, ToRelativePosition(Vector2.Zero), Color.Black);
                batch.Draw(item.Texture, ToRelativePosition(item.Position), Color.Black);
            });
            batch.End();
            EndDrawingShadowCasters();
            resolver.ResolveShadows(RenderTarget, RenderTarget, Position());
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(RenderTarget, Position() - AreaSize * 0.5f, LightColor());
        }

        #endregion
    }
}
