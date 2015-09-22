using System;
using System.ComponentModel.Composition;
using Gemini.Framework;
using Gem.IDE.Modules.SpriteSheets.Views;
using Gem.DrawingSystem.Animations;
using System.ComponentModel;
using Gemini.Modules.Inspector;
using Caliburn.Micro;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Gem.IDE.Modules.SpriteSheets.ViewModels
{
    [Export(typeof(AnimationStripViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AnimationStripViewModel : PersistedDocument
    {

        #region Fields

        private ISceneView sceneView;
        private readonly IAnimationRepository repository;
        private AnimationStripSettings settings;

        #endregion

        #region Persistence

        protected override Task DoNew()
        {
            return TaskUtility.Completed;
        }

        protected override Task DoLoad(string filePath)
        {
            return TaskUtility.Completed;
        }

        protected override Task DoSave(string filePath)
        {
            return TaskUtility.Completed;
        }

        #endregion

        #region Ctor

        public AnimationStripViewModel(string path, AnimationStripSettings settings, IAnimationRepository repository)
        {
            this.repository = repository;
            this.settings = settings;
            Path = path;
            frameWidth = settings.FrameWidth;
            frameHeight = settings.FrameHeight;
            frameDelay = settings.FrameDelay * 1000;
            lastFrame = settings.LastFrame;
            firstFrame = settings.StartFrame;
            name = settings.Name;
            DisplayName = $"{name}{Extensions.Animation}";
            SetupInspector();
        }

        #endregion

        #region Helpers

        private void SetupInspector()
        {
            var inspectorTool = IoC.Get<IInspectorTool>();
            inspectorTool.SelectedObject = new InspectableObjectBuilder()
                    .WithCollapsibleGroup("Animation", group =>
                            group.WithObjectProperties(this, model =>
                            model.Attributes.Matches(new AnimationAttribute())))
                    .WithCollapsibleGroup("Sprite sheet", group =>
                            group.WithObjectProperties(this, model =>
                            model.Attributes.Matches(new SpriteSheetAttribute())))
                    .WithCollapsibleGroup("Presentation", group =>
                            group.WithObjectProperties(this, model =>
                            model.Attributes.Matches(new PresentationAttribute())))
                   .ToInspectableObject();
            inspectorTool.DisplayName = "Sprite-Sheet Animation Inspector";
            IoC.Get<IShell>().ShowTool<IInspectorTool>();
        }

        private void Save()
        {
            repository.Save(settings);
        }

        [Browsable(false)]
        public override bool ShouldReopenOnStart
        {
            get { return true; }
        }

        private AnimationStripSettings Settings =>
           settings = new AnimationStripSettings(
                FrameWidth,
                FrameHeight,
                TileSheetWidth,
                TileSheetHeight,
                Name,
                FrameDelay / 1000,
                true,
                settings.Image,
                FirstFrame,
                LastFrame);

        private AnimationViewOptions options =>
            new AnimationViewOptions(ShowNumbers, ShowGrid, Animate, ShowTilesheet);

        #endregion

        #region Frame

        private string name;
        [Animation]
        public string Name
        {
            get { return name; }
            set
            {
                repository.Delete(name);
                name = value.Trim();
                DisplayName = name + ".animation";
                NotifyOfPropertyChange(() => Name);
                sceneView?.Invalidate(Settings);
                Save();
            }
        }

        private int frameWidth;
        [Animation]
        [DisplayName("Frame Width")]
        public int FrameWidth
        {
            get { return frameWidth; }
            set
            {
                frameWidth = (value < 1) ? 1 : value;
                NotifyOfPropertyChange(() => FrameWidth);
                sceneView?.Invalidate(Settings);
                Save();
            }
        }

        private int frameHeight;
        [Animation]
        [DisplayName("Frame Height")]
        public int FrameHeight
        {
            get { return frameHeight; }
            set
            {
                frameHeight = (value < 1) ? 1 : value;
                NotifyOfPropertyChange(() => FrameHeight);
                sceneView?.Invalidate(Settings);
                Save();
            }
        }


        private int firstFrame;
        [Animation]
        [DisplayName("First Frame")]
        public int FirstFrame
        {
            get { return firstFrame; }
            set
            {
                firstFrame = (value < 0) ? 0 : value;
                firstFrame = (firstFrame > lastFrame) ? lastFrame : firstFrame;
                NotifyOfPropertyChange(() => FirstFrame);
                sceneView?.Invalidate(Settings);
                Save();
            }
        }

        private int lastFrame;
        [Animation]
        [DisplayName("Last Frame")]
        public int LastFrame
        {
            get { return lastFrame; }
            set
            {
                lastFrame = (value < firstFrame) ? firstFrame : value;
                NotifyOfPropertyChange(() => LastFrame);
                sceneView?.Invalidate(Settings);
                Save();
            }
        }

        private double frameDelay;
        [Animation]
        [DisplayName("Frame Delay (ms)")]
        public double FrameDelay
        {
            get { return frameDelay; }
            set
            {
                frameDelay = (value < 0) ? 0 : value;
                NotifyOfPropertyChange(() => FrameDelay);
                sceneView?.Invalidate(Settings);
                Save();
            }
        }
        #endregion

        #region SpriteSheet

        [SpriteSheet]
        public string Path { get; }

        private int tilesheetWidth;
        [SpriteSheet]
        [DisplayName("TileSheet Width")]
        public int TileSheetWidth
        {
            get
            {
                return tilesheetWidth;
            }
            private set
            {
                tilesheetWidth = value;
                NotifyOfPropertyChange(() => TileSheetWidth);
            }
        }

        private int tileSheetHeight;
        [SpriteSheet]
        [DisplayName("TileSheet Height")]
        public int TileSheetHeight
        {
            get
            {
                return tileSheetHeight;
            }
            private set
            {
                tileSheetHeight = value;
                NotifyOfPropertyChange(() => TileSheetHeight);
            }
        }

        #endregion

        #region Presentation

        private bool animate;
        [Presentation]
        public bool Animate
        {
            get { return animate; }
            set
            {
                animate = value;
                NotifyOfPropertyChange(() => Animate);
                sceneView?.SetOptions(options);
            }
        }

        private bool showNumbers;
        [Presentation]
        [DisplayName("Show Numbers")]
        public bool ShowNumbers
        {
            get { return showNumbers; }
            set
            {
                showNumbers = value;
                NotifyOfPropertyChange(() => ShowNumbers);
                sceneView?.SetOptions(options);
            }
        }

        private bool showGrid;
        [Presentation]
        [DisplayName("Show Grid")]
        public bool ShowGrid
        {
            get { return showGrid; }
            set
            {
                showGrid = value;
                NotifyOfPropertyChange(() => ShowGrid);
                sceneView?.SetOptions(options);
            }
        }

        private bool showTilesheet = true;
        [Presentation]
        [DisplayName("Show Tilesheet")]
        public bool ShowTilesheet
        {
            get { return showTilesheet; }
            set
            {
                showTilesheet = value;
                NotifyOfPropertyChange(() => ShowTilesheet);
                sceneView?.SetOptions(options);
            }
        }

        private Color backgroundColor;
        [Presentation]
        [DisplayName("Background color")]
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                NotifyOfPropertyChange(() => BackgroundColor);
                sceneView.BackgroundColor = value;
            }
        }
        #endregion

        #region Document Members

        protected override void OnViewLoaded(object view)
        {
            sceneView = view as ISceneView;
            sceneView.OnGraphicsDeviceLoaded += (sender, args) =>
            {
                if (settings.Image == null)
                {
                    var result = sceneView.LoadTexture(Path);
                    TileSheetWidth = result.Item1;
                    TileSheetHeight = result.Item2;
                    settings.Image = result.Item3;
                }
                else
                {
                    TileSheetWidth = settings.TileSheetWidth;
                    TileSheetHeight = settings.TileSheetHeight;
                    sceneView.SetColorData(settings.Image, settings.TileSheetWidth, settings.TileSheetHeight);
                }
                sceneView.Invalidate(Settings);
                sceneView.SetOptions(options);
            };
            base.OnViewLoaded(view);
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                var view = GetView() as IDisposable;
                view?.Dispose();
            }

            base.OnDeactivate(close);
        }

        #endregion
    }
}