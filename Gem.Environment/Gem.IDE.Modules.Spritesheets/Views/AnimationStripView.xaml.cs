﻿#region Usings

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
using Microsoft.Xna.Framework.Content;
using Gem.IDE.Infrastructure;
using Gem.Gui.Styles;
using System.Collections.Generic;
using System.Linq;
using Gem.Engine.Containers;
using MRectangle = Microsoft.Xna.Framework.Rectangle;
using MColor = Microsoft.Xna.Framework.Color;
using Gemini.Modules.StatusBar;
using System.Windows;
using Gemini.Framework.Services;

#endregion

namespace Gem.IDE.Modules.SpriteSheets.Views
{
    /// <summary>
    /// Interaction logic for AnimationStripView.xaml
    /// </summary>
    public partial class AnimationStripView : UserControl, ISceneView
    {

        #region Fields

        private readonly IOutput output;
        private readonly IStatusBar statusBar;

        private ParallelTaskStarter updateLoop;
        private GraphicsDevice graphicsDevice;
        private AnimationStrip animation;
        private SpriteBatch batch;
        private ContentContainer container;

        private int selectedFrame = -1;
        private bool drawingSelectedFrame;
        private Action<SpriteBatch> additionalDraw = batch => { };
        private List<Tuple<int, MRectangle>> frames = new List<Tuple<int, MRectangle>>();
        private Vector2 animationPosition;
        private CoordinateViewer coordinates;
        #endregion

        #region Events

        public event EventHandler<EventArgs> OnGraphicsDeviceLoaded;

        #endregion

        #region Ctor

        public AnimationStripView()
        {
            InitializeComponent();
            output = IoC.Get<IOutput>();
            IoC.Get<IShell>().ToolBars.Visible = true;
            statusBar = IoC.Get<IStatusBar>();
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            updateLoop?.Stop();
            GraphicsControl.Dispose();
        }

        #endregion

        #region ISceneView Members

        public void Invalidate(AnimationStripSettings settings)
        {
            animation = new AnimationStrip(settings);
            var analyzer = new AnimationStripAnalyzer(settings);
            frames = analyzer.Frames.ToList();
            LoadFrameTexture(settings.FrameWidth, settings.FrameHeight);
            LoadSolidTexture(settings.FrameWidth, settings.FrameHeight);

            Width = analyzer.Width;
            Height = analyzer.Height;
            coordinates = new CoordinateViewer(graphicsDevice, MColor.Black, (int)Width, (int)Height);
            ReDraw();
        }

        public Tuple<int, int, byte[]> LoadTexture(string path)
        {
            var texture = ImageHelper.LoadAsTexture2D(graphicsDevice, path).Result;
            container.Textures.Add("SpriteSheet", x => texture);
            int bytesPerPixel = 4;  //RGBA
            var spritesheetData = new byte[texture.Width
                                     * texture.Height
                                     * bytesPerPixel];
            texture.GetData(spritesheetData);

            return new Tuple<int, int, byte[]>(texture.Width, texture.Height, spritesheetData);
        }

        public void SetColorData(byte[] data, int width, int height)
        {
            container.Textures.Add("SpriteSheet", x => new Texture2D(graphicsDevice, width, height));
            container.Textures["SpriteSheet"].SetData(data);
        }
        public void SetOptions(AnimationViewOptions options)
        {
            additionalDraw = batch => { };
            if (options.ShowTileSheet)
            {
                additionalDraw += batch => batch.Draw(container.Textures["SpriteSheet"], Vector2.Zero, MColor.White);
                if (options.ShowGrid)
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
            if (options.Animate)
            {
                if (options.ShowTileSheet)
                {
                    additionalDraw += batch => batch.Draw(container.Textures["Solid"], animation.Frame, MColor.Gray * 0.5f);
                }
                additionalDraw += DrawAnimation;
            }
            drawingSelectedFrame = !options.ShowNumbers;
            AnimateFrames(options.Animate);
            ReDraw();
        }

        private void AnimateFrames(bool animate)
        {
            if (updateLoop == null && animate)
            {
                updateLoop = new ParallelTaskStarter(TimeSpan.FromMilliseconds(5));
                animationPosition = new Vector2(animation.Settings.TileSheetWidth / 2 - animation.Settings.FrameWidth / 2,
                                                animation.Settings.TileSheetHeight / 2 - animation.Settings.FrameHeight / 2);
                updateLoop.Start(() =>
                {
                    animation.Update(updateLoop.ElapsedTime);
                    ReDraw();
                });
                output.AppendLine(
                    $"Animating frames {animation.Settings.StartFrame} - {animation.Settings.LastFrame} with {animation.Settings.FrameDelay * 1000} ms delay");
            }
            if (updateLoop != null && !animate)
            {
                updateLoop.Stop();
                updateLoop = null;
            }
        }

        #endregion

        #region Private Helpers

        private void LoadFrameTexture(int frameWidth, int frameHeight)
        {
            container.Textures.Remove("Frame");
            container.Textures.Add("Frame", x => new Texture2D(graphicsDevice, frameWidth, frameHeight));
            container.Textures["Frame"].SetData(Pattern
                                       .Border(MColor.Black, MColor.Transparent)
                                       .Get(frameWidth, frameHeight));
        }

        private void LoadSolidTexture(int frameWidth, int frameHeight)
        {
            container.Textures.Remove("Solid");
            container.Textures.Add("Solid", x => new Texture2D(graphicsDevice, frameWidth, frameHeight));
            container.Textures["Solid"].SetData(Pattern
                                       .SolidColor(MColor.White)
                                       .Get(frameWidth, frameHeight));

        }

        private void ReDraw()
        {
            GraphicsControl?.Invalidate();
        }

        private MColor GetColorByFrame(int frameIndex)
        {
            if (frameIndex >= animation.Settings.StartFrame
            && frameIndex <= animation.Settings.LastFrame)
            {
                return new MColor(22, 160, 133);
            }
            return new MColor(44, 62, 80);
        }

        #endregion

        #region Draw

        private void DrawNumbers(SpriteBatch batch)
        {
            foreach (var frame in frames)
            {
                var offset = container.Fonts["FrameNumberFont"].MeasureString(frame.Item1.ToString());
                batch.Draw(container.Textures["Solid"], new MRectangle((int)(frame.Item2.Center.X - offset.X / 2), (int)(frame.Item2.Center.Y - offset.Y / 2), (int)offset.X, (int)offset.Y), GetColorByFrame(frame.Item1));
                batch.DrawString(container.Fonts["FrameNumberFont"], frame.Item1.ToString(), new Vector2(frame.Item2.Center.X - offset.X / 2, frame.Item2.Center.Y - offset.Y / 2), MColor.White);
            }
        }

        private void DrawGrid(SpriteBatch batch)
        {
            foreach (var frame in frames)
            {
                batch.Draw(container.Textures["Frame"], new MRectangle(frame.Item2.X, frame.Item2.Y, animation.Settings.FrameWidth, animation.Settings.FrameHeight), MColor.White);
            }
        }

        private void DrawSelectedFrame(SpriteBatch batch)
        {
            if (selectedFrame == -1) return;
            foreach (var frame in frames)
            {
                if (frame.Item1 == selectedFrame)
                {
                    batch.Draw(container.Textures["Frame"], new MRectangle(frame.Item2.X, frame.Item2.Y, animation.Frame.Width, animation.Frame.Height), MColor.White);
                    var offset = container.Fonts["FrameNumberFont"].MeasureString(frame.Item1.ToString());
                    batch.Draw(container.Textures["Solid"], frame.Item2, MColor.Gray * 0.2f);
                    batch.Draw(container.Textures["Solid"], new MRectangle((int)(frame.Item2.Center.X - offset.X / 2), (int)(frame.Item2.Center.Y - offset.Y / 2), (int)offset.X, (int)offset.Y), GetColorByFrame(frame.Item1));
                    batch.DrawString(container.Fonts["FrameNumberFont"], frame.Item1.ToString(), new Vector2(frame.Item2.Center.X - offset.X / 2, frame.Item2.Center.Y - offset.Y / 2), MColor.White);
                    break;
                }
            }
        }

        private void DrawAnimation(SpriteBatch batch)
        {
            int frameOffset = 5;

            batch.Draw(container.Textures["Solid"],
                new MRectangle((int)animationPosition.X - frameOffset,
                               (int)animationPosition.Y - frameOffset,
                                animation.Frame.Width + frameOffset * 2,
                                animation.Frame.Height + frameOffset * 2),
                                MColor.DarkGreen * 0.8f);

            batch.Draw(container.Textures["SpriteSheet"],
                       new MRectangle((int)animationPosition.X, (int)animationPosition.Y, animation.Frame.Width, animation.Frame.Height),
                       animation.Frame,
                       MColor.White);
        }

        private void OnGraphicsControlDraw(object sender, DrawEventArgs e)
        {
            e.GraphicsDevice.Clear(MColor.Transparent);
            if (animation != null)
            {
                batch.Begin();

                additionalDraw(batch);
                batch.End();
                coordinates.Draw();
            }
        }

        #endregion

        #region Control Events

        private void OnGraphicsControlLoadContent(object sender, GraphicsDeviceEventArgs e)
        {
            if (graphicsDevice != null) return;

            graphicsDevice = e.GraphicsDevice;
            batch = new SpriteBatch(graphicsDevice);
            container = new ContentContainer(
                new ContentManager(
                    new Gem.IDE.Infrastructure.ServiceProvider(
                        new DeviceManager(graphicsDevice))));

            container.Fonts.Add("FrameNumberFont", content => content.Load<SpriteFont>("Content/Fonts/consoleFont"));
            OnGraphicsDeviceLoaded?.Invoke(this, EventArgs.Empty);
            output.AppendLine("Loaded animation editor");
        }

        #endregion

        #region Input

        private void OnGraphicsControlMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            coordinates.Set((int)position.X, (int)position.Y);
            statusBar.Items.Clear();
            statusBar.AddItem($"X: {position.X}  /  Y: {position.Y}", new GridLength(1, GridUnitType.Star));

            var newSelectedFrame =
                frames.FirstOrDefault(frame => frame.Item2.Contains(position.ToMonogamePoint()))
                ?.Item1 ?? -1;

            if (newSelectedFrame != selectedFrame)
            {
                selectedFrame = newSelectedFrame;
                output.AppendLine($"Frame: {selectedFrame}");
            }
            ReDraw();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            selectedFrame = -1;
            coordinates.Disable();
            ReDraw();
        }

        private void OnGraphicsControlHwndLButtonDown(object sender, MouseEventArgs e)
        {
            //GraphicsControl.CaptureMouse();
            GraphicsControl.Focus();
        }

        private void OnGraphicsControlHwndLButtonUp(object sender, MouseEventArgs e)
        {
        }

        private void OnGraphicsControlKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void OnGraphicsControlKeyUp(object sender, KeyEventArgs e)
        {
        }

        private void OnGraphicsControlHwndMouseWheel(object sender, MouseWheelEventArgs e)
        {
        }

        #endregion
    }
}
