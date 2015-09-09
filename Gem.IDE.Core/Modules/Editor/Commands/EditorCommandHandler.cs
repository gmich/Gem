using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Gem.IDE.Core.Modules.Editor.Commands
{
    [CommandHandler]
    public class EditorCommandHandler : CommandHandlerBase<ViewEditorCommandDefinition>
    {
        private readonly IShell _shell;

        [ImportingConstructor]
        public EditorCommandHandler(IShell shell)
        {
            _shell = shell;
        }

        public override Task Run(Command command)
        {
            _shell.OpenDocument((IDocument)IoC.GetInstance(typeof(EditorViewModel), null));
            return TaskUtility.Completed;
        }
    }

}
