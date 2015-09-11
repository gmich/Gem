using System.ComponentModel.Composition;
using Gem.IDE.Core.Modules.SceneViewer.Commands;
using Gemini.Framework.Menus;

namespace Gem.IDE.Core.Modules.SceneViewer
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemDefinition ViewSceneViewerMenuItem = new CommandMenuItemDefinition<ViewSceneViewerCommandDefinition>(
            Startup.Module.DemosMenuGroup, 1);
    }
}