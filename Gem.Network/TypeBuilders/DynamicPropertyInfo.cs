using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.DynamicBuilders
{
    public class DynamicPropertyInfo
    {
        public Type PropertyType { get; set; }
        public string PropertyName { get; set; }

        public static List<DynamicPropertyInfo> GetPropertyInfo(Type[] types, string propertyPrefix = "pprefix")
        {
            var propertyInfo = new List<DynamicPropertyInfo>();

            for (int i = 0; i < types.Count(); i++)
            {
                propertyInfo.Add(new DynamicPropertyInfo
                {
                    PropertyName = propertyPrefix + i,
                    PropertyType = types[i]
                });
            }

            return propertyInfo;
        }
    }
}
