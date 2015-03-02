using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Utilities.Loggers
{
    public static class GemNetworkDebugger
    {

        static GemNetworkDebugger()
        {
            Echo = x => { };
        }

        public static IDebugHost Append { get; set; }

        public static Action<string> Echo { get; set; }

    }
}                                                                                                                                                                                                                                                                                                                                                        