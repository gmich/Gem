using System;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using Gemini.Modules.MonoGame.Controls;
using Gemini.Modules.Output;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel.Composition;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using Gem.DrawingSystem.Animations;
using Gem.Infrastructure;

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
        private int textureWidth = 100;
        private int textureHeight = 20;
        private SpriteBatch batch;
        private ParallelTaskStarter updateLoop;

        public AnimationStripView()
        {
            updateLoop = new ParallelTaskStarter(TimeSpan.Zero);
            updateLoop.Start(() => animation.Update(updateLoop.ElapsedTime));
            InitializeComponent();
            output = IoC.Get<IOutput>();
        }

        public void Invalidate(AnimationStripSettings settings)
        {
            animation = new AnimationStrip(textureWidth, textureHeight, settings);
            ReDraw();
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
                
        private void OnGraphicsControlLoadContent(object sender, GraphicsDeviceEventArgs e)
        {
            graphicsDevice = e.GraphicsDevice;
            batch = new SpriteBatch(graphicsDevice);
            texture = new Texture2D(graphicsDevice, textureWidth, textureHeight);

            var color = new Color[textureWidth * textureHeight];
            for(int i=0;i<color.Length;i++)
            {
                color[i] = Color.Black;
            }

            texture.SetData(color);
        }

        private void OnGraphicsControlDraw(object sender, DrawEventArgs e)
        {
            e.GraphicsDevice.Clear(Color.CornflowerBlue);
            if (animation != null)
            {               
                batch.Begin();
                batch.Draw(texture, animation.Frame, Color.White);
                batch.End();
            }
        }

        #region Input

        private void OnGraphicsControlMouseMove(object sender, MouseEventArgs e)
        {
            ReDraw();
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
