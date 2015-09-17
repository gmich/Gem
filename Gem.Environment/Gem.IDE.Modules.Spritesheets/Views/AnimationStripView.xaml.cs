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
using MRectangle = Microsoft.Xna.Framework.Rectangle;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Gem.IDE.Modules.Spritesheets;
using Gem.IDE.Infrastructure;
using Gem.Gui.Styles;
using System.Collections.Generic;
using System.Linq;
using Gem.IDE.Modules.SpriteSheets.ViewModels;

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
        public event EventHandler<EventArgs> OnGraphicsDeviceLoaded;
        private Texture2D frameTexture;
        private Texture2D solidTexture;
        private SpriteFont font;
        private List<Tuple<int, MRectangle>> frames = new List<Tuple<int, MRectangle>>();
        private int selectedFrame = -1;
        private Action<SpriteBatch> additionalDraw = batch => { };

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
            frames = new AnimationStripAnalyzer(texture.Width, texture.Height, settings).Frames.ToList();
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
            var contentManager = new ContentManager(new ServiceProvider(new DeviceManager(graphicsDevice)));
            texture = contentManager.Load<Texture2D>(Path);
            font = contentManager.Load<SpriteFont>("Content/Fonts/detailsFont");
            OnGraphicsDeviceLoaded?.Invoke(this, EventArgs.Empty);
        }

        private void OnGraphicsControlDraw(object sender, DrawEventArgs e)
        {
            e.GraphicsDevice.Clear(MColor.Transparent);
            if (animation != null)
            {
                batch.Begin();
                batch.Draw(texture, Vector2.Zero, MColor.White);
                additionalDraw(batch);
                batch.Draw(solidTexture, new MRectangle(animation.Frame.X, animation.Frame.Y, animation.Frame.Width, animation.Frame.Height), MColor.Gray * 0.5f);
                batch.End();
            }
        }

        private void DrawNumbers(SpriteBatch batch)
        {
            foreach (var frame in frames)
            {
                var offset = font.MeasureString(frame.Item1.ToString());
                batch.Draw(solidTexture, new MRectangle((int)(frame.Item2.Center.X - offset.X / 2), (int)(frame.Item2.Center.Y - offset.Y / 2), (int)offset.X, (int)offset.Y), MColor.White);
                batch.DrawString(font, frame.Item1.ToString(), new Vector2(frame.Item2.Center.X - offset.X / 2, frame.Item2.Center.Y - offset.Y / 2), GetColorByFrame(frame.Item1));
            }
        }

        private void DrawGrid(SpriteBatch batch)
        {
            foreach (var frame in frames)
            {
                batch.Draw(frameTexture, new MRectangle(frame.Item2.X, frame.Item2.Y, animation.Frame.Width, animation.Frame.Height), MColor.White);
            }
        }

        private void DrawSelectedFrame(SpriteBatch batch)
        {
            if(selectedFrame==-1) return;
            foreach (var frame in frames)
            {
               if(frame.Item1==selectedFrame)
                {
                    batch.Draw(frameTexture, new MRectangle(frame.Item2.X, frame.Item2.Y, animation.Frame.Width, animation.Frame.Height), MColor.White);
                    var offset = font.MeasureString(frame.Item1.ToString());
                    batch.Draw(solidTexture, new MRectangle((int)(frame.Item2.Center.X - offset.X / 2), (int)(frame.Item2.Center.Y - offset.Y / 2), (int)offset.X, (int)offset.Y), MColor.White);
                    batch.DrawString(font, frame.Item1.ToString(), new Vector2(frame.Item2.Center.X - offset.X / 2, frame.Item2.Center.Y - offset.Y / 2), GetColorByFrame(frame.Item1));
                    break;
                }
            }
        }

        public void SetOptions(AnimationViewOptions options)
        {
            additionalDraw = (batch) => { };
            if(options.ShowGrid)
            {
                additionalDraw += DrawGrid;
            }
            if (options.ShowNumbers)
            {
                additionalDraw += DrawNumbers;
            }
            else
            {
                additionalDraw += DrawSelectedFrame;
            }
        }
        
        private MColor GetColorByFrame(int frameIndex)
        {
            if (frameIndex >= animation.Settings.StartFrame
            && frameIndex <= animation.Settings.LastFrame)
            {
                return MColor.Red;
            }
            return MColor.Black;
        }
        
        #region Input

        private void OnGraphicsControlMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);

            selectedFrame = -1;

            foreach (var frame in frames)
            {
                if (frame.Item2.Contains(position.ToMonogamePoint()))
                {
                    selectedFrame = frame.Item1;
                    return;
                }
            }

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
