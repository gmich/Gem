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
using Microsoft.Xna.Framework.Content;
using Gem.IDE.Modules.Spritesheets;
using Gem.IDE.Infrastructure;
using Gem.Gui.Styles;

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
        private ContentManager contentManager;
        public event EventHandler<EventArgs> OnGraphicsDeviceLoaded;
        private Texture2D frameTexture;
        private Texture2D solidTexture;
        private SpriteFont font;

        public AnimationStripView()
        {
            updateLoop = new ParallelTaskStarter(TimeSpan.FromMilliseconds(5));
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

            frameTexture = new Texture2D(graphicsDevice, settings.FrameWidth, settings.FrameHeight);
            frameTexture.SetData(Pattern
                        .Border(MColor.Black, MColor.Transparent)
                        .Get(settings.FrameWidth, settings.FrameHeight));

            solidTexture = new Texture2D(graphicsDevice, settings.FrameWidth, settings.FrameHeight);
            solidTexture.SetData(Pattern
                        .SolidColor(MColor.White)
                        .Get(settings.FrameWidth, settings.FrameHeight));
        }

        public string Path { get; set; } = string.Empty;

        private void ReDraw()
        {
            GraphicsControl?.Invalidate();
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

            //texture = await ConvertImage("Content/tilesheet.png");
            contentManager = new ContentManager(new ServiceProvider(new DeviceManager(graphicsDevice)));
            texture = contentManager.Load<Texture2D>(Path);
            font = contentManager.Load<SpriteFont>("Content/Fonts/menuFont");
            OnGraphicsDeviceLoaded?.Invoke(this, EventArgs.Empty);
        }

        private void OnGraphicsControlDraw(object sender, DrawEventArgs e)
        {
            e.GraphicsDevice.Clear(MColor.CornflowerBlue);
            if (animation != null)
            {
                batch.Begin();
                batch.Draw(texture, new Vector2(0, 160), MColor.White);
                batch.Draw(texture, new Microsoft.Xna.Framework.Rectangle(0, 0, animation.Frame.Width, animation.Frame.Height), animation.Frame, MColor.White);

                foreach (var frame in animation.ParseToEnd())
                {
                    batch.Draw(frameTexture, new Microsoft.Xna.Framework.Rectangle(frame.Item2.X, frame.Item2.Y + 160, animation.Frame.Width, animation.Frame.Height), MColor.White);
                    var offset = font.MeasureString(frame.Item1.ToString());
                    batch.Draw(solidTexture, new Microsoft.Xna.Framework.Rectangle((int)(frame.Item2.Center.X - offset.X / 2), (int)(frame.Item2.Center.Y + 160 - offset.Y / 2), (int)offset.X, (int)offset.Y), MColor.White);
                    batch.DrawString(font, frame.Item1.ToString(), new Vector2(frame.Item2.Center.X - offset.X / 2, frame.Item2.Center.Y + 160 - offset.Y / 2), MColor.Black);
                }
                batch.Draw(solidTexture, new Microsoft.Xna.Framework.Rectangle(animation.Frame.X, animation.Frame.Y + 160, animation.Frame.Width, animation.Frame.Height), MColor.Gray * 0.3f);
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
