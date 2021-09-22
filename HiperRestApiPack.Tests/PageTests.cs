using NUnit.Framework;
using System;

namespace HiperRestApiPack.Tests
{
    public class PageTests
    {
        
        [Test]
        public void TestIndex()
        {
            var page = new Page(null, 1, 10, 100);
            Assert.AreEqual(100, page.TotalCount);
            Assert.AreEqual(10, page.TotalPages);
            Assert.True(page.HasNextPage);
            Assert.False(page.HasPreviousPage);

            page = new Page(null, 10, 10, 100);
            Assert.AreEqual(100, page.TotalCount);
            Assert.AreEqual(10, page.TotalPages);
            Assert.False(page.HasNextPage);
            Assert.True(page.HasPreviousPage);

            page = new Page(null, 5, 10, 100);
            Assert.AreEqual(100, page.TotalCount);
            Assert.AreEqual(10, page.TotalPages);
            Assert.True(page.HasNextPage);
            Assert.True(page.HasPreviousPage);

            page = new Page(null, 1, 1, 1);
            Assert.AreEqual(1, page.TotalCount);
            Assert.AreEqual(1, page.TotalPages);
            Assert.False(page.HasNextPage);
            Assert.False(page.HasPreviousPage);

            page = new Page(null, 1, 100, 2);
            Assert.AreEqual(2, page.TotalCount);
            Assert.AreEqual(1, page.TotalPages);
            Assert.False(page.HasNextPage);
            Assert.False(page.HasPreviousPage);
        }

        [Test]
        public void TestConstructorChecks() {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                new Page(null, 0, 10, 100);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                new Page(null, 1, 10, -1);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                new Page(null, 11, 10, 100);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                new Page(null, 3, 1, 2);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                new Page(null, 1, -5, 50);
            });
        }

    }
}