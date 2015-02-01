using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gem.Network.Tests
{
    [TestClass]
    public class ClassBuilderTests
    {
        [TestMethod]
        public void CreateNewPOCOClassTest()
        {
            var propertyList = new List<PropertyInfo>
            {
                new PropertyInfo{
                        PropertyName = "Name",
                        PropertyType = typeof(string)
                }
            };

            Type myNewType = ClassBuilder.CreateNewObject("POCO", propertyList);

            dynamic myObject = Activator.CreateInstance(myNewType);
            Assert.IsTrue(myObject.GetType().Name == "POCO");

            var propertyInfo = myObject.GetType().GetProperty("Name");
            Assert.IsNull(propertyInfo.GetValue(myObject));

            myObject.Name = "Test";
            Assert.IsNotNull(propertyInfo.GetValue(myObject));
        }

        [TestMethod]
        public void CreateComplexPOCOClassTest()
        {
            var propertyList = new List<PropertyInfo>
            {
                new PropertyInfo{
                        PropertyName = "StringProperty",
                        PropertyType = typeof(string)
                },
                                new PropertyInfo{
                        PropertyName = "IntProperty",
                        PropertyType = typeof(int)
                },
                                new PropertyInfo{
                        PropertyName = "DoubleProperty",
                        PropertyType = typeof(double)
                },
                                new PropertyInfo{
                        PropertyName = "FloatProperty",
                        PropertyType = typeof(float)
                }                                
            };

            Type myNewType = ClassBuilder.CreateNewObject("POCO", propertyList);

            dynamic myObject = Activator.CreateInstance(myNewType);
            Assert.IsTrue(myObject.GetType().Name == "POCO");

            var stringProperty = myObject.GetType().GetProperty("StringProperty");
            var intProperty = myObject.GetType().GetProperty("IntProperty");
            var doubleProperty = myObject.GetType().GetProperty("DoubleProperty");
            var floatProperty = myObject.GetType().GetProperty("FloatProperty");

            Assert.IsNull(stringProperty.GetValue(myObject));


            myObject.StringProperty = "String";
            myObject.IntProperty = 0;
            myObject.DoubleProperty = 2;
            myObject.FloatProperty = 1.0f;

            Assert.AreEqual(stringProperty.GetValue(myObject), "String");
            Assert.AreEqual(intProperty.GetValue(myObject), 0);
            Assert.AreEqual(doubleProperty.GetValue(myObject), 2);
            Assert.AreEqual(floatProperty.GetValue(myObject), 1.0f);
        }
    }
}
