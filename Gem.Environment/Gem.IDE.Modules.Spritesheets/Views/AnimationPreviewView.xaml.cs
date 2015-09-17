using System;
using System.Windows.Controls;
using Gemini.Modules.MonoGame.Controls;
using Microsoft.Xna.Framework.Graphics;
using Gem.DrawingSystem.Animations;
using Gem.Infrastructure;
using MColor = Microsoft.Xna.Framework.Color;

namespace Gem.IDE.Modules.SpriteSheets.Views
{
    /// <summary>
    /// Interaction logic for AnimationPreviewView.xaml
    /// </summary>
    public partial class AnimationPreviewView : UserControl
    {
        private GraphicsDevice graphicsDevice;
        private AnimationStrip animation;
        private Texture2D animationStripTexture;
        private SpriteBatch batch;
        private ParallelTaskStarter updateLoop;

        public AnimationPreviewView(AnimationStripSettings settings, Texture2D animationStripTexture)
        {
            InitializeComponent();
            this.animationStripTexture = animationStripTexture;
            updateLoop = new ParallelTaskStarter(TimeSpan.FromMilliseconds(5));
            updateLoop.Start(() =>
            {
                animation?.Update(updateLoop.ElapsedTime);
                GraphicsControl?.Invalidate();
            });
        }

        public void Invalidate(AnimationStripSettings settings)
        {
            animation = new AnimationStrip(animationStripTexture.Width, animationStripTexture.Height, settings);        
        }

        public void Dispose()
        {
            updateLoop.Stop();
            GraphicsControl.Dispose();
        }

        private void OnGraphicsControlLoadContent(object sender, GraphicsDeviceEventArgs e)
        {
            graphicsDevice = e.GraphicsDevice;
            batch = new SpriteBatch(graphicsDevice);
        }

        private void OnGraphicsControlDraw(object sender, DrawEventArgs e)
        {
            e.GraphicsDevice.Clear(MColor.CornflowerBlue);
            if (animation != null)
            {
                batch.Begin();
                batch.Draw(animationStripTexture, new Microsoft.Xna.Framework.Rectangle(0, 0, animation.Frame.Width, animation.Frame.Height), animation.Frame, MColor.White);           
                batch.End();
            }
        }


    }
}
