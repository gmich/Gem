using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Gem.IDE.Core.Modules.Camera.ViewModels;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;

namespace Gem.IDE.Core.Modules.Camera.Commands
{
    [CommandHandler]
    public class CameraCommandHandler : CommandHandlerBase<CameraCommandDefinition>
    {
        private readonly IShell shell;

        [ImportingConstructor]
        public CameraCommandHandler(IShell shell)
        {
            this.shell = shell;
        }

        public override Task Run(Command command)
        {
            shell.OpenDocument(new CameraViewModel());
            return TaskUtility.Completed;
        }
    }
}