using System;
using System.ComponentModel.Composition;
using Gem.IDE.Core.Modules.SceneViewer.Views;
using Gemini.Framework;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Gem.IDE.Core.Modules.SceneViewer.ViewModels
{
    [Export(typeof(SceneViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
	public class SceneViewModel : Document
    {
        private ISceneView sceneView;

       
        public override bool ShouldReopenOnStart
        {
            get { return true; }
        }

        private Color color = Color.White;
	    public Color Color
	    {
            get { return color; }
            set
            {
                color = value;
                NotifyOfPropertyChange(() => Color);

                if (sceneView != null)
                    sceneView.Invalidate();
            }
	    }

        public SceneViewModel()
        {
            DisplayName = "Free painting";
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