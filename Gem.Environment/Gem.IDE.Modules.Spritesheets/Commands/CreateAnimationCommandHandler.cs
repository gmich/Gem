using Gem.IDE.Modules.SpriteSheets.ViewModels;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.IDE.Modules.SpriteSheets.Commands
{
    [CommandHandler]
    public class CreateAnimationCommandHandler : CommandHandlerBase<CreateAnimationCommandDefinition>
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public CreateAnimationCommandHandler(IShell shell)
        {
            this.shell = shell;
        }

        public override Task Run(Command command)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Image Files(*.bmp, *.jpg, *.png) | *.bmp; *.jpg; *.png;";

            if (dialog.ShowDialog() == true)
            {
                shell.OpenDocument(new AnimationStripViewModel(dialog.FileName));
            }
            return TaskUtility.Completed;

        }
    }
}
