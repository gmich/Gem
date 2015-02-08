using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Network.Configuration;
using Gem.Network.Cache;
using Gem.Network.DynamicBuilders;

namespace Gem.Network.Tests.Repository
{

    [TestClass]
    public class RepositoryTests
    {

        [TestMethod]
        public void RegisterTypesAndResolveTest()
        {
            #region Types Definitions

            var typeInfo1 = new List<DynamicPropertyInfo>
            {
                new DynamicPropertyInfo{
                        PropertyName = "Type1",
                        PropertyType = typeof(string)
                }
            };

            var typeInfo2 = new List<DynamicPropertyInfo>
            {
                new DynamicPropertyInfo{
                        PropertyName = "Type2",
                        PropertyType = typeof(string)
                }
            };

            var typeInfo3 = new List<DynamicPropertyInfo>
            {
                new DynamicPropertyInfo{
                        PropertyName = "Type3",
                        PropertyType = typeof(string)
                }
            };

            #endregion

            Type type1 = PocoBuilder.Create("Type1", typeInfo1);
            Type type2 = PocoBuilder.Create("Type2", typeInfo2);
            Type type3 = PocoBuilder.Create("Type3", typeInfo3);

            var typeRepository = new TypeRepository();
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
            var typeInfo1 = new List<DynamicPropertyInfo>
            {
                new DynamicPropertyInfo{
                        PropertyName = "Type1",
                        PropertyType = typeof(string)
                }
            };
                  
            Type type1 = PocoBuilder.Create("Type1", typeInfo1);
            
            var typeRepository = new TypeRepository();

            typeRepository.RegisterType("Type1", type1);
            typeRepository.RegisterType("Type1", type1);
        }
    }
}

