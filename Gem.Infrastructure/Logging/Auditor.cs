using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Infrastructure.Logging
{
    public static class Auditor
    {
        private static DebugHost debugHost;
        static Auditor()
        {
            debugHost = new DebugHost();
        }

        public static DebugHost Logger
        {
            get { return debugHost; }
        }


    }
}
