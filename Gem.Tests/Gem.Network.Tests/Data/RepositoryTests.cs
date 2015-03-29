using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Network.Builders;
using Gem.Network.Repositories;

namespace Gem.Network.Tests.Repository
{

    [TestClass]
    public class RepositoryTests
    {

        [TestMethod]
        public void RegisterTypesAndResolveTest()
        {
            #region Types Definitions

            var typeInfo1 = new List<RuntimePropertyInfo>
            {
                new RuntimePropertyInfo{
                        PropertyName = "Type1",
                        PropertyType = typeof(string)
                }
            };

            var typeInfo2 = new List<RuntimePropertyInfo>
            {
                new RuntimePropertyInfo{
                        PropertyName = "Type2",
                        PropertyType = typeof(string)
                }
            };

            var typeInfo3 = new List<RuntimePropertyInfo>
            {
                new RuntimePropertyInfo{
                        PropertyName = "Type3",
                        PropertyType = typeof(string)
                }
            };

            #endregion

            IPocoBuilder PocoBuilder = new ReflectionEmitBuilder();
            Type type1 = PocoBuilder.Build("Type1", typeInfo1);
            Type type2 = PocoBuilder.Build("Type2", typeInfo2);
            Type type3 = PocoBuilder.Build("Type3", typeInfo3);

            var typeRepository = new FlyweightRepository<Type,string>();
            typeRepository.Add("Type1", type1);
            typeRepository.Add("Type2", type2);
            typeRepository.Add("Type3", type3);


            Assert.IsTrue(typeRepository.GetById("Type1").Equals(type1));
            Assert.IsTrue(typeRepository.GetById("Type2").Equals(type2));
            Assert.IsTrue(typeRepository.GetById("Type3").Equals(type3));
        }

    }
}

