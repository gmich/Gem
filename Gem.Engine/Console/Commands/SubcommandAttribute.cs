using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.Console.Commands
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SubcommandAttribute : Attribute
    {
        private readonly string parentCommand;
        private readonly string subCommand;
        private readonly string description;
        private readonly bool requiresAuthorization;

        public SubcommandAttribute(string parentCommand, string subCommand, string description, bool requiresAuthorization = false)
        {
            if (parentCommand == null)
            {
                throw new ArgumentException("parentCommand");
            }
            this.parentCommand = parentCommand;

            if (subCommand == null)
            {
                throw new ArgumentException("argument");
            }
            this.subCommand = subCommand;

            if (description == null)
            {
                throw new ArgumentException("description");
            }
            this.description = description;

            this.requiresAuthorization = requiresAuthorization;
        }

        public string SubCommand { get { return subCommand; } }
        public string ParentCommand { get { return parentCommand; } }
        public string Description { get { return description; } }
        public bool RequiresAuthorization { get { return requiresAuthorization; } }
    }
}
