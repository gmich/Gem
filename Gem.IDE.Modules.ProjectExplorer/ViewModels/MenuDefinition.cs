using Gem.IDE.Modules.ProjectExplorer.Commands;
using Gemini.Framework.Menus;
using System.ComponentModel.Composition;

namespace Gem.IDE.Modules.ProjectExplorer.ViewModels
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemDefinition ViewProjectExplorerMenuItem = new CommandMenuItemDefinition<ViewProjectExplorerCommandDefinition>(
        Gemini.Modules.MainMenu.MenuDefinitions.ViewPropertiesMenuGroup, 1);
    }
}
