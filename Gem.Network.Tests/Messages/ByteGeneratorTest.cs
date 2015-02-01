using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Network.Other;

namespace Gem.Network.Tests
{
    [TestClass]
    public class ByteGeneratorTests
    {
        [TestMethod]
        public void UniqueByteArrayInitializationTest()
        {
            int arraylength = byte.MaxValue;
            var uniqueByteArray = RandomGenerator.GetRandomBytesArray(arraylength);

            //check if all elements are unique
            Assert.IsTrue(uniqueByteArray.Distinct().Count() == arraylength);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException),
        "Raise exception when the array length is not within the supported boundaries")]
        public void InvalidLengthForByteArrayThrowsExceptionTest()
        {
            int arraylength = byte.MaxValue + 1;
            var uniqueByteArray = RandomGenerator.GetRandomBytesArray(arraylength);

            Assert.IsTrue(uniqueByteArray.Distinct().Count() == arraylength);
        }
    }
}
