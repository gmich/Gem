using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Network.Utilities;
using Gem.Network;
using Gem.Network.Builders;

namespace Gem.Network.Tests
{
    using Extensions;
    using Gem.Network.Events;

    [TestClass]
    public class ObjectExtensionsTests
    {

        [TestMethod]
        public void SetDynamicObjectValueTest()
        {
            var propertyList = new List<DynamicPropertyInfo>
            {
                new DynamicPropertyInfo{
                        PropertyName = "StringParameter",
                        PropertyType = typeof(string)
                },      
                new DynamicPropertyInfo{
                        PropertyName = "IntParameter",
                        PropertyType = typeof(Byte)
                }
            };
            IPocoBuilder PocoBuilder = new ReflectionEmitBuilder();
            Type myNewType = PocoBuilder.Build("POCO", propertyList);

            object myObject = Activator.CreateInstance(myNewType, "string", (byte)1);
            var value = myObject.GetValue("StringParameter");
            Assert.AreEqual(value, "string");
        }

        [TestMethod]
        public void SetDynamicObjectValueTest_CsScript()
        {
            var propertyList = new List<DynamicPropertyInfo>
            {
                new DynamicPropertyInfo{
                        PropertyName = "StringParameter",
                        PropertyType = typeof(string)
                },      
                new DynamicPropertyInfo{
                        PropertyName = "IntParameter",
                        PropertyType = typeof(Byte)
                }
            };
            IPocoBuilder PocoBuilder = new CsScriptPocoBuilder();
            Type myNewType = PocoBuilder.Build("POCO", propertyList);

            var myObject = (INetworkPackage)Activator.CreateInstance(myNewType, "string", (byte)1);
            myObject.Id= 1;
            Assert.AreEqual(myObject.Id, 1);
        }
        

        [TestMethod]
        public void GetSetDynamicObjectValueTest()
        {
            var propertyList = new List<DynamicPropertyInfo>
            {
                new DynamicPropertyInfo{
                        PropertyName = "Name",
                        PropertyType = typeof(string)
                }
            };
            IPocoBuilder PocoBuilder = new ReflectionEmitBuilder();
            Type myNewType = PocoBuilder.Build("POCO", propertyList);
            object myObject = Activator.CreateInstance(myNewType);

            myObject.SetValue("Name", "Value");
            var value = myObject.GetValue("Name");

            Assert.AreEqual("Value", value);
        }
    }
}
