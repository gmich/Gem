using Gem.Network.Client;
using Gem.Network.Commands;
using Gem.Network.Configuration;
using Gem.Network.Server;
using Gem.Network.Utilities.Loggers;
using System.Collections.Generic;

namespace Gem.Network
{
    /// <summary>
    /// Startup class that initializes dependencies, server , client and holds the active profile reference
    /// </summary>
    public static class GemNetwork
    {

        #region Predefined Package Types

        internal static readonly byte ConnectionApprovalByte = (byte)1;
        internal static readonly byte NotificationByte = (byte)2;
        internal static readonly byte DisconnectByte = (byte)3;
        internal static readonly byte InitialId = (byte)4;

        #endregion

        #region Ctor

        static GemNetwork()
        {
            GemNetworkDebugger.Append.RegisterAppender(new Log4NetWrapper("DebugLogger"));

            //Setup the default configuration for now
            Dependencies.Setup(new DefaultConfiguration().Load("gem.config"));

            Client = new Peer();
            Server = new NetworkServer(GemNetworkDebugger.Echo);
            commander = new ServerCommandHost(Server);
            MessageCounter = new Dictionary<string, byte>();
        }

        #endregion

        #region Fields

        /// <summary>
        /// The terminal
        /// </summary>
        internal static ICommandHost commander;

        /// <summary>
        /// The base client
        /// </summary>
        internal static IClient Client;

        /// <summary>
        /// The base server
        /// </summary>
        internal static IServer Server;

        /// <summary>
        /// Holds how many configurations are initialized by profile
        /// </summary>
        private static Dictionary<string, byte> MessageCounter;

        /// <summary>
        /// Gets a unique id by profile
        /// </summary>
        /// <param name="profile">The profile</param>
        /// <returns>The id</returns>
        internal static byte GetMesssageId(string profile) 
        {
               if(MessageCounter.ContainsKey(profile))
               {
                   return MessageCounter[profile]++;
               }
               else
               {
                   MessageCounter.Add(profile, InitialId);
                   return InitialId;
               }
        }

        #endregion
        
        #region Properties

        public static string ActiveProfile
        {
            get;
            internal set;
        }

        internal static ICommandHost Commander
        {
            get
            {
                return commander;
            }
        }
               
        #endregion

    }
}
