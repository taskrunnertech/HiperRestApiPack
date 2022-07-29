using Xunit;

namespace HiperRestApiPack.Tests
{
    public class PagedRequestToQueryStringTests
    {
        [Fact]
        public void Test_Empty_PagedRequest()
        {
            var pr = new PagedRequest();
            Assert.Equal("page=1&pageSize=10", pr.PagingQuery());
        }

        [Fact]
        public void Test_Full_PagedRequest()
        {
            var pr = new PagedRequest
            {
                Select = "(Id, Two, Three)",
                Sum = "Unused",
                Page = 10,
                PageSize = 20,
                Order = OrderType.Desc,
                OrderBy = "Order",
                Search = "Search"
            };
            Assert.Equal("select=%28Id%2C%20Two%2C%20Three%29&page=10&pageSize=20&order=Desc&orderBy=Order&search=Search", pr.PagingQuery());
        }

        [Fact]
        public void Test_WithoutOrder_PagedRequest()
        {
            var pr = new PagedRequest
            {
                Select = "string with space",
                PageSize = 100
            };
            Assert.Equal("select=string%20with%20space&page=1&pageSize=100", pr.PagingQuery());
        }
    }
}
