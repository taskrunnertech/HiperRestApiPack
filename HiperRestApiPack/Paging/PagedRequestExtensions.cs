using System;
using System.Collections.Generic;
using System.Globalization;

namespace HiperRestApiPack
{
    public static class PagedRequestExtensions
    {
        public static string PagingQuery(this PagedRequest request)
        {
            var parameters = new List<string>();
            if(!string.IsNullOrEmpty(request.Select))
            {
                parameters.Add(string.Format("select={0}", Uri.EscapeDataString(request.Select)));
            }
            parameters.Add(string.Format(CultureInfo.InvariantCulture, "page={0}", request.Page));
            parameters.Add(string.Format(CultureInfo.InvariantCulture, "pageSize={0}", request.PageSize));
            if(request.Order.HasValue)
            {
                parameters.Add(string.Format("order={0}", request.Order.Value));
            }
            if(!string.IsNullOrEmpty(request.OrderBy))
            {
                parameters.Add(string.Format("orderBy={0}", Uri.EscapeDataString(request.OrderBy)));
            }
            if(!string.IsNullOrEmpty(request.Search))
            {
                parameters.Add(string.Format("search={0}", Uri.EscapeDataString(request.Search)));
            }

            return string.Join('&', parameters);
        }
    }
}
