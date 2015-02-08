using Gem.Network.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Gem.Network.Tests.Cache
{
    [TestClass]
    public class GenericCacheTests
    {

        public class ArrayTypeEquality : EqualityComparer<Type[]>
        {
            public override int GetHashCode(Type[] type)
            {
                return type.GetHashCode();
            }

            public override bool Equals(Type[] type1, Type[] type2)
            {
                return type1.SequenceEqual(type2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException),
        "Raise exception when the memory used is more than the buffer")]
        public void InsertToCacheWhenBufferIsFullThrowsException()
        {
            long byteCapacity = 10;
            var typeCache = new GCache<Type[], Type>(byteCapacity, new ArrayTypeEquality());

            //this will full up the buffer
            typeCache.Add(new Type[] { typeof(string) }, typeof(int));
        }

        [TestMethod]
        public void CacheLookupTest()
        {
            long byteCapacity = 100000;
            var typeCache = new GCache<Type[], Type>(byteCapacity, new ArrayTypeEquality());

            var typeKey = new Type[] { typeof(string), typeof(int) };
            var typeValue = typeof(int);

            typeCache.Add(typeKey, typeValue);
            typeCache.Add(new Type[] { typeof(int) }, typeof(string));
            typeCache.Add(new Type[] { typeof(string), typeof(float) }, typeof(string));
            typeCache.Add(new Type[] { typeof(object), typeof(int) }, typeof(string));

            var lookupResult = typeCache.Lookup(typeKey);

            Assert.AreEqual(typeValue, lookupResult);
        }

        [TestMethod]
        public void CacheDeallocatesLeastIteratedEntryTest()
        {
            long byteCapacity = 2000;
            var typeCache = new GCache<Type[], Type>(byteCapacity, new ArrayTypeEquality());
            typeCache.Events.OnUsedMemoryEvent += ((sender, args) => Trace.WriteLine(args.ByteSize));

            var typeKey = new Type[] { typeof(string), typeof(int) };
            var typeValue = typeof(int);

            typeCache.Add(typeKey, typeValue);
            typeCache.Add(new Type[] { typeof(int) }, typeof(string));
            typeCache.Add(new Type[] { typeof(string), typeof(float) }, typeof(string));

            var lookupResult = typeCache.Lookup(typeKey);

            typeCache.Add(new Type[] { typeof(object), typeof(int) }, typeof(string));

            Assert.AreEqual(typeValue, lookupResult);
        }

        [TestMethod]
        public void CacheMemoryManagementTest()
        {
            // Trace.Listeners.Add(new TextWriterTraceListener("CacheMemoryManagementTest.log", "cache"));
            long byteCapacity = 2000;
            int twosec = 1000;
            var typeCache = new ManagedCache<Type[], Type>(byteCapacity, new ArrayTypeEquality(), twosec);
            typeCache.Events.OnUsedMemoryEvent += ((sender, args) =>  Trace.WriteLine(args.ByteSize));

            var typeValue = typeof(int);

            //Fill the buffer
            typeCache.Add(new Type[] { typeof(string), typeof(int) }, typeValue);
            typeCache.Add(new Type[] { typeof(string), typeof(int), typeof(string) }, typeValue);
            typeCache.Add(new Type[] { typeof(int), typeof(string) }, typeValue);

            System.Threading.Thread.Sleep(2000);
            
            //Add successfully when the background thread frees the cache's resources
            typeCache.Add(new Type[] { typeof(string) }, typeValue);
            // Trace.Flush();

        }
    }
}

   
    

