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
            Append = new DebugHost();
            Echo = x => { };
        }

        /// <summary>
        /// The debug host
        /// </summary>
        public static IDebugHost Append { get; set; }

        /// <summary>
        /// The global echo
        /// </summary>
        private static Action<string> echo;
        public static Action<string> Echo
        {
            get
            {
                return echo;
            }
            set
            {
                echo = value;
                Append.RemoveAll();
                Append.RegisterAppender(new ActionAppender(echo));
            }

        }

    }
}