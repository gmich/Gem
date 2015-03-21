using Gem.Network.Client;
using Gem.Network.Commands;
using Gem.Network.Fluent;
using Gem.Network.Server;
using Gem.Network.Utilities.Loggers;
using System;
using System.Collections.Generic;

namespace Gem.Network
{
    public static class GemNetwork
    {

        //Predefined package types
        internal static readonly byte InitialId = (byte)3;
        internal static readonly byte DisconnectByte = (byte)2;
        internal static readonly byte NotificationByte = (byte)1;
        internal static readonly byte ConnectionApprovalByte = (byte)0;

        static GemNetwork()
        {
            Startup.Setup();
            Client = new Peer();
            Server = new NetworkServer(GemNetworkDebugger.Echo);
            commander = new ServerCommandHost(Server);
            MessageCounter = new Dictionary<string, byte>();

        }
  
        #region Fields
                
        internal static ICommandHost commander;

        internal static IClient Client;

        internal static IServer Server;

        private static Dictionary<string, byte> MessageCounter;

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
