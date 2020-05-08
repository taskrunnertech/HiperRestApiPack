using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
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

        public async Task<ApiResponse> ToApiResult<TSource>(IQueryable<TSource> query, PagedRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Sum))
            {
                return CreatedResult(query.Sum(request.Sum));
            }
            var page = await ToPageList(query, request);
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


        public async Task<Page> ToPageList<TSource>(IQueryable<TSource> query, PagedRequest request)
        {
            var tempQuery = Order(query, request)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);

            var selectedFieldQuery = SelectDynamic(tempQuery, request.Select);
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

        public IQueryable SelectDynamic(IQueryable source, string select = null)
        {
            if (!string.IsNullOrWhiteSpace(select))
            {
                return source.Select(select);
            }

            PropertyInfo[] props = source.ElementType.GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).ToArray();
            int n = props.Length;
            Dictionary<string, PropertyInfo> sourceProperties = new Dictionary<string, PropertyInfo>();
            for (int i = 0; i < n; i++)
            {
                var attr = props[i].GetCustomAttribute<IgnoreFieldAttribute>();
                if (attr == null)
                {
                    sourceProperties[props[i].Name] = props[i];
                }
            }
            Type dynamicType = LinqRuntimeTypeBuilder.GetDynamicType(sourceProperties.Values);

            ParameterExpression sourceItem = Expression.Parameter(source.ElementType, "t");
            IEnumerable<MemberBinding> bindings = dynamicType.GetFields().Select(p => Expression.Bind(p, Expression.Property(sourceItem, sourceProperties[p.Name]))).OfType<MemberBinding>();

            Expression selector = Expression.Lambda(Expression.MemberInit(
                Expression.New(dynamicType.GetConstructor(Type.EmptyTypes)), bindings), sourceItem);

            return source.Provider.CreateQuery(Expression.Call(typeof(Queryable), "Select", new Type[] { source.ElementType, dynamicType },
                source.Expression, Expression.Quote(selector)));
        }


    }
}
