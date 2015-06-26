using System.ComponentModel;
using Gem.Infrastructure.Attributes;

namespace Gem.Engine.Console.Commands
{
    public class TerminalSettings
    {
        public TerminalSettings()
        {
            this.AssignDefaultValues();
        }
        
        [DefaultValue('|')]
        public char CommandSeparator { get; set; }

        [DefaultValue('>')]
        public char SubCommandSeparator { get; set; }

        [DefaultValue(' ')]
        public char ArgumentSeparator { get; set; }

        public static TerminalSettings Default { get { return new TerminalSettings(); } }
    }
}
