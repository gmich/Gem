using Gem.Network.Builders;
using System;
using System.Collections.Generic;

namespace Gem.Network.Factories
{
    /// <summary>
    /// A factory for creating runtime POCOs
    /// </summary>
    public interface IPocoFactory
    {
        /// <summary>
        /// Creates runtime POCOs 
        /// </summary>
        /// <param name="propertyInfo">The poco's properties names and types</param>
        /// <param name="classname">The poco's name</param>
        /// <returns>The POCO's type</returns>
        Type Create(List<RuntimePropertyInfo> propertyInfo, string classname);
    }
}
