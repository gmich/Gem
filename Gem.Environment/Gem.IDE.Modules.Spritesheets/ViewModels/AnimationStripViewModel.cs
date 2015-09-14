using System;
using System.ComponentModel.Composition;
using Gemini.Framework;
using Gem.IDE.Modules.SpriteSheets.Views;
using Gem.DrawingSystem.Animations;
using System.ComponentModel;

namespace Gem.IDE.Modules.SpriteSheets.ViewModels
{
    [Export(typeof(AnimationStripViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AnimationStripViewModel : Document
    {

        #region Fields

        private ISceneView sceneView;

        #endregion

        #region Ctor

        public AnimationStripViewModel()
        {
            DisplayName = "Sprite-Sheet Animations";
        }

        #endregion

        #region Helpers

        [Browsable(false)]
        public override bool ShouldReopenOnStart
        {
            get { return true; }
        }

        private AnimationStripSettings settings =>
            new AnimationStripSettings(
                FrameWidth,
                FrameHeight,
                Path,
                Name,
                FrameDelay / 1000,
                FirstFrame,
                LastFrame);

        #endregion

        #region Frame

        private string name = "My_Animation";
        [Animation]
        public string Name
        {
            get { return name; }
            set
            {
                name = value.Trim();
                NotifyOfPropertyChange(() => Name);
                sceneView?.Invalidate(settings);
            }
        }

        private int frameWidth = 32;
        [Animation]
        [DisplayName("Frame Width")]
        public int FrameWidth
        {
            get { return frameWidth; }
            set
            {
                frameWidth = (value < 1) ? 1 : value;
                NotifyOfPropertyChange(() => FrameWidth);
                sceneView?.Invalidate(settings);
            }
        }

        private int frameHeight = 32;
        [Animation]
        [DisplayName("Frame Height")]
        public int FrameHeight
        {
            get { return frameHeight; }
            set
            {
                frameHeight = (value < 1) ? 1 : value;
                NotifyOfPropertyChange(() => FrameHeight);
                sceneView?.Invalidate(settings);
            }
        }


        private int firstFrame = 0;
        [Animation]
        [DisplayName("First Frame")]
        public int FirstFrame
        {
            get { return firstFrame; }
            set
            {
                firstFrame = (value < 0 ) ? 0 : value;
                firstFrame = (firstFrame > lastFrame) ? lastFrame : firstFrame;
                NotifyOfPropertyChange(() => FirstFrame);
                sceneView?.Invalidate(settings);
            }
        }

        private int lastFrame = 0;
        [Animation]
        [DisplayName("Last Frame")]
        public int LastFrame
        {
            get { return lastFrame; }
            set
            {
                lastFrame = (value < firstFrame) ? firstFrame : value;
                NotifyOfPropertyChange(() => LastFrame);
                sceneView?.Invalidate(settings);
            }
        }

        private double frameDelay = 20;
        [Animation]
        [DisplayName("Frame Delay (ms)")]
        public double FrameDelay
        {
            get { return frameDelay; }
            set
            {
                frameDelay = (value < 0) ? 0 : value;
                NotifyOfPropertyChange(() => FrameDelay);
                sceneView?.Invalidate(settings);
            }
        }
        #endregion

        #region Presentation

        private bool animate = false;
        [Presentation]
        public bool Animate
        {
            get { return animate; }
            set
            {
                animate = value;
                NotifyOfPropertyChange(() => Animate);
                sceneView?.Invalidate(settings);
            }
        }

        #endregion

        #region Tilesheet

        [SpriteSheet]
        public string Path { get; } = "Content/tilesheet.png";

        #endregion

        #region Document Members

        protected override void OnViewLoaded(object view)
        {
            sceneView = view as ISceneView;
            sceneView.Path = Path;
            sceneView.OnGraphicsDeviceLoaded += (sender, args) => sceneView.Invalidate(settings);
            base.OnViewLoaded(view);
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                var view = GetView() as IDisposable;
                if (view != null)
                    view.Dispose();
            }

            base.OnDeactivate(close);
        }

        #endregion
    }
}