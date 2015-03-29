using System;

namespace Gem.Network.Utilities.Loggers
{
    /// <summary>
    /// Echoes information about GemNetwork flow
    /// </summary>
    public static class GemNetworkDebugger
    {

        static GemNetworkDebugger()
        {
            Echo = x => { };

            Append = new DebugHost();
            Append.RegisterAppender(new ActionAppender(Echo));
        }

        /// <summary>
        /// The debug host
        /// </summary>
        public static IDebugHost Append { get; set; }

        /// <summary>
        /// The global echo
        /// </summary>
        public static Action<string> Echo { get; set; }

    }
}                                                                                                                                                                                                                                                                                                                                                        