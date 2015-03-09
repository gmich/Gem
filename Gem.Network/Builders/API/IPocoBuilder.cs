using System;
using System.Collections.Generic;

namespace Gem.Network.Builders
{
    public interface IPocoBuilder
    {
        /// <summary>
        /// Creates a runtime POCO.
        /// The POCO is used to store the properties of the network messages
        /// </summary>
        /// <param name="className">The POCO's name</param>
        /// <param name="propertyFields">The property names and types <see cref="PropertyInfo"/></param>
        /// <returns>The new type</returns>
        Type Build(string className, List<DynamicPropertyInfo> propertyFields);
    }
}
