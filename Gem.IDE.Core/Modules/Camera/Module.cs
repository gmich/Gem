using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Gemini.Framework;
using Gemini.Modules.Inspector;
using Gemini.Modules.Inspector.MonoGame;
using Gemini.Modules.Inspector.Inspectors;
using System;

namespace Gem.IDE.Core.Modules.Camera
{
    [Export(typeof(IModule))]
    public class Module : ModuleBase
    {
        private readonly IInspectorTool inspectorTool;

        public override IEnumerable<IDocument> DefaultDocuments
        {
            get { yield return new ViewModels.CameraViewModel(); }
        }


        [ImportingConstructor]
        public Module(IInspectorTool inspectorTool)
        {
            this.inspectorTool = inspectorTool;
        }

        public override void PostInitialize()
        {
            var cameraViewModel = Shell.Documents.OfType<ViewModels.CameraViewModel>().FirstOrDefault();
            if (cameraViewModel != null)
                inspectorTool.SelectedObject = new InspectableObjectBuilder()
                    .WithCheckBoxEditor(cameraViewModel, x => x.Switch)
                    .ToInspectableObject();
            inspectorTool.DisplayName = "Camera Properties";
        }
    }


}