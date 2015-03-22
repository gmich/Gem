using Gem.Network.Protocol;
using Gem.Network.Utilities.Loggers;
using System;
using System.Linq;

namespace Gem.Network.Utilities
{
    /// <summary>
    /// Helper class for attribute resolving 
    /// </summary>
    public sealed class AttributeResolver
    {
        /// <summary>
        /// Checks if the class is marked with the specified attribute
        /// </summary>
        /// <param name="type">The type to check</param>
        public static void FindAndDo<T>(Type type, Action<T> action)
            where T : Attribute
        {
            T Attr;

            //Querying Class Attributes
            foreach (Attribute attr in type.GetCustomAttributes(true))
            {
                Attr = attr as T;
                if (null != Attr)
                {
                    action(Attr);
                }
            }
        }

        /// <summary>
        /// Finds all types in all the referenced assemblies that are tagged with TAttribute
        /// and invokes an action with the tagged type as the parameter
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute</typeparam>
        /// <param name="action">The delegate that's invoked with the TAttribute's types</param>
        /// <param name="isInherited">Check for inherited attributes too</param>
        public static void DoWithAllTypesWithAttribute<TAttribute>(Action<Type, TAttribute> action,
                                                                   bool isInherited = true)
                                                                   where TAttribute : Attribute
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                if (assembly == null) continue;

                try
                {
                    assembly.GetTypes()
                            .Where(x => x.IsDefined(typeof(TAttribute), isInherited))
                            .Select(x => new
                            {
                                Type = x,
                                Attribute = x.GetCustomAttributes(typeof(TAttribute), isInherited).First()
                            })
                            .ToList()
                            .ForEach(x => action(x.Type, x.Attribute as TAttribute));
                }
                catch (Exception ex)
                {
                    GemNetworkDebugger.Append.Error("Failed to load assembly {0}. Reason: {1}",assembly, ex.Message);
                }
            }
        }
    }
}

