using System.Text.RegularExpressions;
using System;
using System.Text;
using System.Collections.Generic;
using CSScriptLibrary;
using System.Linq;
using Gem.Network.Handlers;
using Gem.Network.Events;
using Lidgren.Network;

namespace Gem.Network.Builders
{
    /// <summary>
    /// Builds runtime POCOs using CsScriptLibary. 
    /// The POCOs type is aligned to INetworkPackage
    /// </summary>
    public class CsScriptPocoBuilder : IPocoBuilder
    {

        /// <summary>
        /// Pass all the parameters to the constructor, so when the class is instantiated using Activator,
        /// you can store properties using the constructor
        /// </summary>
        /// <param name="propertyFields">The types and names</param>
        /// <returns>A string with the constructor declaration</returns>
        private string GetConstructorDeclaration(List<DynamicPropertyInfo> propertyFields)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < propertyFields.Count; i++)
            {
                sb.Append(string.Format("{0} {1},", DynamicPropertyInfo.GetPrimitiveTypeAlias(propertyFields[i].PropertyType),propertyFields[i].PropertyName));
            }
            sb.Length--;
            return sb.ToString();
        }

        /// <summary>
        /// Store the parameters that are passed in the constructor
        /// </summary>
        /// <param name="propertyFields"></param>
        /// <param name="propertyFields">The types and names</param>
        /// <returns>A string with the constructor body</returns>
        private string GetConstructorBody(List<DynamicPropertyInfo> propertyFields)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < propertyFields.Count; i++)
            {
                sb.Append(string.Format("this.{0} = {0};", propertyFields[i].PropertyName));
            }
            return sb.ToString();
        }

        /// <summary>
        /// This constrtuctor takes a NetIncomingMessage and decodes its data using Read()
        /// </summary>
        /// <param name="propertyFields">The fields that are being decoded</param>
        /// <returns>A string with the constructor body</returns>
        private string GetDecodeConstructorBody(List<DynamicPropertyInfo> propertyFields)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < propertyFields.Count; i++)
            {
                sb.Append(string.Format("this.{0} = msg.{1}();", propertyFields[i].PropertyName, DynamicPropertyInfo.GetDecodePrefix(propertyFields[i].PropertyType)));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates prop {get; set;}
        /// </summary>
        /// <param name="propertyFields">The fields that are being decoded</param>
        /// <returns>A string with properties</returns>
        private string GetGetterSetters(List<DynamicPropertyInfo> propertyFields)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < propertyFields.Count; i++)
            {
                sb.Append(string.Format("public {0} {1} {{get;set;}}", DynamicPropertyInfo.GetPrimitiveTypeAlias(propertyFields[i].PropertyType), propertyFields[i].PropertyName));
            }
            return sb.ToString();
        }

 
        /// <summary>
        /// Builds the dynamic POCO and aligns it to INetworkPackage
        /// </summary>
        /// <param name="className">The POCO class name</param>
        /// <param name="propertyFields">The field's types and names</param>
        /// <returns>A type that's aligned to INetworkPackage</returns>
        public Type Build(string className, List<DynamicPropertyInfo> propertyFields)
        {
            var str = String.Format(@"using Microsoft.CSharp; using Lidgren.Network;
                                            namespace Gem.Network.Builders
                                            {{
                                            public class {0} : Gem.Network.Events.INetworkPackage
                                             {{ 
                                              public {0}() {{}}  
                                              public {0}(NetIncomingMessage msg) {{{4}}}                                                                                               
                                                                                             
                                                 public {0}({1})
                                                 {{
                                                  {2}
                                                 }}
                                                 
                                                {3}
                                                private byte _Id;
                                                public byte Id 
                                                {{
                                                get
                                                {{
                                                     return _Id;
                                                }}
                                                set
                                                {{
                                                    _Id = value;
                                                }}
                                                }}
                                                }}                                                                                        
                                             }}", className,
                                                GetConstructorDeclaration(propertyFields),
                                                GetConstructorBody(propertyFields),
                                                GetGetterSetters(propertyFields),
                                                GetDecodeConstructorBody(propertyFields));

            return CSScript.LoadCode(str)
                           .CreateObject("*")
                           .AlignToInterface<INetworkPackage>()
                           .GetType();
        }
    }
}
