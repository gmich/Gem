#region Usings

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
using WPoint = System.Windows.Point;
using Gemini.Modules.StatusBar;
using System.Windows;
using Gemini.Framework.Services;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

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
        private readonly int margin = 100;
        private readonly WPoint noLocation = new WPoint(-9999, -9999);

        private CameraHandler cameraHandler;
        private ParallelTaskStarter updateLoop;
        private GraphicsDevice graphicsDevice;
        private AnimationStrip animation;
        private SpriteBatch batch;
        private ContentContainer container;

        private int selectedFrame = -1;
        private bool drawingSelectedFrame;
        private Action<SpriteBatch> additionalDraw = batch => { };
        private List<Tuple<int, MRectangle>> frames = new List<Tuple<int, MRectangle>>();
        private Func<Vector2> animationPosition;
        private CoordinateViewer coordinates;
        private WPoint previousPosition;
        private Action<AnimationStripSettings, Action<AnimationStripSettings>> saveSettings;

        #endregion

        #region Events

        public event EventHandler<EventArgs> OnGraphicsDeviceLoaded;
        public event EventHandler<double> onScaleChange;

        #endregion

        #region Ctor

        public AnimationStripView()
        {
            previousPosition = noLocation;
            InitializeComponent();
            output = IoC.Get<IOutput>();
            IoC.Get<IShell>().ToolBars.Visible = true;
            statusBar = IoC.Get<IStatusBar>();
            cameraHandler = new CameraHandler(margin / 2, margin / 2, (int)Width, (int)Height);
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

        private MColor backgroundColor = MColor.Transparent;
        public MColor BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                ReDraw();
            }
        }

        public double Scale
        {
            get { return cameraHandler.Zoom; }
            set
            {
                cameraHandler.Zoom = (float)value;
                ReDraw();
            }
        }

        public void Invalidate(AnimationStripSettings settings, Action<AnimationStripSettings> saveCallback)
        {
            animation = new AnimationStrip(settings);
            var analyzer = new AnimationStripAnalyzer(settings);
            frames = analyzer.Frames.ToList();
            LoadFrameTexture(settings.FrameWidth, settings.FrameHeight);
            LoadSolidTexture(settings.FrameWidth, settings.FrameHeight);

            Width = analyzer.Width + margin;
            Height = analyzer.Height + margin;
            coordinates = new CoordinateViewer(graphicsDevice, MColor.Black, (int)Width, (int)Height);
            cameraHandler.UpdateViewport((int)Width, (int)Height);
            ReDraw();

            Save(settings, saveCallback);
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
            saveSettings = CropAndSave;
            return new Tuple<int, int, byte[]>(texture.Width, texture.Height, spritesheetData);
        }

        public void SetColorData(byte[] data, int width, int height)
        {
            saveSettings = SaveUnedited;
            container.Textures.Add("SpriteSheet", x => new Texture2D(graphicsDevice, width, height));
            container.Textures["SpriteSheet"].SetData(data);
        }

        public void SetOptions(AnimationViewOptions options)
        {
            additionalDraw = batch => { };
            if (options.ShowTileSheet)
            {
                additionalDraw += batch => batch.Draw(container.Textures["SpriteSheet"], AsCameraRelative(Vector2.Zero), MColor.White);
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
                    additionalDraw += batch => batch.Draw(container.Textures["Solid"], AsCameraRelative(animation.Frame), MColor.Gray * 0.5f);
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
                animationPosition = () => new Vector2(animation.Settings.TileSheetWidth / 2 - animation.Settings.FrameWidth / 2,
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

        #region Save
        
        private async void Save(AnimationStripSettings settings, Action<AnimationStripSettings> saveCallback)
        {
            var saveBuffer = new BufferBlock<System.Action>();
            AddToSaveQueue(saveBuffer, () => saveSettings?.Invoke(settings, saveCallback));
            await SaveAsync(saveBuffer);
        }

        private void AddToSaveQueue(ITargetBlock<System.Action> target, System.Action saveAction)
        {
            target.Post(saveAction);
            target.Complete();
        }

        private async Task<bool> SaveAsync(ISourceBlock<System.Action> source)
        {
            while (await source.OutputAvailableAsync())
            {
                var data = source.Receive();
                data.Invoke();
            }
            return true;
        }

        private void CropAndSave(AnimationStripSettings settings, Action<AnimationStripSettings> saveCallback)
        {
            int noOfFrames = (settings.LastFrame - settings.StartFrame) + 1;
            var newData = GetAnimationSubtexture(settings.Image, noOfFrames);

            saveCallback(new AnimationStripSettings(
                settings.FrameWidth,
                settings.FrameHeight,
                settings.FrameWidth * (noOfFrames),
                settings.FrameHeight,
                settings.Name,
                settings.FrameDelay,
                settings.Loop,
                newData,
                0,
                noOfFrames - 1));
        }

        private void SaveUnedited(AnimationStripSettings settings, Action<AnimationStripSettings> saveCallback)
        {
            saveCallback(settings);
        }

        private byte[] GetAnimationSubtexture(byte[] imageData, int noOfFrames)
        {       
            var croppedTexture = new Texture2D(graphicsDevice, animation.Settings.FrameWidth, animation.Settings.FrameHeight);
            return
                 frames
                    .Skip(animation.Settings.StartFrame)
                    .Take(noOfFrames)
                    .Select(frame =>
                        GetImageData(imageData, frame.Item2, croppedTexture))
                    .Aggregate((first, second) =>
                    {
                        var next = new byte[first.Length + second.Length];
                        first.CopyTo(next, 0);
                        second.CopyTo(next, first.Length);
                        return next;
                    });

            //return ConcatArrays(byteArrays);
        }
        
        private byte[] GetImageData(byte[] textureData, MRectangle subTextureBounds, Texture2D cropTexture)
        {
            int depth = 4;
            var data = new MColor[subTextureBounds.Width * subTextureBounds.Height];
            container.Textures["SpriteSheet"].GetData(
                0,
                subTextureBounds, data,
                0,
                data.Length);
            cropTexture.SetData(data);
            var cropedData = new byte[cropTexture.Width
                         * cropTexture.Height
                         * depth];
            cropTexture.GetData(cropedData);
            return cropedData;
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

        private MRectangle AsCameraRelative(MRectangle other) =>
            new MRectangle(
                other.X,
                other.Y,
                other.Width,
                other.Height);

        private Vector2 AsCameraRelative(Vector2 other) =>
            new Vector2(
                other.X,
                other.Y);

        private void DrawNumbers(SpriteBatch batch)
        {
            foreach (var frame in frames)
            {
                var offset = container.Fonts["FrameNumberFont"].MeasureString(frame.Item1.ToString());
                var borderOffset = new Vector2(
                    (offset.X > offset.Y)
                        ? offset.X : offset.Y,
                    offset.Y);

                batch.Draw(container.Textures["Solid"],
                    AsCameraRelative(
                        new MRectangle((int)(frame.Item2.Center.X - borderOffset.X / 2),
                        (int)(frame.Item2.Center.Y - borderOffset.Y / 2), (
                        int)borderOffset.X, (int)borderOffset.Y)),
                    GetColorByFrame(frame.Item1));

                batch.DrawString(container.Fonts["FrameNumberFont"],
                    frame.Item1.ToString(),
                    AsCameraRelative(
                        new Vector2(frame.Item2.Center.X - offset.X / 2,
                        frame.Item2.Center.Y - offset.Y / 2)),
                    MColor.White);
            }
        }

        private void DrawGrid(SpriteBatch batch)
        {
            foreach (var frame in frames)
            {
                batch.Draw(container.Textures["Frame"],
                    AsCameraRelative(
                        new MRectangle(
                            frame.Item2.X,
                            frame.Item2.Y,
                            animation.Settings.FrameWidth,
                            animation.Settings.FrameHeight)),
                    MColor.White);
            }
        }

        private void DrawSelectedFrame(SpriteBatch batch)
        {
            if (selectedFrame == -1) return;
            foreach (var frame in frames)
            {
                if (frame.Item1 == selectedFrame)
                {
                    batch.Draw(
                        container.Textures["Frame"],
                        AsCameraRelative(
                            new MRectangle(
                                frame.Item2.X,
                                frame.Item2.Y,
                                animation.Frame.Width,
                                animation.Frame.Height)),
                            MColor.White);
                    var offset = container.Fonts["FrameNumberFont"]
                        .MeasureString(frame.Item1.ToString());

                    var borderOffset = new Vector2(
                        (offset.X > offset.Y)
                            ? offset.X : offset.Y,
                        offset.Y);

                    batch.Draw(container.Textures["Solid"],
                        AsCameraRelative(
                            frame.Item2),
                            MColor.Gray * 0.2f);

                    batch.Draw(container.Textures["Solid"],
                        AsCameraRelative(
                            new MRectangle(
                                (int)(frame.Item2.Center.X - borderOffset.X / 2),
                                (int)(frame.Item2.Center.Y - borderOffset.Y / 2),
                                (int)borderOffset.X,
                                (int)borderOffset.Y)),
                        GetColorByFrame(frame.Item1));

                    batch.DrawString(container.Fonts["FrameNumberFont"],
                        frame.Item1.ToString(),
                        AsCameraRelative(
                            new Vector2(
                                frame.Item2.Center.X - offset.X / 2,
                                frame.Item2.Center.Y - offset.Y / 2)),
                        MColor.White);
                    break;
                }
            }
        }

        private void DrawAnimation(SpriteBatch batch)
        {
            //int frameOffset = 5;
            //batch.Draw(container.Textures["Solid"],
            //    new MRectangle((int)animationPosition.X - frameOffset,
            //                   (int)animationPosition.Y - frameOffset,
            //                    animation.Frame.Width + frameOffset * 2,
            //                    animation.Frame.Height + frameOffset * 2),
            //                    MColor.DarkGreen * 0.8f);

            var position = animationPosition();
            batch.Draw(container.Textures["SpriteSheet"],
                       AsCameraRelative(
                           new MRectangle(
                               (int)position.X,
                               (int)position.Y,
                               animation.Frame.Width,
                               animation.Frame.Height)),
                       animation.Frame,
                       MColor.White);
        }

        private void OnGraphicsControlDraw(object sender, DrawEventArgs e)
        {
            e.GraphicsDevice.Clear(BackgroundColor);
            if (animation != null)
            {
                batch.Begin(transformMatrix: cameraHandler.Matrix);

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
            //cameraHandler = new CameraHandler((int)Width -margin / 2,(int)Height -margin / 2, (int)Width, (int)Height);
        }

        #endregion

        #region Input

        private void OnGraphicsControlMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            var worldPosition = cameraHandler.Camera.TranslateScreenToWorld(position.ToMonogameVector2());
            coordinates.Set((int)position.X, (int)position.Y);
            statusBar.Items.Clear();
            statusBar.AddItem($"X: {(int)worldPosition.X}  /  Y: {(int)worldPosition.Y}", new GridLength(1, GridUnitType.Star));

            var newSelectedFrame =
                frames.FirstOrDefault(frame => (frame.Item2).Contains(worldPosition))
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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                var position = e.GetPosition(this);
                if (previousPosition != noLocation)
                {
                    var worldPosition = cameraHandler.Camera.TranslateScreenToWorld(position.ToMonogameVector2());
                    var worldPreviousPosition = cameraHandler.Camera.TranslateScreenToWorld(previousPosition.ToMonogameVector2());
                    cameraHandler.Camera.Position += (worldPosition - worldPreviousPosition);
                }
                previousPosition = position;
            }
            else
            {
                previousPosition = noLocation;
            }
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
            Scale += (e.Delta > 0) ? 0.05f : -0.05f;
            onScaleChange?.Invoke(this, cameraHandler.Zoom);
        }

        #endregion
    }
}
