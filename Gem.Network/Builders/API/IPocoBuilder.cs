using System;
using System.Collections.Generic;

namespace Gem.Network.Builders
{
    public interface IPocoBuilder
    {
        /// <summary>
        /// Creates a new POCO type
        /// </summary>
        /// <param name="className">The POCO's name</param>
        /// <param name="propertyFields">The property names and types <see cref="PropertyInfo"/></param>
        /// <returns>The new type</returns>
        Type Build(string className, List<DynamicPropertyInfo> propertyFields);
    }
}
