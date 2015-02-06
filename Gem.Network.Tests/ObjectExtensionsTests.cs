using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Network.Utilities;
using Gem.Network.DynamicBuilders;

namespace Gem.Network.Tests
{

    [TestClass]
    public class ObjectExtensionsTests
    {

        [TestMethod]
        public void SetDynamicObjectValueTest()
        {
            var propertyList = new List<DynamicPropertyInfo>
            {
                new DynamicPropertyInfo{
                        PropertyName = "Name",
                        PropertyType = typeof(string)
                }
            };

            Type myNewType = PocoBuilder.Create("POCO", propertyList);

            object myObject = Activator.CreateInstance(myNewType);
            var value = myObject.GetValue("Name");
            Assert.IsNull(value);
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

            Type myNewType = PocoBuilder.Create("POCO", propertyList);
            object myObject = Activator.CreateInstance(myNewType);

            myObject.SetValue("Name", "Value");
            var value = myObject.GetValue("Name");

            Assert.AreEqual("Value", value);
        }
    }
}
