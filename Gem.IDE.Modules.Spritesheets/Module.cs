using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Gemini.Framework;
using Gemini.Modules.Inspector;
using Gemini.Modules.Inspector.MonoGame;
using Gem.IDE.Modules.SpriteSheets.ViewModels;
using System.Reflection;

namespace Gem.IDE.Modules.SpriteSheets
{
    [Export(typeof(IModule))]
    public class Module : ModuleBase
    {
        private readonly IInspectorTool inspectorTool;

        public override IEnumerable<IDocument> DefaultDocuments
        {
            get
            {
                yield return new AnimationStripViewModel();
            }
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
            var animationStripViewModel = Shell.Documents.OfType<AnimationStripViewModel>().FirstOrDefault();
            if (animationStripViewModel != null)
                inspectorTool.SelectedObject = new InspectableObjectBuilder()
                    .WithCheckBoxEditor(animationStripViewModel, x => x.Switch)
                    .ToInspectableObject();
            inspectorTool.DisplayName = "SpriteSheet Inspector";
        }
    }
}