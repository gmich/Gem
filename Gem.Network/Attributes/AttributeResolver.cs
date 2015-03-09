using System;
namespace Gem.Network.Other
{
    /// <summary>
    /// Helper class for attribute resolving 
    /// </summary>
    public class AttributeResolver
    {
        /// <summary>
        /// Checks if the class is marked with the specified attribute
        /// </summary>
        /// <param name="type">The type to check</param>
        public static void Resolve<T>(Type type)
            where T: Attribute
        {
            T HelpAttr;
            
            //Querying Class Attributes
            foreach (Attribute attr in type.GetCustomAttributes(true))
            {
                HelpAttr = attr as T;
                if (null != HelpAttr)
                {
        
                }
            }                  
        }
    }
}

