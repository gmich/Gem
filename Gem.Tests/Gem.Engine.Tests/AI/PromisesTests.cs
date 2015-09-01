using Gem.AI.Promises;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gem.Engine.Tests.AI
{
    [TestClass]
    public class PromisesTests
    {

        [TestMethod]
        public void SimplePromiseResolveTest()
        {
            var promisedValue = 5;
            var promise = Promise<int>.Resolved(promisedValue);

            var completed = 0;
            promise.Then(v =>
            {
                Assert.AreEqual(promisedValue, v);
                ++completed;
            });

            Assert.AreEqual(1, completed);
        }
    }
}
