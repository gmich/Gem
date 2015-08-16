using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.DrawingSystem
{
    public class RenderTargetRenderer
    {
        private readonly GraphicsDevice device;
        private readonly Action<SpriteBatch> render;

        public RenderTargetRenderer(Action<SpriteBatch> render,GraphicsDevice device,Vector2 size)
        {
            this.device = device;
            this.render = render;
            var pp = new PresentationParameters();
            Target = new RenderTarget2D(device,
                (int)size.X,
                (int)size.Y,
                false, 
                SurfaceFormat.Color, 
                DepthFormat.None,
                pp.MultiSampleCount, 
                RenderTargetUsage.DiscardContents);
        }

        public RenderTarget2D Target { get; }

        public void Render(SpriteBatch batch)
        {
            device.SetRenderTarget(Target);
            device.Clear(Color.Transparent);

            render(batch);

            device.SetRenderTarget(null);
        }
    }
}
