using System;
using System.Collections.Generic;

namespace Gem.Infrastructure.Configuration
{

    /// <summary>
    /// Factory for class-based items.
    /// </summary>
    /// <typeparam name="TBaseType">The base type of each item.</typeparam>
    /// <typeparam name="TAttributeType">The type of the attribute used to annotate items.</typeparam>
    internal class Factory<TBaseType, TAttributeType> : INamedItemFactory<TBaseType, Type>, IFactory
        where TBaseType : class
        where TAttributeType : NameBaseAttribute
    {
        private readonly Dictionary<string, GetTypeDelegate> items = new Dictionary<string, GetTypeDelegate>(StringComparer.OrdinalIgnoreCase);
        private ConfigurationItemFactory parentFactory;

        internal Factory(ConfigurationItemFactory parentFactory)
        {
            this.parentFactory = parentFactory;
        }

        private delegate Type GetTypeDelegate();

        /// <summary>
        /// Scans the assembly.
        /// </summary>
        /// <param name="types">The types to scan.</param>
        /// <param name="prefix">The prefix.</param>
        public void ScanTypes(Type[] types, string prefix)
        {
            foreach (Type t in types)
            {
                try
                {
                    this.RegisterType(t, prefix);
                }
                catch (Exception ex)
                {
                    Gem.Infrastructure.Logging.Auditor.Logger.Error("Failed to register types {0} with prefix {1}. {2}", types, prefix, ex);
                }
            }
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="itemNamePrefix">The item name prefix.</param>
        public void RegisterType(Type type, string itemNamePrefix)
        {
            TAttributeType[] attributes = (TAttributeType[])type.GetCustomAttributes(typeof(TAttributeType), false);
            if (attributes != null)
            {
                foreach (TAttributeType attr in attributes)
                {
                    this.RegisterDefinition(itemNamePrefix + attr.Name, type);
                }
            }
        }

        /// <summary>
        /// Registers the item based on a type name.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="typeName">Name of the type.</param>
        public void RegisterNamedType(string itemName, string typeName)
        {
            this.items[itemName] = () => Type.GetType(typeName, false);
        }

        /// <summary>
        /// Clears the contents of the factory.
        /// </summary>
        public void Clear()
        {
            this.items.Clear();
        }

        /// <summary>
        /// Registers a single type definition.
        /// </summary>
        /// <param name="name">The item name.</param>
        /// <param name="type">The type of the item.</param>
        public void RegisterDefinition(string name, Type type)
        {
            this.items[name] = () => type;
        }

        /// <summary>
        /// Tries to get registered item definition.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="result">Reference to a variable which will store the item definition.</param>
        /// <returns>Item definition.</returns>
        public bool TryGetDefinition(string itemName, out Type result)
        {
            GetTypeDelegate del;

            if (!this.items.TryGetValue(itemName, out del))
            {
                result = null;
                return false;
            }

            try
            {
                result = del();
                return result != null;
            }
            catch (Exception ex)
            {
                Gem.Infrastructure.Logging.Auditor.Logger.Error("Unable to get definition for item {0}. {1}", itemName, ex);
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Tries to create an item instance.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="result">The result.</param>
        /// <returns>True if instance was created successfully, false otherwise.</returns>
        public bool TryCreateInstance(string itemName, out TBaseType result)
        {
            Type type;

            if (!this.TryGetDefinition(itemName, out type))
            {
                result = null;
                return false;
            }

            result = (TBaseType)this.parentFactory.CreateInstance(type);
            return true;
        }

        /// <summary>
        /// Creates an item instance.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <returns>Created item.</returns>
        public TBaseType CreateInstance(string name)
        {
            TBaseType result;

            if (this.TryCreateInstance(name, out result))
            {
                return result;
            }

            throw new ArgumentException(typeof(TBaseType).Name + " cannot be found: '" + name + "'");
        }
    }
}
