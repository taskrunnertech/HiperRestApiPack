using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace HiperRestApiPack.EF
{
    public class FilteredEfQuery : IFilteredQuery
    {
        public async Task<ApiResponse> FirstOrDefault<TSource>(IQueryable<TSource> query, PagedRequest request)
        {
            var selectedFieldQuery = SelectDynamic(query, request.Select);
            var n = selectedFieldQuery.GetEnumerator();
            n.MoveNext();
            var result = await Task.FromResult(n.Current);
            return CreatedResult(result);
        }

        public async Task<ApiResponse> ToApiResult<TSource>(IQueryable<TSource> query, PagedRequest request, string filterSelect = null)
        {
            if (!string.IsNullOrWhiteSpace(request.Sum))
            {
                return CreatedResult(query.Sum(request.Sum));
            }
            var page = await ToPageList(query, request, filterSelect);
            return CreatedResult(page);
        }

        public async Task<ApiResponse> ToApiResult<TSource, TResult>(IQueryable<TSource> query, PagedRequest request, Func<TSource, TResult> mapper) where TResult : class, new()
        {
            if (!string.IsNullOrWhiteSpace(request.Sum))
            {
                return CreatedResult(query.Sum(request.Sum));
            }
            var page = await ToPageList(query, request, mapper);
            return CreatedResult(page);
        }


        public async Task<Page> ToPageList<TSource>(IQueryable<TSource> query, PagedRequest request, string filterSelect = null)
        {
            if (query == null)
            {
                return new Page(Enumerable.Empty<TSource>(), 1, request.PageSize, 0);
            }

            IQueryable tempQuery = Order(query, request)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);

            var selectedFieldQuery = SelectDynamic(tempQuery, request.Select, filterSelect);

            var result = SomeDynamic(selectedFieldQuery, request);
            var page = new Page(result, request.Page, request.PageSize, await query.CountAsync());
            return page;
        }

        private ApiResponse CreatedResult(object data)
        {
            return new ApiResponse
            {
                Data = data,
                Success = true
            };
        }

        public async Task<Page> ToPageList<TSource, TResult>(IQueryable<TSource> query, PagedRequest request, Func<TSource, TResult> mapper) where TResult : class, new()
        {
            if (query == null)
            {
                return new Page(Enumerable.Empty<TResult>(), 1, request.PageSize, 0);
            }

            var history = await Order(query, request)
              .Skip((request.Page - 1) * request.PageSize)
              .Take(request.PageSize).ToListAsync();
            var result = history.Select(x => mapper(x)).ToList();
            return new Page(result, request.Page, request.PageSize, await query.CountAsync());
        }

        public IQueryable<TSource> Order<TSource>(IQueryable<TSource> query, PagedRequest request)
        {
            if (string.IsNullOrEmpty(request.OrderBy))
            {
                return query;
            }
            string orderDircetion = request.Order == OrderType.Desc ? "descending" : string.Empty;
            return query.OrderBy($"{request.OrderBy} {orderDircetion}");
        }


        public object SomeDynamic(IQueryable source, PagedRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Sum))
            {
                return source.Sum(request.Sum);
            }
            return source;
        }

        public IQueryable SelectDynamic(IQueryable source, string select = null, string filterSelect = null)
        {
            if (!string.IsNullOrEmpty(filterSelect))
            {
                source = source.Select(filterSelect);
            }

            if (!string.IsNullOrWhiteSpace(select))
            {
                return source.Select(select);
            }
            
            return source;
        }


    }
}
