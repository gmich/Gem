using Gemini.Framework.Commands;

namespace Gem.IDE.Core.Modules.Camera.Commands
{
    [CommandDefinition]
    public class CameraCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Demos.Camera";

        public override string Name
        {
            get { return CommandName; }
        }
        
        public override string Text
        {
            get { return "Camera"; }
        }

        public override string ToolTip
        {
            get { return "Camera"; }
        }
    }
}