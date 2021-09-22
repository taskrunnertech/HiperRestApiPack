using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiperRestApiPack
{
    public interface IFilteredQuery
    {
        Task<ApiResponse> FirstOrDefault<TSource>(IQueryable<TSource> query, PagedRequest request);

        Task<Page> ToPageList<TSource>(IQueryable<TSource> query, PagedRequest request, string filterSelect = null);

        Task<Page> ToPageList<TSource, TResult>(IQueryable<TSource> query, PagedRequest request,
            Func<TSource, TResult> mapper) where TResult : class, new();

        Task<ApiResponse> ToApiResult<TSource>(IQueryable<TSource> query, PagedRequest request, string filterSelect = null);

        Task<ApiResponse> ToApiResult<TSource, TResult>(IQueryable<TSource> query, PagedRequest request,
            Func<TSource, TResult> mapper) where TResult : class, new();

        IQueryable<TSource> Order<TSource>(IQueryable<TSource> query, PagedRequest request);
    }
}
