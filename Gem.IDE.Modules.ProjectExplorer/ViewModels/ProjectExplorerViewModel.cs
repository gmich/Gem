using Gemini.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gemini.Framework.Services;

namespace Gem.IDE.Modules.ProjectExplorer.ViewModels
{
    [Export(typeof(ProjectExplorerViewModel))]
    public class ProjectExplorerViewModel : Tool
    {
        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Right; }
        }
    }


}
