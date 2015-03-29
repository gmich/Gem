using Lidgren.Network;
using System;
using System.ComponentModel.DataAnnotations;

namespace Gem.Network
{
    /// <summary>
    /// How a message is sent
    /// </summary>
    public class PackageConfig
    {

        #region Required Fields

        [Required]
        public int SequenceChannel { get; set; }

        [Required]
        public NetDeliveryMethod DeliveryMethod { get; set; }

        #endregion

        #region Predefined PackagesConfigs

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

        public static PackageConfig UDPSequenced
        {
            get
            {
                return new PackageConfig
                {
                    SequenceChannel = 0,
                    DeliveryMethod = NetDeliveryMethod.UnreliableSequenced
                };
            }
        }

        #endregion

    }

}
