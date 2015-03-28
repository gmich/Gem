using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gem.Network.Extensions
{
    /// <summary>
    /// Helper class that extends object
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Get object's property value by it's name
        /// </summary>
        /// <param name="propName">The property's name</param>
        /// <param name="obj">The object</param>
        /// <returns>The property's value</returns>
        public static object GetValue(this object obj, string propName)
        {
            return obj.GetType().GetProperty(propName).GetValue(obj, null);
        }

        /// <summary>
        /// Set object's property value by it's name
        /// </summary>
        /// <param name="propName">The property's name</param>
        /// <param name="obj">The object</param>
        /// <returns>The property's value</returns>
        public static void SetValue(this object obj, string propName, object propValue)
        {
            obj.GetType().GetProperty(propName).SetValue(obj, propValue);
        }

        /// <summary>
        /// Invokes an object's method by name
        /// </summary>
        /// <param name="obj">The object</param>
        /// <param name="name">The method's name</param>
        /// <param name="args">The arguments of the method that's invoked</param>
        public static void DynamicInvoke(this object obj, string name, object[] args)
        {
            obj.GetType().GetMethod(name).Invoke(obj, args);
        }

        /// <summary>
        /// Reads all properties of an object
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The object's properties as array</returns>
        public static object[] ReadAllProperties(this object obj)
        {
            return obj.GetType().GetProperties().Select(x => x.GetValue(obj)).ToArray();
        }

        /// <summary>
        /// Returns all object's property types
        /// </summary>
        /// <param name="obj">The object to read the properties from</param>
        /// <returns>The object's properties as IEnumerable</returns>
        public static IEnumerable<Type> GetPropertyTypes(this object obj)
        {
            return obj.GetType().GetProperties().Select(x => x.PropertyType);
        }

        /// <summary>
        /// Returns all object's declared property types
        /// </summary>
        /// <param name="obj">The object to read the properties from</param>
        /// <returns>The object's properties as IEnumerable</returns>
        public static IEnumerable<Type> GetDeclaredPropertyTypes(this object obj)
        {
            return obj.GetType().GetProperties(BindingFlags.NonPublic
                                             | BindingFlags.Instance
                                             | BindingFlags.DeclaredOnly)
                                .Where(x => x.PropertyType.IsPrimitive)
                                .Select(x => x.PropertyType);
        }
    }
}
