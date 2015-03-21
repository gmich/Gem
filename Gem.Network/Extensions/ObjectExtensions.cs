using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Get object's property value by it's name using reflection
        /// </summary>
        /// <param name="propName">The property's name</param>
        /// <param name="obj">The object</param>
        /// <returns>The property's value</returns>
        public static object GetValue(this object obj, string propName)
        {
            return obj.GetType().GetProperty(propName).GetValue(obj, null);
        }

        /// <summary>
        /// Get object's property value by it's name using reflection
        /// </summary>
        /// <param name="propName">The property's name</param>
        /// <param name="obj">The object</param>
        /// <returns>The property's value</returns>
        public static void SetValue(this object obj, string propName, object propValue)
        {
            obj.GetType().GetProperty(propName).SetValue(obj, propValue);
        }

        public static void DynamicInvoke(this object obj, string name, object[] args)
        {
            obj.GetType().GetMethod(name).Invoke(obj, args);
        }

        public static object[] ReadAllProperties(this object obj)
        {
            return obj.GetType().GetProperties().Select(x => x.GetValue(obj)).ToArray();
        }
        
        public static IEnumerable<Type> GetPropertyTypes(this object obj)
        {
            return obj.GetType().GetProperties().Select(x => x.PropertyType);
        }

        public static IEnumerable<Type> GetDeclaredPropertyTypes(this object obj)
        {
            return obj.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(x=>x.PropertyType.IsPrimitive)
                                .Select(x => x.PropertyType);
        }
    }
}
