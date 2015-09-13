using System;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using Gemini.Modules.MonoGame.Controls;
using Gemini.Modules.Output;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Gem.DrawingSystem.Animations;
using Gem.Infrastructure;
using System.Windows.Media.Imaging;
using System.Drawing;
using WindowsColor = System.Drawing.Color;
using MColor = Microsoft.Xna.Framework.Color;
using System.Threading.Tasks;

namespace Gem.IDE.Modules.SpriteSheets.Views
{
    /// <summary>
    /// Interaction logic for AnimationStripView.xaml
    /// </summary>
    public partial class AnimationStripView : UserControl, ISceneView, IDisposable
    {
        private readonly IOutput output;
        private GraphicsDevice graphicsDevice;
        private AnimationStrip animation;
        private Texture2D texture;
        private SpriteBatch batch;
        private ParallelTaskStarter updateLoop;
        private object locked = new Object();

        public AnimationStripView()
        {
            updateLoop = new ParallelTaskStarter(TimeSpan.FromMilliseconds(10));
            updateLoop.Start(() =>
            {
                animation?.Update(updateLoop.ElapsedTime);
                ReDraw();
            });
            InitializeComponent();
            output = IoC.Get<IOutput>();
        }

        public void Invalidate(AnimationStripSettings settings)
        {
            animation = new AnimationStrip(texture.Width, texture.Height, settings);
        }

        private void ReDraw()
        {
            GraphicsControl.Invalidate();
        }

        public void Dispose()
        {
            updateLoop.Stop();
            GraphicsControl.Dispose();
        }

        private async void OnGraphicsControlLoadContent(object sender, GraphicsDeviceEventArgs e)
        {
            graphicsDevice = e.GraphicsDevice;
            batch = new SpriteBatch(graphicsDevice);
        
            texture = await ConvertImage("Content/tilesheet.png");
            animation = new AnimationStrip(texture.Width, texture.Height, new AnimationStripSettings(130,150," ", "test",300));
        }


        private Task<Texture2D> ConvertImage(string path)
        {
            return Task.Run(() =>
            {
                var bmp = System.Drawing.Image.FromFile(path) as Bitmap;
                texture = new Texture2D(graphicsDevice, bmp.Width, bmp.Height);
                var pixels = new MColor[bmp.Width * bmp.Height];
                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        WindowsColor c = bmp.GetPixel(x, y);
                        pixels[(y * bmp.Width) + x] = new MColor(c.R, c.G, c.B, c.A);
                    }
                }
                texture.SetData(pixels);

                return texture;
            });
        }

        private void OnGraphicsControlDraw(object sender, DrawEventArgs e)
        {
            e.GraphicsDevice.Clear(MColor.CornflowerBlue);
            if (animation != null)
            {
                batch.Begin();
                batch.Draw(texture, new Vector2(0,100),  MColor.White);
                batch.Draw(texture, new Microsoft.Xna.Framework.Rectangle(animation.Frame.X,animation.Frame.Y+100,animation.Frame.Width,animation.Frame.Height), MColor.White);
                batch.Draw(texture, new Microsoft.Xna.Framework.Rectangle(0, 0, animation.Frame.Width, animation.Frame.Height), animation.Frame, MColor.White);
                batch.End();
            }
        }

        #region Input

        private void OnGraphicsControlMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
        }

        private void OnGraphicsControlHwndLButtonDown(object sender, MouseEventArgs e)
        {
            output.AppendLine("Mouse left button down");
            GraphicsControl.CaptureMouse();
            GraphicsControl.Focus();
        }

        private void OnGraphicsControlHwndLButtonUp(object sender, MouseEventArgs e)
        {
            output.AppendLine("Mouse left button up");
            GraphicsControl.ReleaseMouseCapture();
        }

        private void OnGraphicsControlKeyDown(object sender, KeyEventArgs e)
        {
            output.AppendLine("Key down: " + e.Key);
        }

        private void OnGraphicsControlKeyUp(object sender, KeyEventArgs e)
        {
            output.AppendLine("Key up: " + e.Key);
        }

        private void OnGraphicsControlHwndMouseWheel(object sender, MouseWheelEventArgs e)
        {
            output.AppendLine("Mouse wheel: " + e.Delta);
        }

        #endregion
    }
}
