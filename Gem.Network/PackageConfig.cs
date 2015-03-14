using Lidgren.Network;
using System;

namespace Gem.Network
{
    public class PackageConfig
    {
        public int SequenceChannel { get; set; }

        public NetDeliveryMethod DeliveryMethod { get; set; }

        public static PackageConfig TCP
        {
            get
            {
                return new PackageConfig 
                { 
                    SequenceChannel = 0, 
                    DeliveryMethod = NetDeliveryMethod.ReliableSequenced
                };
            }
        }

        public static PackageConfig UDP
        {
            get
            {
                return new PackageConfig
                {
                    SequenceChannel = 0,
                    DeliveryMethod = NetDeliveryMethod.Unreliable
                };
            }
        }
    }

}
