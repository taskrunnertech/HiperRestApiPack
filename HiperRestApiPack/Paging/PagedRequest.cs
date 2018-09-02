using System;
using System.Runtime.Serialization;

namespace HiperRestApiPack
{
    public enum OrderType
    {
        /// <summary>
        /// Ascending sort
        /// </summary>
        [EnumMember(Value = "asc")]
        Asc,

        /// <summary>
        /// Descending sort
        /// </summary>
        [EnumMember(Value = "desc")]
        Desc
    }
    public class PagedRequest
    {
        /// <summary>
        /// Page to fetch
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Size of page to fetch
        /// Default is 20
        /// </summary>
        public int PageSize { get; set; } = 20;

        private OrderType? _order;

        /// <summary>
        /// Sort order type ascending or descending
        /// </summary>
        public OrderType? Order
        {
            get
            {
                if (!_order.HasValue && !String.IsNullOrWhiteSpace(OrderBy))
                {
                    throw new PagingException($"You need to supply Order (asc or desc) in order to order collection by {OrderBy}.");
                }

                if (_order.HasValue && String.IsNullOrWhiteSpace(OrderBy))
                {
                    throw new PagingException($"You need to supply OrderBy field in order to order collection {_order.Value.ToString().ToLowerInvariant()}.");
                }

                return _order;
            }
            set { _order = value; }
        }

        /// <summary>
        /// Field name to sort   
        /// </summary>
        public string OrderBy { get; set; }
    }

    public class PagingException : Exception
    {
        public PagingException(string message)
            : base(message)
        {
        }
    }
}
