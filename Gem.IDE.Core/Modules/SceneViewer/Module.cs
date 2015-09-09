using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Gem.IDE.Core.Modules.SceneViewer.ViewModels;
using Gemini.Framework;
using Gemini.Modules.Inspector;
using Gemini.Modules.Inspector.MonoGame;

namespace Gem.IDE.Core.Modules.SceneViewer
{
	[Export(typeof(IModule))]
	public class Module : ModuleBase
	{
	    private readonly IInspectorTool inspectorTool;

	    public override IEnumerable<IDocument> DefaultDocuments
	    {
            get { yield return new SceneViewModel(); }
	    }


        [ImportingConstructor]
	    public Module(IInspectorTool inspectorTool)
        {
            this.inspectorTool = inspectorTool;
        }

        public override void Initialize()
        {

            Shell.ActiveDocumentChanged += (sender, e) => RefreshInspector();
            RefreshInspector();
        }

        private void RefreshInspector()
        {
            if (Shell.ActiveItem != null)
                inspectorTool.SelectedObject = new InspectableObjectBuilder()
                    .WithObjectProperties(Shell.ActiveItem, pd => pd.ComponentType == Shell.ActiveItem.GetType())
                    .ToInspectableObject();
            else
                inspectorTool.SelectedObject = null;
        }

        public override void PostInitialize()
	    {
            var sceneViewModel = Shell.Documents.OfType<SceneViewModel>().FirstOrDefault();
            if (sceneViewModel != null)
                inspectorTool.SelectedObject = new InspectableObjectBuilder()
                    .WithXnaColorEditor(sceneViewModel, x => x.Color)
                    .ToInspectableObject();
            inspectorTool.DisplayName = "Colors";
        }
	}
}