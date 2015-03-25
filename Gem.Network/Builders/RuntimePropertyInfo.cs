using System;
using System.Linq;
using System.Collections.Generic;
using Gem.Network.Extensions;

namespace Gem.Network.Builders
{
    /// <summary>
    /// Holds information that are passed to IPocoBuilder to build runtime types
    /// </summary>
    public class RuntimePropertyInfo
    {
        #region Fields

        /// <summary>
        /// The runtime type
        /// </summary>
        public Type PropertyType { get; set; }
        
        /// <summary>
        /// Its invocation name
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// The prefix of the property name.
        /// This is used for identifying runtime created types
        /// </summary>
        internal const string PropertyPrefix = "pprefix";

        #endregion

        #region Static Helpers

        /// <summary>
        /// Sets up a generic list of <see cref="RuntimePropertyInfo"/>
        /// </summary>
        /// <param name="types">The types of property info</param>
        /// <param name="propertyPrefix">The prefix of the type's names</param>
        /// <returns>A list of <see cref="RuntimePropertyInfo"/></returns>
        public static List<RuntimePropertyInfo> GetPropertyInfo(Type[] types)
        {
            var propertyInfo = new List<RuntimePropertyInfo>();

            for (int i = 0; i < types.Count(); i++)
            {
                propertyInfo.Add(new RuntimePropertyInfo
                {
                    PropertyName = PropertyPrefix + i,
                    PropertyType = types[i]
                });
            }

            return propertyInfo;
        }


        /// <summary>
        /// Uses reflection to retrieve an object's fields types and nammes as 
        /// dynamic property info
        /// </summary>
        /// <typeparam name="T">The object to reflect</typeparam>
        /// <returns>An inenumerable holding the propety names and types</returns>
        public static IEnumerable<RuntimePropertyInfo> GetPropertyTypesAndNames<T>()
            where T : new()
        {
            return new T().GetType()
                      .GetProperties()
                      .Select(x => new RuntimePropertyInfo
                      {
                          PropertyName = x.Name,
                          PropertyType = x.PropertyType
                      });
        }

        /// <summary>
        /// Factory method that uses the PropertyPrefeix to return a dynamic property info
        /// </summary>
        /// <param name="types">The type</param>
        /// <param name="order">The index of the property prefix</param>
        /// <returns>A dynamic property info</returns>
        public static RuntimePropertyInfo GetPropertyInfo(Type types, int order)
        {
                return new RuntimePropertyInfo
                {
                    PropertyName = PropertyPrefix + order,
                    PropertyType = types
                };
        }
        
        /// <summary>
        /// Matches the primitive types to their string representation
        /// </summary>
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

        /// <summary>
        /// This is used in the constructor body of CsScriptPOCOBuilder
        /// </summary>
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

        /// <summary>
        /// Returns the primitive type's string representation that's used to declare it
        /// e.g. a System.Single is represented as float.
        /// NOTICE: only primitive types
        /// </summary>
        /// <param name="primitiveType">The type</param>
        /// <returns>A string of the type's declaration</returns>
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

        /// <summary>
        /// Returns the lidgren decoding invocation for network incoming message
        /// as a string, according to the primitive type
        /// NOTICE: only primitive types
        /// </summary>
        /// <param name="primitiveType">The primitive type</param>
        /// <returns>A string representing the decoding invocation</returns>
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
