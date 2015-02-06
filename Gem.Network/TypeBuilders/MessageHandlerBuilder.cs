using Gem.Network.DynamicBuilders;
using System.Text.RegularExpressions;
using System;
using System.Text;
using System.Collections.Generic;
using CSScriptLibrary;
using System.Linq;

namespace Gem.Network.Handlers
{
    /// <summary>
    /// Creates a type that handles the server incoming message's
    /// </summary>
    public class IncomingMessageHandlerBuilder
    {
        /// <summary>
        /// Creates a string that casts objects to the correct type e.g. (string)arg[0],(int)arg[1] etc
        /// </summary>
        /// <param name="properties">The properties that are being handled</param>
        /// <returns>A string to concat to the messagehandler class</returns>
        private static string GetArgumentsCallForDynamicInvoker(List<string> propertyNames)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < propertyNames.Count; i++)
            {
                sb.Append(string.Format("({0})args[{1}],", Regex.Replace(propertyNames[i].ToLower(), "[0-9]", ""), i));
            }
            sb.Length--;
            return sb.ToString();
        }

        /// <summary>
        /// Build a type that handles incoming messages <see cref="IMessageHandler"/>
        /// </summary>
        /// <param name="properties">The properties that are being handled</param>
        /// <param name="typeName">The type's name</param>
        /// <param name="functionName">The function to invoke. 
        /// The function must be a public member of the class that is being passed in the constructor</param>
        /// <returns>The type of the message handler</returns>
        public static Type BuildMessageHandler(List<DynamicPropertyInfo> properties, string typeName, string functionName)
        {
            var str = String.Format(@"public class {0} : IHandler
                                             {{ private readonly dynamic element;
                                              public {0}() {{}}                                                                                               
                                                 public {0}(dynamic element)
                                                 {{
                                                     this.element = element;
                                                 }}
                                                 public void Handle(params object[] args)
                                                 {{
                                                     element.{1}({2});
                                                 }}                                                                                             
                                             }}", typeName,
                                                functionName,
                                                GetArgumentsCallForDynamicInvoker(properties.Select(x => x.PropertyType.Name).ToList()));

            return CSScript.LoadCode(str)
                           .CreateObject("*")
                           .AlignToInterface<IMessageHandler>()
                           .GetType();
        }
    }
}
