using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Other
{
    class AttributeResolver
    {
        public void Resolver(Type type)
        {
            ClientAttribute HelpAttr;


            //Querying Class Attributes
            foreach (Attribute attr in type.GetCustomAttributes(true))
            {
                HelpAttr = attr as ClientAttribute;
                if (null != HelpAttr)
                {
                    Console.WriteLine("Description of AnyClass:\n{0}",
                                      HelpAttr.Configuration);
                }
            }
            //Querying Class-Method Attributes  
            foreach (MethodInfo method in type.GetMethods())
            {
                foreach (Attribute attr in method.GetCustomAttributes(true))
                {
                    HelpAttr = attr as ClientAttribute;
                    if (null != HelpAttr)
                    {
                        Console.WriteLine("Description of {0}:\n{1}",
                                          method.Name,
                                          HelpAttr.Configuration);
                    }
                }
            }
            //Querying Class-Field (only public) Attributes
            foreach (FieldInfo field in type.GetFields())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    HelpAttr = attr as ClientAttribute;
                    if (null != HelpAttr)
                    {
                        Console.WriteLine("Description of {0}:\n{1}",
                                          field.Name, HelpAttr.Configuration);
                    }
                }
            }
        }
    }
}

