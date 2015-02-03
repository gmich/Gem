using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gem.Network.Messages;
using Seterlund.CodeGuard;
using CSScriptLibrary;
using System.Text.RegularExpressions;

namespace Gem.Network.Configuration
{
    public static class NetworkConfig
    {
        public static DynamicMessage ForTag(string tag)
        {
            return new DynamicMessage(tag);
        }

        //TODO: remove
        public static List<Type> Send(params object[] args)
        {
            var types = new List<Type>();

            foreach (var arg in args)
            {
                types.Add(arg.GetType());
            }
            return types;
        }
    }

    public sealed class DynamicMessage
    {
        private readonly string Tag;

        private List<PropertyInfo> propertyInfo;
        private int PropertyCount { get; set; }

        public Type PocoType{ get; set;}
        public IncomingMessageTypes MessageType { get; private set; }

        public string this[int param]
        {
            get { return propertyInfo[param].PropertyName; }
        }

        public DynamicMessage(string tag)
        {
            propertyInfo = new List<PropertyInfo>();
            this.Tag=tag;
            this.PropertyCount = 0;
        }

        public IncomingMessageHandler CreateEvent(params Type[] args)
        {
            //Guard.That(typeof(T) == typeof(IncomingMessageTypes),"Invalid message type");

            Guard.That(args.Any(x => x.IsPrimitive || x == typeof(string)),"All types should be primitive");

            foreach (var arg in args)
            {
                propertyInfo.Add(new PropertyInfo
                {
                    PropertyName = (Tag + (++PropertyCount)),
                    PropertyType = arg
                });
            }
            return new IncomingMessageHandler(this.Tag,this.propertyInfo);
        }
    }

    public sealed class IncomingMessageHandler
    {
        string Tag { get; set; }
        int argsCount { get; set; }
        string DelegateName { get; set;}
        object ObjectsDelegate { get; set; }
        private List<PropertyInfo> propertyInfo;

        public IncomingMessageHandler(string Tag, List<PropertyInfo> propertyInfo)
        {
            this.Tag = Tag;
            this.propertyInfo = propertyInfo;
            this.argsCount = propertyInfo.Count;
        }

        public MessageHandler HandleWith(object obj, string DelegateName)
        {
            ObjectsDelegate = obj;
            this.DelegateName = DelegateName;

            return new MessageHandler(CreateMessageHandler());
        }

        private string GetArgumentsCallForDynamicInvoker()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < argsCount; i++)
            {
                sb.Append(string.Format("({0})args[{1}],", Regex.Replace(propertyInfo[i].PropertyType.Name.ToLower(), "[0-9]", ""), i));
            }
            sb.Length--;
            return sb.ToString();
        }

        private dynamic CreateMessageHandler()
        {
            var str = String.Format(@"public class {0} 
                                             {{ 
                                                 private readonly dynamic element;
                                              public {0}()
                                                 {{
                                                    
                                                 }}
                                                 public {0}(dynamic element)
                                                 {{
                                                     this.element = element;
                                                 }}
                                                 public void Handle(params object[] args)
                                                 {{
                                                     element.{1}({2});
                                                 }}                                                                                             
                                             }}", Tag, DelegateName, GetArgumentsCallForDynamicInvoker());

            var messageHandler = CSScript.LoadCode(str)
                                               .CreateObject("*")
                                               .GetType();
            // .AlignToInterface<Sender>();

            return Activator.CreateInstance(messageHandler, ObjectsDelegate);

        }


        //internal interface MessageHandler
        //{
        //    void Handle(params object[] args);
        //}
    }

    public class MessageHandler
    {
        private readonly dynamic objectThatHandlesMessages;

        public MessageHandler(dynamic obj)
        {
            this.objectThatHandlesMessages = obj;
        }

        public void HandleMessage(params object[] args)
        {
            objectThatHandlesMessages.Handle(args);
        }
        
    }

}
