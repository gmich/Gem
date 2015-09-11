using Caliburn.Micro;
using Gem.IDE.Core.Modules.Editor.Commands;
using Gemini.Framework;
using Gemini.Framework.Menus;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Gem.IDE.Core.Modules.Editor
{

    [Export(typeof(IModule))]
    public class Module : ModuleBase
    {
        [Export]
        public static MenuItemGroupDefinition ViewDemoMenuGroup = new MenuItemGroupDefinition(
               Gemini.Modules.MainMenu.MenuDefinitions.ViewMenu, 10);
        [Export]
        public static MenuItemDefinition ViewEditorMenuItem = new CommandMenuItemDefinition<ViewEditorCommandDefinition>(
            ViewDemoMenuGroup, 1);

        public override IEnumerable<IDocument> DefaultDocuments
        {
            get
            {
                yield return IoC.Get<EditorViewModel>();
            }
        }
    }

}
