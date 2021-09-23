using System;
using System.Collections.Generic;
using System.Linq;

namespace HiperRestApiPack
{
    public class ApiResponse
    {
        public object Data { get; set; }

        public bool Success { get; set; } = true;

        public int? ErrorCode { get; set; }

        public string Message { get; set; }
    }

    public interface IPage
    {
        object Items { get; }

        int Index { get; }

        int Size { get; }

        int TotalCount { get; }

        int TotalPages { get; }

        bool HasPreviousPage { get; }

        bool HasNextPage { get; }
    }

    public class Page : IPage
    {
        public Page(object items, int pageIndex, int pageSize, int totalItemCount)
        {
            if(pageSize <= 0) {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }
            if(totalItemCount < 0) {
                throw new ArgumentOutOfRangeException(nameof(totalItemCount));
            }

            Items = items;
            Index = pageIndex;
            Size = pageSize;
            TotalCount = totalItemCount;
            TotalPages = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            HasNextPage = pageIndex < TotalPages;
            HasPreviousPage = pageIndex > 1;

            if (pageIndex <= 0 || pageIndex > TotalPages) {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }
        }

        public object Items { get; }
        public int Index { get; }
        public int Size { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }

        public static IPage Empty => new Page(Enumerable.Empty<object>(), 0, 0, 0);
    }
}
