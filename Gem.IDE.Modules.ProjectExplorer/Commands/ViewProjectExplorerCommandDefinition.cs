using Gemini.Framework.Commands;

namespace Gem.IDE.Modules.ProjectExplorer.Commands
{
    [CommandDefinition]
    public class ViewProjectExplorerCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.ProjectExplorer";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "Project Explorer"; }
        }

        public override string ToolTip
        {
            get { return "Project Explorer"; }
        }
    }
}
