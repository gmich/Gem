using Gemini.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gemini.Framework.Services;
using System.Collections.ObjectModel;
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
