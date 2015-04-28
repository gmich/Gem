using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Gem.Infrastructure.Configuration
{
    
    /// <summary>
    /// Provides registration information for named items
    /// </summary>
    public class ConfigurationItemFactory
    {
        private readonly IList<object> allFactories;

        /// <summary>
        /// Initializes static members of the <see cref="ConfigurationItemFactory"/> class.
        /// </summary>
        static ConfigurationItemFactory()
        {
            //TODO: provide configuration type
            Default = BuildDefaultFactory<ConfigurationItemFactory>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationItemFactory"/> class.
        /// </summary>
        /// <param name="assemblies">The assemblies to scan for named items.</param>
        public ConfigurationItemFactory(params Assembly[] assemblies)
        {
            this.CreateInstance = FactoryHelper.CreateInstance;


            foreach (var asm in assemblies)
            {
                this.RegisterItemsFromAssembly(asm);
            }
        }

        /// <summary>
        /// Gets or sets default singleton instance of <see cref="ConfigurationItemFactory"/>.
        /// </summary>
        public static ConfigurationItemFactory Default { get; set; }

        /// <summary>
        /// Gets or sets the creator delegate used to instantiate configuration objects.
        /// </summary>
        /// <remarks>
        /// By overriding this property, one can enable dependency injection or interception for created objects.
        /// </remarks>
        public ConfigurationItemCreator CreateInstance { get; set; }


        /// <summary>
        /// Registers named items from the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void RegisterItemsFromAssembly(Assembly assembly)
        {
            this.RegisterItemsFromAssembly(assembly, string.Empty);
        }

        /// <summary>
        /// Registers named items from the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="itemNamePrefix">Item name prefix.</param>
        public void RegisterItemsFromAssembly(Assembly assembly, string itemNamePrefix)
        {
            //TODO: log
            var typesToScan = assembly.SafeGetTypes();
            foreach (IFactory f in this.allFactories)
            {
                f.ScanTypes(typesToScan, itemNamePrefix);
            }
        }

        /// <summary>
        /// Clears the contents of all factories.
        /// </summary>
        public void Clear()
        {
            foreach (IFactory f in this.allFactories)
            {
                f.Clear();
            }
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="itemNamePrefix">The item name prefix.</param>
        public void RegisterType(Type type, string itemNamePrefix)
        {
            foreach (IFactory f in this.allFactories)
            {
                f.RegisterType(type, itemNamePrefix);
            }
        }

        /// <summary>
        /// Builds the default configuration item factory.
        /// </summary>
        /// <returns>Default factory.</returns>
        private static ConfigurationItemFactory BuildDefaultFactory<TConfigurationAssembly>()
        {
            var assembly = typeof(TConfigurationAssembly).Assembly;
            var factory = new ConfigurationItemFactory(assembly);
            var assemblyLocation = Path.GetDirectoryName(assembly.Location);
            if (assemblyLocation == null)
            {
                return factory;
            }

            var extensionDlls = Directory.GetFiles(assemblyLocation, "Gem*.dll")
                .Select(Path.GetFileName)
                .Where(x => !x.Equals("Gem.Tests.dll", StringComparison.OrdinalIgnoreCase))
                .Select(x => Path.Combine(assemblyLocation, x));
            foreach (var extensionDll in extensionDlls)
            {
                //TODO: log
                var extensionAssembly = Assembly.LoadFrom(extensionDll);
                factory.RegisterItemsFromAssembly(extensionAssembly);
            }
            return factory;
        }

    }
}
