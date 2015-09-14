using Gemini.Framework.Commands;

namespace Gem.IDE.Modules.SpriteSheets.Commands
{
    [CommandDefinition]
    public class AnimationStripViewerCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Demos.SpriteSheetAnimations";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "Sprite-Sheet Animations"; }
        }

        public override string ToolTip
        {
            get { return "Sprite-Sheet Animations"; }
        }
    }
}