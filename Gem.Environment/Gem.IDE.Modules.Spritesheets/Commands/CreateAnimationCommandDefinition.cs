using Gemini.Framework.Commands;
using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace Gem.IDE.Modules.SpriteSheets.Commands
{
    [CommandDefinition]
    public class CreateAnimationCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Demos.NewAnimation";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "New Animation"; }
        }

        public override string ToolTip
        {
            get { return "New Animation"; }
        }

        public override Uri IconSource
        {
            get { return new Uri("pack://application:,,,/Gemini;component/Resources/Icons/Open.png"); }
        }

        //[Export]
        //public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<CreateAnimationCommandDefinition>(
        //    new KeyGesture(Key.A, ModifierKeys.Shift));
    }
}

