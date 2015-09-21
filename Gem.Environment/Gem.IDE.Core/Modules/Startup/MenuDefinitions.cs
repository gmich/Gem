using System.ComponentModel.Composition;
using Gem.IDE.Core.Modules.SceneViewer.Commands;
using Gemini.Framework.Menus;
using Gem.IDE.Core.Modules.Camera.Commands;
using Gem.IDE.Modules.SpriteSheets.Commands;

namespace Gem.IDE.Core.Modules
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemDefinition ViewSceneViewerMenuItem = new CommandMenuItemDefinition<ViewSceneViewerCommandDefinition>(
            Startup.Module.DemosMenuGroup, 1);

        [Export]
        public static MenuItemDefinition ViewCameraMenuItem = new CommandMenuItemDefinition<CameraCommandDefinition>(
            Startup.Module.DemosMenuGroup, 2);

        [Export]
        public static MenuItemDefinition CreateAnimationMenuItem = new CommandMenuItemDefinition<CreateAnimationCommandDefinition>(
             Startup.Module.DemosMenuGroup, 3);
    }
}