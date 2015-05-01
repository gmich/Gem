using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using Gem.Infrastructure.Attributes;

namespace Gem.Infrastructure.Tests
{
    [TestClass]
    public class AttributeTests
    {

        #region Runtime Mapper Dummy Objects

        /// <summary>
        /// This is used to test the DefaultValueHelper
        /// </summary>
        public class ClassWithDefaultValues
        {
            [DefaultValue(999)]
            public int Id { get; set; }

            [DefaultValue("DefaultValue")]
            public string Name { get; set; }
        }

        #endregion

        [TestMethod]
        [TestCategory("DefaultValueAttribute")]
        public void AttributeHelper_RetrievesDefaultValues()
        {
            var objWithDefaultValues = new ClassWithDefaultValues();

            Assert.AreEqual(999, objWithDefaultValues.GetDefaultValueFor(x => x.Id));
            Assert.AreEqual("DefaultValue", objWithDefaultValues.GetDefaultValueFor(x => x.Name));
        }

        [TestMethod]
        [TestCategory("DefaultValueAttribute")]
        public void AttributeHelper_AssignsDefaultValuesToInstance()
        {
            var objWithDefaultValues = new ClassWithDefaultValues();
            objWithDefaultValues.AssignDefaultValues();

            Assert.AreEqual(999, objWithDefaultValues.Id);
            Assert.AreEqual("DefaultValue", objWithDefaultValues.Name);
        }
    }
}
