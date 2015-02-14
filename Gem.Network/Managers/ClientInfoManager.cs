using Gem.Network.Messages;
using Gem.Network.Other;
using Gem.Network.Repositories;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Managers
{
    public sealed class NetworkConfig
    {
        static NetworkConfig()
        {
            cachedInfo = null;
            configurations = new List<string>();

        }

        private Dictionary<IncomingMessageTypes,IManager> networkManagers;

        public static void Setup(string config)
        {
            
        
        }

        public static void MessageType(IncomingMessageTypes messageTypes)
        {

        }

        internal static ClientNetworkInfo cachedInfo { get; set; }
        internal static Byte randomByte { get; set; }
        private static List<string> configurations;

        public string this[int index]
        {
            get { return configurations[index]; }
        }
    }


    class ClientInfoManager
    {

        private Dictionary<byte, ClientNetworkInfo> clientInfo;
        
        private byte GetUniqueByte()
        {
            Guard.That(clientInfo).IsTrue(x => x.Count < (int)byte.MaxValue,
           "You have reached the maximum capacity. Consider deregistering");
            byte randomByte;

            do
            {
                randomByte = RandomGenerator.GetRandomByte();

            } while (!clientInfo.ContainsKey(randomByte));

            return randomByte;
        }
    }
}
