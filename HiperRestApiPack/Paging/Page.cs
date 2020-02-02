using System.Collections.Generic;
using System.Linq;

namespace HiperRestApiPack
{
    public interface IPage
    {
        object Items { get; }

        int Index { get; }

        int Size { get; }

        int TotalCount { get; }

        int TotalPages { get; }

        bool HasPreviousPage { get; }

        bool HasNextPage { get; }
        decimal? Sum { get; }
    }

    public class Page : IPage
    {
        public Page(object items, int pageIndex, int pageSize, int totalItemCount)
        {
            this.Items = items;
            this.Index = pageIndex;
            this.Size = pageSize;
            this.TotalCount = totalItemCount;
            this.TotalPages = (totalItemCount / pageSize) + 1;
            this.HasNextPage = pageIndex < this.TotalPages;
            this.HasPreviousPage = pageIndex > 1;
        }

        public object Items { get; }
        public int Index { get; }
        public int Size { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }

        public static IPage Empty => new Page(Enumerable.Empty<object>(), 0, 0, 0);

        public decimal? Sum { get; }
    }
}
