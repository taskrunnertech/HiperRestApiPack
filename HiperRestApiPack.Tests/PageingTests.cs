using Xunit;

using System;

namespace HiperRestApiPack.Tests
{
    public class PageingTests
    {

        [Fact]
        public void TestIndex_EmptyList_TotalPageZero()
        {
            var page = new Page(null, 1, 10, 0);
            Assert.Equal(0, page.TotalPages);
        }

        [Fact]
        public void TestIndex_TotalPageCalculation_TotalPage()
        {
            var page = new Page(null, 1, 10, 100);
            Assert.Equal(10, page.TotalPages);

            page = new Page(null, 10, 10, 100);
            Assert.Equal(10, page.TotalPages);

            page = new Page(null, 5, 10, 100);
            Assert.Equal(10, page.TotalPages);

            page = new Page(null, 1, 1, 1);
            Assert.Equal(1, page.TotalPages);
          
            page = new Page(null, 1, 100, 2);
            Assert.Equal(1, page.TotalPages);
           
        }

        [Fact]
        public void TestIndex_TotalCountCalculation_TotalCount()
        {
            var page = new Page(null, 1, 10, 100);
            Assert.Equal(100, page.TotalCount);

            page = new Page(null, 10, 10, 100);
            Assert.Equal(100, page.TotalCount);

            page = new Page(null, 5, 10, 100);
            Assert.Equal(100, page.TotalCount);
           
            page = new Page(null, 1, 1, 1);
            Assert.Equal(1, page.TotalCount);

            page = new Page(null, 1, 100, 2);
            Assert.Equal(2, page.TotalCount);
        }

        [Fact]
        public void TestIndex_HasPreviousPage_TrueOrFalse()
        {
            var page = new Page(null, 1, 10, 100);
            Assert.False(page.HasPreviousPage);

            page = new Page(null, 10, 10, 100);
            Assert.True(page.HasPreviousPage);

            page = new Page(null, 5, 10, 100);
            Assert.True(page.HasPreviousPage);

            page = new Page(null, 1, 1, 1);
            Assert.False(page.HasPreviousPage);

            page = new Page(null, 1, 100, 2);
            Assert.False(page.HasPreviousPage);
        }

        [Fact]
        public void TestIndex_HasNextPage_TrueOrFalse()
        {
            var page = new Page(null, 1, 10, 100);
            Assert.True(page.HasNextPage);

            page = new Page(null, 10, 10, 100);
            Assert.False(page.HasNextPage);

            page = new Page(null, 5, 10, 100);
            Assert.True(page.HasNextPage);

            page = new Page(null, 1, 1, 1);
            Assert.False(page.HasNextPage);

            page = new Page(null, 1, 100, 2);
            Assert.False(page.HasNextPage);
        }

        [Fact]
        public void TestConstructor_InitialWithWrongValues_Exception() {
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