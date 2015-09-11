using Gemini.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.IDE.Core.Modules.Editor.Commands
{
    [CommandDefinition]
    public class ViewEditorCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View Editor";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "Editor"; }
        }

        public override string ToolTip
        {
            get { return "Editor"; }
        }
    }

}
