using Gem.IDE.Modules.ProjectExplorer.ViewModels;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.IDE.Modules.ProjectExplorer.Commands
{
    [CommandHandler]
    public class ViewProjectExplorerCommandHandler : CommandHandlerBase<ViewProjectExplorerCommandDefinition>
    {
        private readonly IShell _shell;

        [ImportingConstructor]
        public ViewProjectExplorerCommandHandler(IShell shell)
        {
            _shell = shell;
        }

        public override Task Run(Command command)
        {
            _shell.ShowTool<ProjectExplorerViewModel>();
            return TaskUtility.Completed;
        }
    }
}