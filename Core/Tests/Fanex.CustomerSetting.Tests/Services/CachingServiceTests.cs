namespace Fanex.CustomerSetting.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;
    using System.Threading;
    using System.Threading.Tasks;
    using CustomerSetting.Services.Impl;
    using NUnit.Framework;

    [TestFixture]
    public class CachingServiceTests
    {
        private const string TestKey = "CachingServiceTests.TestKey";

        [TearDown]
        public void TearDown()
        {
            MemoryCache.Default.Remove(TestKey);
        }

        [Test]
        public void CachingService_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(()=> new CachingService(null));
        }

        [Test]
        public void AddComplexObjectThenGetGenericReturnsCachedObject()
        {
            var sut = new CachingService();
            sut.Add(TestKey, new ComplexTestObject());
            var actual = sut.Get<ComplexTestObject>(TestKey);
            var expected = new ComplexTestObject();
            Assert.AreEqual(ComplexTestObject.SomeMessage, ComplexTestObject.SomeMessage);
            Assert.AreEqual(expected.SomeItems, actual.SomeItems);
        }

        [Test]
        public void AddComplexObjectThenGetReturnsCachedObject()
        {
            var sut = new CachingService();
            sut.Add(TestKey, new ComplexTestObject());
            var actual = sut.Get<ComplexTestObject>(TestKey);
            var expected = new ComplexTestObject();
            Assert.AreEqual(ComplexTestObject.SomeMessage, ComplexTestObject.SomeMessage);
            Assert.AreEqual(expected.SomeItems, actual.SomeItems);
        }

        [Test]
        public void AddEmptyKeyThrowsException()
        {
            var sut = new CachingService();

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Add(string.Empty, new object()));
        }

        [Test]
        public void AddEmptyKeyThrowsExceptionWithExpiration()
        {
            var sut = new CachingService();

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Add(string.Empty, new object(), DateTimeOffset.Now.AddHours(1)));
        }

        [Test]
        public void AddEmptyKeyThrowsExceptionWithPolicy()
        {
            var sut = new CachingService();

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Add(string.Empty, new object(), new CacheItemPolicy()));
        }

        [Test]
        public void AddEmptyKeyThrowsExceptionWithSliding()
        {
            var sut = new CachingService();

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Add(string.Empty, new object(), new TimeSpan(1000)));
        }

        [Test]
        public void AddNullKeyThrowsException()
        {
            var sut = new CachingService();

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Add(null, new object()));
        }

        [Test]
        public void AddNullKeyThrowsExceptionWithExpiration()
        {
            var sut = new CachingService();

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Add(null, new object(), DateTimeOffset.Now.AddHours(1)));
        }

        [Test]
        public void AddNullKeyThrowsExceptionWithPolicy()
        {
            var sut = new CachingService();

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Add(null, new object(), new CacheItemPolicy()));
        }

        [Test]
        public void AddNullKeyThrowsExceptionWithSliding()
        {
            var sut = new CachingService();

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Add(null, new object(), new TimeSpan(1000)));
        }

        [Test]
        public void AddNullThrowsException()
        {
            var sut = new CachingService();

            Assert.Throws<ArgumentNullException>(() => sut.Add<object>(TestKey, null));
        }

        [Test]
        public void AddThenGetReturnsCachedObject()
        {
            var sut = new CachingService();
            sut.Add(TestKey, "testObject");
            Assert.AreEqual("testObject", sut.Get<string>(TestKey));
        }

        [Test]
        public void AddWithOffsetReturnsCachedItem()
        {
            var sut = new CachingService();
            sut.Add(TestKey, "testObject", DateTimeOffset.Now.AddSeconds(1));
            Assert.AreEqual("testObject", sut.Get<string>(TestKey));
        }

        [Test]
        public void AddWithOffsetThatExpiresReturnsNull()
        {
            var sut = new CachingService();
            sut.Add(TestKey, "testObject", DateTimeOffset.Now.AddSeconds(1));
            Thread.Sleep(1500);
            Assert.IsNull(sut.Get<string>(TestKey));
        }

        [Test]
        public void AddWithPolicyReturnsCachedItem()
        {
            var sut = new CachingService();
            sut.Add(TestKey, "testObject", new CacheItemPolicy());
            Assert.AreEqual("testObject", sut.Get<string>(TestKey));
        }

        [Test]
        public void AddWithSlidingReturnsCachedItem()
        {
            var sut = new CachingService();
            sut.Add(TestKey, "testObject", new TimeSpan(0, 0, 10));
            Assert.AreEqual("testObject", sut.Get<string>(TestKey));
        }

        [Test]
        public void AddWithSlidingThatExpiresReturnsNull()
        {
            var sut = new CachingService();
            sut.Add(TestKey, "testObject", new TimeSpan(0, 0, 1));
            Thread.Sleep(2000);
            Assert.IsNull(sut.Get<string>(TestKey));
        }

        [Test]
        public void GetCachedNullableStructTypeParamReturnsType()
        {
            var sut = new CachingService();
            DateTime? cached = new DateTime();
            sut.Add(TestKey, cached);
            Assert.AreEqual(cached.Value, sut.Get<DateTime>(TestKey));
        }

        [Test]
        public void GetEmptyKeyThrowsException()
        {
            var sut = new CachingService();

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Get<object>(string.Empty));
        }

        [Test]
        public void GetFromCacheTwiceAtSameTimeOnlyAddsOnce()
        {
            var times = 0;
            var sut = new CachingService();
            var t1 = Task.Factory.StartNew(() =>
            {
                sut.GetOrAdd(
                    TestKey,
                    () =>
                    {
                        Interlocked.Increment(ref times);
                        return new DateTime(2001, 01, 01);
                    });
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                sut.GetOrAdd(
                    TestKey,
                    () =>
                    {
                        Interlocked.Increment(ref times);
                        return new DateTime(2001, 01, 01);
                    });
            });

            Task.WaitAll(t1, t2);

            Assert.AreEqual(1, times);
        }

        [Test]
        public void GetNullKeyThrowsException()
        {
            var sut = new CachingService();

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Get<object>(null));
        }

        [Test]
        public void GetOrAddAndThenGetObjectReturnsCorrectType()
        {
            var sut = new CachingService();
            Func<ComplexTestObject> fetch = () => new ComplexTestObject();
            sut.GetOrAdd(TestKey, fetch);
            var actual = sut.Get<ComplexTestObject>(TestKey);
            Assert.IsNotNull(actual);
        }

        [Test]
        public void GetOrAddAndThenGetValueObjectReturnsCorrectType()
        {
            var sut = new CachingService();
            Func<int> fetch = () => 123;
            sut.GetOrAdd(TestKey, fetch);
            var actual = sut.Get<int>(TestKey);
            Assert.AreEqual(123, actual);
        }

        [Test]
        public void GetOrAddAndThenGetWrongtypeObjectReturnsNull()
        {
            var sut = new CachingService();
            Func<ComplexTestObject> fetch = () => new ComplexTestObject();
            sut.GetOrAdd(TestKey, fetch);
            var actual = sut.Get<Exception>(TestKey);
            Assert.IsNull(actual);
        }

        [Test]
        public void GetOrAddWillAddOnFirstCall()
        {
            var times = 0;
            var sut = new CachingService();

            var expected = sut.GetOrAdd(
                TestKey,
                () =>
                {
                    times++;
                    return new DateTime(2001, 01, 01);
                });

            Assert.AreEqual(2001, expected.Year);
            Assert.AreEqual(1, times);
        }

        [Test]
        public void GetOrAddWillAddOnFirstCallButReturnCachedOnSecond()
        {
            var times = 0;
            var sut = new CachingService();

            var expectedFirst = sut.GetOrAdd(
                TestKey,
                () =>
                {
                    times++;
                    return new DateTime(2001, 01, 01);
                });

            var expectedSecond = sut.GetOrAdd(
                TestKey,
                () =>
                {
                    times++;
                    return new DateTime(2002, 01, 01);
                });

            Assert.AreEqual(2001, expectedFirst.Year);
            Assert.AreEqual(2001, expectedSecond.Year);
            Assert.AreEqual(1, times);
        }

        [Test]
        public void GetOrAddWillNotAddIfExistingData()
        {
            var times = 0;
            var sut = new CachingService();
            var cached = new DateTime(1999, 01, 01);
            sut.Add(TestKey, cached);

            var expected = sut.GetOrAdd(
                TestKey,
                () =>
                {
                    times++;
                    return new DateTime(2001, 01, 01);
                });
            Assert.AreEqual(1999, expected.Year);
            Assert.AreEqual(0, times);
        }

        [Test]
        public void GetOrAddWithOffsetWillAddAndReturnCached()
        {
            var sut = new CachingService();

            var expectedFirst = sut.GetOrAdd(
                TestKey,
                () => new DateTime(2001, 01, 01),
                DateTimeOffset.Now.AddSeconds(5));

            var expectedSecond = sut.Get<DateTime>(TestKey);

            Assert.AreEqual(2001, expectedFirst.Year);
            Assert.AreEqual(2001, expectedSecond.Year);
        }

        [Test]
        public void GetOrAddWithPolicyAndThenGetObjectReturnsCorrectType()
        {
            var sut = new CachingService();
            Func<ComplexTestObject> fetch = () => new ComplexTestObject();
            sut.GetOrAdd(TestKey, fetch, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddHours(1), Priority = CacheItemPriority.NotRemovable });
            var actual = sut.Get<ComplexTestObject>(TestKey);
            Assert.IsNotNull(actual);
        }

        [Test]
        public void GetOrAddWithPolicyAndThenGetValueObjectReturnsCorrectType()
        {
            var sut = new CachingService();
            Func<int> fetch = () => 123;
            sut.GetOrAdd(TestKey, fetch, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddHours(1), Priority = CacheItemPriority.NotRemovable });
            var actual = sut.Get<int>(TestKey);
            Assert.AreEqual(123, actual);
        }

        [Test]
        public void GetOrAddWithPolicyWillAddOnFirstCallButReturnCachedOnSecond()
        {
            var times = 0;
            var sut = new CachingService();

            var expectedFirst = sut.GetOrAdd(
                TestKey,
                () =>
                {
                    times++;
                    return new DateTime(2001, 01, 01);
                },
                new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddHours(1), Priority = CacheItemPriority.NotRemovable });

            var expectedSecond = sut.GetOrAdd(
                TestKey,
                () =>
            {
                times++;
                return new DateTime(2002, 01, 01);
            },
            new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddHours(1), Priority = CacheItemPriority.NotRemovable });

            Assert.AreEqual(2001, expectedFirst.Year);
            Assert.AreEqual(2001, expectedSecond.Year);
            Assert.AreEqual(1, times);
        }

        [Test]
        public void GetWithClassTypeParamReturnsType()
        {
            var sut = new CachingService();
            var cached = new EventArgs();
            sut.Add(TestKey, cached);
            Assert.AreEqual(cached, sut.Get<EventArgs>(TestKey));
        }

        [Test]
        public void GetWithIntRetunsDefualtIfNotCached()
        {
            var sut = new CachingService();
            Assert.AreEqual(default(int), sut.Get<int>(TestKey));
        }

        [Test]
        public void GetWithNullableIntRetunsCachedNonNullableInt()
        {
            var sut = new CachingService();
            const int Expected = 123;
            sut.Add(TestKey, Expected);
            Assert.AreEqual(Expected, sut.Get<int?>(TestKey));
        }

        [Test]
        public void GetWithNullableStructTypeParamReturnsType()
        {
            var sut = new CachingService();
            var cached = new DateTime();
            sut.Add(TestKey, cached);
            Assert.AreEqual(cached, sut.Get<DateTime?>(TestKey));
        }

        [Test]
        public void GetWithStructTypeParamReturnsType()
        {
            var sut = new CachingService();
            var cached = new DateTime(2000, 1, 1);
            sut.Add(TestKey, cached);
            Assert.AreEqual(cached, sut.Get<DateTime>(TestKey));
        }

        [Test]
        public void GetWithValueTypeParamReturnsType()
        {
            var sut = new CachingService();
            const int Cached = 3;
            sut.Add(TestKey, Cached);
            Assert.AreEqual(3, sut.Get<int>(TestKey));
        }

        [Test]
        public void GetWithWrongClassTypeParamReturnsNull()
        {
            var sut = new CachingService();
            var cached = new EventArgs();
            sut.Add(TestKey, cached);
            Assert.IsNull(sut.Get<ArgumentNullException>(TestKey));
        }

        [Test]
        public void GetWithWrongStructTypeParamReturnsNull()
        {
            var sut = new CachingService();
            var cached = new DateTime();
            sut.Add(TestKey, cached);
            Assert.AreEqual(new TimeSpan(), sut.Get<TimeSpan>(TestKey));
        }

        [Test]
        public void RemovedItemCannotBeRetrievedFromCache()
        {
            var sut = new CachingService();
            sut.Add(TestKey, new object());
            Assert.NotNull(sut.Get<object>(TestKey));
            sut.Remove(TestKey);
            Assert.Null(sut.Get<object>(TestKey));
        }

        private class ComplexTestObject
        {
            public const string SomeMessage = "testing123";
            public readonly IList<object> SomeItems = new object[] { 1, 2, 3, "testing123" };
        }
    }
}