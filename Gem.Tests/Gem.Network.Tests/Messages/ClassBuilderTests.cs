using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Network.Configuration;
using Gem.Network.Builders;

namespace Gem.Network.Tests
{

    [TestClass]
    public class PocoBuilderTests
    {

        [TestMethod]
        public void CreateNewPOCOClassTest()
        {
            var propertyList = new List<RuntimePropertyInfo>
            {
                new RuntimePropertyInfo{
                        PropertyName = "Name",
                        PropertyType = typeof(string)
                }
            };
            IPocoBuilder PocoBuilder = new ReflectionEmitBuilder();
            Type myNewType = PocoBuilder.Build("POCO", propertyList);

            dynamic myObject = Activator.CreateInstance(myNewType);
            Assert.IsTrue(myObject.GetType().Name == "POCO");

            var propertyInfo = myObject.GetType().GetProperty("Name");
            Assert.IsNull(propertyInfo.GetValue(myObject));

            myObject.Name = "Test";
            Assert.IsNotNull(propertyInfo.GetValue(myObject));
        }


        [TestMethod]
        public void CreateNewPOCOClassConstructorTest()
        {
            var propertyList = new List<RuntimePropertyInfo>
            {
                new RuntimePropertyInfo{
                        PropertyName = "Name",
                        PropertyType = typeof(string)
                }
            };
            IPocoBuilder PocoBuilder = new ReflectionEmitBuilder();
            Type myNewType = PocoBuilder.Build("POCO", propertyList);

            dynamic myObject = Activator.CreateInstance(myNewType,"Test");
            Assert.IsTrue(myObject.GetType().Name == "POCO");

            var propertyInfo = myObject.GetType().GetProperty("Name");
            //Assert.IsNull(propertyInfo.GetValue(myObject));

            Assert.AreEqual("Test", myObject.Name);
        }

        [TestMethod]
        public void CreateComplexPOCOClassTest()
        {
            var propertyList = new List<RuntimePropertyInfo>
            {
                new RuntimePropertyInfo{
                        PropertyName = "StringProperty",
                        PropertyType = typeof(string)
                },
                                new RuntimePropertyInfo{
                        PropertyName = "IntProperty",
                        PropertyType = typeof(int)
                },
                                new RuntimePropertyInfo{
                        PropertyName = "DoubleProperty",
                        PropertyType = typeof(double)
                },
                                new RuntimePropertyInfo{
                        PropertyName = "FloatProperty",
                        PropertyType = typeof(float)
                }                                
            };
            IPocoBuilder PocoBuilder = new ReflectionEmitBuilder();
            Type myNewType = PocoBuilder.Build("POCO", propertyList);

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

        [TestMethod]
        public void CreateComplexPOCOClassConstructorTest()
        {
            var propertyList = new List<RuntimePropertyInfo>
            {
                new RuntimePropertyInfo{
                        PropertyName = "StringProperty",
                        PropertyType = typeof(string)
                },
                                new RuntimePropertyInfo{
                        PropertyName = "IntProperty",
                        PropertyType = typeof(int)
                },
                                new RuntimePropertyInfo{
                        PropertyName = "DoubleProperty",
                        PropertyType = typeof(double)
                },
                                new RuntimePropertyInfo{
                        PropertyName = "FloatProperty",
                        PropertyType = typeof(float)
                }                                
            };
            IPocoBuilder PocoBuilder = new ReflectionEmitBuilder();
            Type myNewType = PocoBuilder.Build("POCO", propertyList);

            dynamic myObject = Activator.CreateInstance(myNewType, "String", 0, 2, 1.0f);

            Assert.AreEqual(myObject.StringProperty, "String");
            Assert.AreEqual(myObject.IntProperty, 0);
            Assert.AreEqual(myObject.DoubleProperty, 2);
            Assert.AreEqual(myObject.FloatProperty, 1.0f);
        }

        //[TestMethod]
        //public void CreatePOCOByNetConfigArgumentsTest()
        //{
        //    var types = NetworkConfig.Send("string", 1);

        //    var propertyList = new List<DynamicPropertyInfo>();
        //    int counter=0;
        //    foreach (var type in types)
        //    {
        //        propertyList.Add(new DynamicPropertyInfo
        //        {
        //            PropertyName = "A" + counter,
        //            PropertyType = type
        //        });
        //        counter++;
        //    }
        //    IPocoBuilder PocoBuilder = new ReflectionEmitBuilder();
        //    Type myNewType = PocoBuilder.Build("POCO", propertyList);

        //    dynamic myObject = Activator.CreateInstance(myNewType);
        //    Assert.IsTrue(myObject.GetType().Name == "POCO");

        //    var stringProperty = myObject.GetType().GetProperty("A0");
        //    var intProperty = myObject.GetType().GetProperty("A1");

        //    Assert.IsNull(stringProperty.GetValue(myObject));

        //    myObject.A0 = "String";
        //    myObject.A1 = 1;
            
        //    Assert.AreEqual(stringProperty.GetValue(myObject), "String");
        //    Assert.AreEqual(intProperty.GetValue(myObject), 1);   
        //}
    }
}
