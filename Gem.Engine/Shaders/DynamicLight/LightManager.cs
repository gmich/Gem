using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

namespace Gem.Engine.Shaders
{
    
    internal class LightManager
    {

        #region Fields

        private readonly QuadRender quadRender;
        private readonly ShadowmapResolver shadowmapResolver;
        private readonly Dictionary<string, LightArea> lightAreas = new Dictionary<string, LightArea>();
        private readonly List<CastedItem> castedItems = new List<CastedItem>();
        private readonly RenderTarget2D screenShadows;
        private readonly GraphicsDevice device;
        public Action<SpriteBatch> DrawBackground = batch => { };

        #endregion

        #region Ctor

        public LightManager(ContentManager content, GraphicsDevice device)
        {
            this.quadRender = new QuadRender(device);
            this.device = device;
            shadowmapResolver = new ShadowmapResolver(device, quadRender, ShadowmapSize.Size256, ShadowmapSize.Size1024);
            shadowmapResolver.LoadContent(content);
            screenShadows = new RenderTarget2D(device, device.Viewport.Width, device.Viewport.Height);
        }

        #endregion

        #region Public Properties
        
        public LightArea this[string tag]
        {
            get { return lightAreas[tag]; }
        }

        public IDisposable AddCastedItem(CastedItem castedItem)
        {
            castedItems.Add(castedItem);
            return Gem.Infrastructure.Disposable.Create(castedItems, castedItem);
        }

        public IEnumerable<LightArea> Lights { get { return lightAreas.Values; } }

        #endregion

        #region Private Helpers

        private void Cast(SpriteBatch batch)
        {
            device.Clear(Color.CornflowerBlue);

            foreach (var area in lightAreas.Values)
            {
                area.Cast(batch, shadowmapResolver);
            }
            device.SetRenderTarget(screenShadows);
            device.Clear(Color.Black);
            batch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            foreach (var area in lightAreas.Values)
            {
                area.Draw(batch);
            }
            batch.End();
            device.SetRenderTarget(null);

            DrawBackground.Invoke(batch);
        }

        #endregion

        #region Public Methods

        public bool AddLightArea(string tag, ShadowmapSize size, Func<Vector2> position, Func<Color> color)
        {
            if (lightAreas.ContainsKey(tag))
            {
                return false;
            }
            var lightArea = new LightArea(device, size, position, color);
            lightArea.CastedItems = castedItems;
            lightAreas.Add(tag, lightArea);
            return true;
        }

        public void Render(SpriteBatch batch)
        {
            Cast(batch);

            BlendState blendState = new BlendState();
            blendState.ColorSourceBlend = Blend.DestinationColor;
            blendState.ColorDestinationBlend = Blend.SourceColor;

            batch.Begin(SpriteSortMode.Immediate, blendState);
            batch.Draw(screenShadows, Vector2.Zero, Color.White);
            batch.End();
        }

        #endregion

    }
}
