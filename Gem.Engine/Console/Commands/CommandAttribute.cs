using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Console
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        private readonly string command;
        private readonly string description;
        private readonly bool requiresAuthorization;

        public CommandAttribute(string command, string description, bool requiresAuthorization = false)
        {
            if (command == null)
            {
                throw new ArgumentException("command");
            }
            this.command = command;

            if (description == null)
            {
                throw new ArgumentException("description");
            }
            this.description = description;

            this.requiresAuthorization = requiresAuthorization;
        }

        public string Command { get { return command; } }
        public string Description { get { return description; } }
        public bool RequiresAuthorization { get { return requiresAuthorization; } }
    }
}
