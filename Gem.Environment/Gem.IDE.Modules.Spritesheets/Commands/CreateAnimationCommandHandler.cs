using Gem.IDE.Modules.SpriteSheets.ViewModels;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using Microsoft.Win32;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Gem.DrawingSystem.Animations.Repository;
using static Gem.DrawingSystem.Animations.Extensions;
using Gem.DrawingSystem.Animations;
using Xceed.Wpf.Toolkit;
using System.Windows;

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
                var repository = new JsonAnimationRepository($"{Environment.CurrentDirectory}\\Content");
                if (dialog.FileName.EndsWith(Animation))
                {
                    repository.LoadByPath(dialog.FileName)
                        .Done(settings =>
                                shell.OpenDocument(new AnimationStripViewModel(dialog.FileName, settings,path=> new JsonAnimationRepository(path))),
                               ex =>
                               {
                                    var res = Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
                               });
                }
                else
                {
                    var defaultSettings = new AnimationStripSettings(32, 32, 0, 0, GetRandomName(repository.Exists, 1), 0.02d, false, null);
                    shell.OpenDocument(new AnimationStripViewModel(dialog.FileName, defaultSettings, path => new JsonAnimationRepository(path)));
                }
            }
            return TaskUtility.Completed;
        }

        private string GetRandomName(Predicate<string> exists, int suffix)
        {
            string newName = "animation_" + suffix.ToString();
            return exists(newName) ?
                GetRandomName(exists, suffix + 1) : newName;
        }

    }
}
