using System.Text.RegularExpressions;
using System;
using System.Text;
using System.Collections.Generic;
using CSScriptLibrary;
using System.Linq;
using Gem.Network.Handlers;
using Gem.Network.Events;

namespace Gem.Network.Builders
{
    /// <summary>
    /// Creates a type that handles the server incoming messages
    /// </summary>
    public class CsScriptPocoBuilder : IPocoBuilder
    {

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

        private string GetConstructorBody(List<DynamicPropertyInfo> propertyFields)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < propertyFields.Count; i++)
            {
                sb.Append(string.Format("this.{0} = {0};", propertyFields[i].PropertyName));
            }
            return sb.ToString();
        }

        private string GetGetterSetters(List<DynamicPropertyInfo> propertyFields)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < propertyFields.Count; i++)
            {
                sb.Append(string.Format("public {0} {1} {{get;set;}}", DynamicPropertyInfo.GetPrimitiveTypeAlias(propertyFields[i].PropertyType), propertyFields[i].PropertyName));
            }
            return sb.ToString();
        }



        public Type Build(string className, List<DynamicPropertyInfo> propertyFields)
        {
            var str = String.Format(@"using Microsoft.CSharp;
                                            namespace Gem.Network.Builders
                                            {{
                                            public class {0} : Gem.Network.Events.INetworkPackage
                                             {{ 
                                              public {0}() {{}}                                                                                               
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
                                                GetGetterSetters(propertyFields));

            return CSScript.LoadCode(str)
                           .CreateObject("*")
                           .AlignToInterface<INetworkPackage>()
                           .GetType();
        }
    }
}
