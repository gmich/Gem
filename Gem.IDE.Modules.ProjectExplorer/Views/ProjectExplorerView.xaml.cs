using System.Windows.Controls;

namespace Gem.IDE.Modules.ProjectExplorer.Views
{
    /// <summary>
    /// Interaction logic for ProjectExplorerView.xaml
    /// </summary>
    public partial class ProjectExplorerView : UserControl, IProjectExplorer
    {
        public ProjectExplorerView()
        {
            InitializeComponent();
        }

        public void AddItem(string name)
        {         
        }
    }
}
