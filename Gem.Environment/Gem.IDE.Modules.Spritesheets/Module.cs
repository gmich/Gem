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


        public override void PostInitialize()
        {
            var animationStripViewModel = Shell
                                        .Documents
                                        .OfType<AnimationStripViewModel>()
                                        .FirstOrDefault();

            if (animationStripViewModel != null)
            {
                inspectorTool.SelectedObject = new InspectableObjectBuilder()
                    .WithCollapsibleGroup("Animation", group => 
                            group.WithObjectProperties(animationStripViewModel, model => 
                            model.Attributes.Matches(new AnimationAttribute())))
                    .WithCollapsibleGroup("Sprite sheet", group => 
                            group.WithObjectProperties(animationStripViewModel, model =>
                            model.Attributes.Matches(new SpriteSheetAttribute())))
                    .WithCollapsibleGroup("Presentation", group =>
                            group.WithObjectProperties(animationStripViewModel, model =>
                            model.Attributes.Matches(new PresentationAttribute())))
                   .ToInspectableObject();
                inspectorTool.DisplayName = "Sprite-Sheet Animation Inspector";
            }
        }
    }
}