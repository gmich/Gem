using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Configuration
{
    public interface IConfigurationReader
    {
        void Load(string path);

        DependencyArgs Dependencies { get; }
    }
}
