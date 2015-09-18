using Gem.IDE.Modules.SpriteSheets.ViewModels;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using static Gem.DrawingSystem.Animations.Extensions;
using System.Threading.Tasks;
using Gem.DrawingSystem.Animations.Repository;

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
            dialog.InitialDirectory = $"{Environment.CurrentDirectory}\\Content";
            dialog.Filter = $"Animation Files(*.bmp, *.jpg, *.png *{Animation}) | *.bmp; *.jpg; *.png; *{Animation};";
            if (dialog.ShowDialog() == true)
            {
                if (dialog.FileName.EndsWith(Animation))
                {
                    shell.OpenDocument(new AnimationStripViewModel(dialog.FileName,
                        new JsonAnimationRepository($"{Environment.CurrentDirectory}\\Content")));
                }
                else
                {
                    shell.OpenDocument(new AnimationStripViewModel(dialog.FileName));
                }
            }
            return TaskUtility.Completed;
        }
    }
}
