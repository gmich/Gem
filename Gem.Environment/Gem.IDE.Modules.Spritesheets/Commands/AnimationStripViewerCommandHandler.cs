using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using Gem.IDE.Modules.SpriteSheets.ViewModels;

namespace Gem.IDE.Modules.SpriteSheets.Commands
{
    [CommandHandler]
    public class AnimationStripViewerCommandHandler : CommandHandlerBase<AnimationStripViewerCommandDefinition>
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public AnimationStripViewerCommandHandler(IShell shell)
        {
            this.shell = shell;
        }

        public override Task Run(Command command)
        {
            shell.OpenDocument(new AnimationStripViewModel());
            return TaskUtility.Completed;
        }
    }
}