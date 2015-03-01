using System;
using System.Linq;
using System.Collections.Generic;

namespace Gem.Network.Builders
{
    public class DynamicPropertyInfo
    {
        public Type PropertyType { get; set; }

        public string PropertyName { get; set; }


        #region Static Helpers

        /// <summary>
        /// Sets up a generic list of <see cref="DynamicPropertyInfo"/>
        /// </summary>
        /// <param name="types">The types of property info</param>
        /// <param name="propertyPrefix">The prefix of the type's names</param>
        /// <returns>A list of <see cref="DynamicPropertyInfo"/></returns>
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

        private static Dictionary<Type, string> PrimitiveTypesAndAliases = new Dictionary<Type, string>()
            {
                 {typeof(Byte),"byte"},
                 {typeof(SByte),"sbyte"},
                 {typeof(Int32),"int"},
                 {typeof(UInt32),"uint"},
                 {typeof(Int16),"short"},
                 {typeof(UInt16),"ushort"},
                 {typeof(Int64),"long"},
                 {typeof(UInt64),"ulong"},
                 {typeof(Single),"float"},
                 {typeof(Double),"double"},
                 {typeof(Char),"char"},
                 {typeof(Boolean),"bool"},
                 {typeof(String),"string"},
                 {typeof(Decimal),"decimal"}
            };

        private static Dictionary<Type, string> DecodeInfo = new Dictionary<Type, string>()
            {
                 {typeof(Byte),"ReadByte"},
                 {typeof(SByte),"ReadSByte"},
                 {typeof(Int32),"ReadInt32"},
                 {typeof(UInt32),"ReadUInt32"},
                 {typeof(Int16),"ReadInt16"},
                 {typeof(UInt16),"ReadUInt16"},
                 {typeof(Int64),"ReadInt64"},
                 {typeof(UInt64),"ReadUInt64"},
                 {typeof(Single),"ReadFloat"},
                 {typeof(Double),"ReadDouble"},
                //{typeof(Char),"char"},
                 {typeof(Boolean),"ReadBoolean"},
                 {typeof(String),"ReadString"},
                //{typeof(Decimal),"decimal"}
            };

        public static string GetPrimitiveTypeAlias(Type primitiveType)
        {
            if (PrimitiveTypesAndAliases.ContainsKey(primitiveType))
            {
                return PrimitiveTypesAndAliases[primitiveType];
            }
            else
            {
                throw new InvalidOperationException("Unsupported type");
            }
        }

        public static string GetDecodePrefix(Type primitiveType)
        {
            if (DecodeInfo.ContainsKey(primitiveType))
            {
                return DecodeInfo[primitiveType];
            }
            else
            {
                throw new InvalidOperationException("Unsupported type for decoding");
            }
        }

        #endregion
    }
}
