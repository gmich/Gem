using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.Console.Commands
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RollbackCommandAttribute : Attribute
    {
        private readonly string command;

        public RollbackCommandAttribute(string command)
        {
            if (command == null)
            {
                throw new ArgumentException("command");
            }
            this.command = command;

        }

        public string Command { get { return command; } }
    }
}
