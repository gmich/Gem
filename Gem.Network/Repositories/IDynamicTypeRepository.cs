using System;

namespace Gem.Network.Cache
{
    /// <summary>
    /// Repository for in-memory types
    /// The types are registered and then initialized using the reflection activator
    /// </summary>
    public interface IDynamicTypeRepository
    {
        /// <summary>
        /// Registers a new type
        /// </summary>
        /// <param name="typeID">The type's unique id</param>
        /// <param name="type"The dynamic created type></param>
        void RegisterType(string typeID, Type type);

        /// <summary>
        /// Deregisters a type 
        /// </summary>
        /// <param name="typeID">The type's unique id</param>
        void DeregisterType(string typeID);

        /// <summary>
        /// Returns a new instance of Type
        /// </summary>
        /// <param name="typeID">The type's id</param>
        /// <returns>A new instance of the dynamicly created type</returns>
        object CreateObject(string typeID);

        /// <summary>
        /// Returns a new instance of Type
        /// </summary>
        /// <param name="typeID">The poco's id</param>
        /// <param name="constructorParams">The constructors parameters</param>
        /// <returns>A new instance of the dynamicly created type</returns>
        object CreateObject(string typeID, params object[] constructorParams);
    }
}
