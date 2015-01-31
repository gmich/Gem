using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Example
{
    class Chatter
    {

        public string Name { get; set; }

        public void Say(string message)
        {
            Console.WriteLine(String.Format("{0} : {1}", Name, message));
        }

        public void Say(string sender, string message)
        {
            Console.WriteLine(String.Format("{0} : {1}", sender, message));
        }
        
    }
}
