using Gemini.Framework;
using System.ComponentModel.Composition;
using Gemini.Framework.Services;
using Gem.IDE.Modules.ProjectExplorer.Views;

namespace Gem.IDE.Modules.ProjectExplorer.ViewModels
{
    [Export(typeof(ProjectExplorerViewModel))]
    public class ProjectExplorerViewModel : Tool
    {
        private IProjectExplorer explorer;

        public ProjectExplorerViewModel() : base()
        {
            this.ViewAttached += (sender, args) =>
            {
                explorer = args.View as IProjectExplorer;
            };
            DisplayName = "Project Explorer";
        }


        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Right; }
        }              

    }
    
}
