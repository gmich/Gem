using System.ComponentModel.Composition;
using Gem.IDE.Core.Modules.Camera.Commands;
using Gemini.Framework.Menus;

namespace Gem.IDE.Core.Modules.Camera
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemDefinition ViewSceneViewerMenuItem = new CommandMenuItemDefinition<CameraCommandDefinition>(
            Startup.Module.DemosMenuGroup, 1);
    }
}