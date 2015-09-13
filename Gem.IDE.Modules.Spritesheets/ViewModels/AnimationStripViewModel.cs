using System;
using System.ComponentModel.Composition;
using Gemini.Framework;
using Gem.IDE.Modules.SpriteSheets.Views;

namespace Gem.IDE.Modules.SpriteSheets.ViewModels
{
    [Export(typeof(AnimationStripViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
	public class AnimationStripViewModel : Document
    {
        private ISceneView sceneView;
               
        public override bool ShouldReopenOnStart
        {
            get { return true; }
        }

        private bool _switch = false;
	    public bool Switch
	    {
            get { return _switch; }
            set
            {
                _switch = value;
                NotifyOfPropertyChange(() => Switch);

                if (sceneView != null)
                    sceneView.Invalidate();
            }
	    }

        public AnimationStripViewModel()
        {
            DisplayName = "SpriteSheets";
        }

        protected override void OnViewLoaded(object view)
        {
            sceneView = view as ISceneView;
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
	}
}