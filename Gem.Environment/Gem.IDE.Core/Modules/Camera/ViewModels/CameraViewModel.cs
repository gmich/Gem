using System;
using System.ComponentModel.Composition;
using Gem.IDE.Core.Modules.SceneViewer.Views;
using Gemini.Framework;

namespace Gem.IDE.Core.Modules.Camera.ViewModels
{
    [Export(typeof(CameraViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
	public class CameraViewModel : Document
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

        public CameraViewModel()
        {
            DisplayName = "Camera";
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