using System;

namespace HiperRestApiPack
{
    public enum OrderType : int
    {
        /// <summary>
        /// Ascending sort
        /// </summary>
        Asc = 0,

        /// <summary>
        /// Descending sort
        /// </summary>
        Desc = 1,
    }

    public class PagedRequest
    {
        /// <summary>
        /// dynamic select new(Id as id, name)
        /// </summary>
        public string Select { get; set; }

        /// <summary>
        /// Sum query
        /// </summary>
        public string Sum { get; set; }

        /// <summary>
        /// Page to fetch
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Size of page to fetch
        /// Default is 10
        /// </summary>
        public int PageSize { get; set; } = 10;

        public OrderType? Order { get; set; }

        /// <summary>
        /// Field name to sort   
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Full text search
        /// </summary>
        public string Search { get; set; }
    }

    public class PagingException : Exception
    {
        public PagingException(string message)
            : base(message)
        {
        }
    }
}
