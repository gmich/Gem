using System.ComponentModel;
using Gem.Infrastructure.Attributes;

namespace Gem.Gui.Configuration
{
    public class Options
    {
        public Options()
        {
            this.AssignDefaultValues();
        }

        [DefaultValue(true)]
        public bool IsVisible { get; set; }

        [DefaultValue(true)]
        public bool IsEnabled { get; set; }

        [DefaultValue(true)]
        public bool IsHoverEnabled { get; set; }

        [DefaultValue(true)]
        public bool IsFocusEnabled { get; set; }
    }
}
