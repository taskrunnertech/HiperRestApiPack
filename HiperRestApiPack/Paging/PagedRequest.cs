using System;
using System.Runtime.Serialization;

namespace HiperRestApiPack
{
    public enum OrderType
    {
        /// <summary>
        /// Ascending sort
        /// </summary>
        Asc,

        /// <summary>
        /// Descending sort
        /// </summary>
        Desc
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
        /// Default is 20
        /// </summary>
        public int PageSize { get; set; } = 10;

        public OrderType Order { get; set; } = OrderType.Asc;

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
