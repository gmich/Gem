using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Configuration
{
    public class NetworkConfig
    {
        NetworkConfig ForTag(string tag)
        {
            return this;
        }

        public static List<Type> Send(params object[] args)
        {
            var types = new List<Type>();

            foreach (var arg in args)
            {
                types.Add(arg.GetType());
            }
            return types;
        }

        void Send<T1>(T1 t)
        {

        }
        void Send<T1, T2>()
        {

        }
        void Send<T1, T2, T3>()
        {

        }

        void Send<T1, T2, T3, T4>()
        {

        }
        void Send<T1, T2, T3, T4, T5>()
        {

        }

        void Send<T1, T2, T3, T4, T5, T6>()
        {

        }
    }
}
