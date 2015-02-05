using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Network.Configuration;
using Gem.Network.Cache;

namespace Gem.Network.Tests.Cache
{

    [TestClass]
    public class CacheTests
    {

        [TestMethod]
        public void RegisterTypesAndResolveTest()
        {
            #region Types Definitions

            var typeInfo1 = new List<PropertyInfo>
            {
                new PropertyInfo{
                        PropertyName = "Type1",
                        PropertyType = typeof(string)
                }
            };

            var typeInfo2 = new List<PropertyInfo>
            {
                new PropertyInfo{
                        PropertyName = "Type2",
                        PropertyType = typeof(string)
                }
            };

            var typeInfo3 = new List<PropertyInfo>
            {
                new PropertyInfo{
                        PropertyName = "Type3",
                        PropertyType = typeof(string)
                }
            };

            #endregion

            Type type1 = ClassBuilder.CreateNewObject("Type1", typeInfo1);
            Type type2 = ClassBuilder.CreateNewObject("Type2", typeInfo2);
            Type type3 = ClassBuilder.CreateNewObject("Type3", typeInfo3);

            var typeRepository = new NetPocoRepository();
            typeRepository.RegisterType("Type1", type1);
            typeRepository.RegisterType("Type2", type2);
            typeRepository.RegisterType("Type3", type3);


            Assert.IsTrue(typeRepository.CreateObject("Type1").GetType().Equals(type1));
            Assert.IsTrue(typeRepository.CreateObject("Type2").GetType().Equals(type2));
            Assert.IsTrue(typeRepository.CreateObject("Type3").GetType().Equals(type3));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
        "Raise exception when resolving a type that's not registered")]
        public void RegisterRegisteredTypeThrowsExceptionTest()
        {
            var typeInfo1 = new List<PropertyInfo>
            {
                new PropertyInfo{
                        PropertyName = "Type1",
                        PropertyType = typeof(string)
                }
            };
                  
            Type type1 = ClassBuilder.CreateNewObject("Type1", typeInfo1);
            
            var typeRepository = new NetPocoRepository();

            typeRepository.RegisterType("Type1", type1);
            typeRepository.RegisterType("Type1", type1);
        }
    }
}

