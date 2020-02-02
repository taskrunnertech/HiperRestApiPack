using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace HiperRestApiPack.EF
{
    public class FilteredEfQuery : IFilteredQuery
    {
        public async Task<object> FirstOrDefault<TSource>(IQueryable<TSource> query, PagedRequest request)
        {
            if (!string.IsNullOrEmpty(request.Select))
            {
                var selectedFieldQuery = SelectDynamic(query, request.Select.Split(","));
                return selectedFieldQuery.GetEnumerator().Current;
            }
            else
            {
               return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<Page> ToPageList<TSource>(IQueryable<TSource> query, PagedRequest request)
        {
            var tempQuery = Order(query, request)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);
            if (!string.IsNullOrEmpty(request.Select))
            {
                var selectedFieldQuery = SelectDynamic(tempQuery, request.Select.Split(","));
                return new Page(selectedFieldQuery, request.Page, request.PageSize, await query.CountAsync());
            }
            else
            {
                var result = await tempQuery.ToListAsync();
                return new Page(result, request.Page, request.PageSize, await query.CountAsync());
            }
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
            query = query.OrderByDynamic(request.OrderBy, request.Order == OrderType.Asc);
            return query;
        }

        public IQueryable SelectDynamic(IQueryable source, IEnumerable<string> fieldNames)
        {
            Dictionary<string, PropertyInfo> sourceProperties = fieldNames.ToDictionary(name => name, name => source.ElementType.GetProperty(name));
            Type dynamicType = LinqRuntimeTypeBuilder.GetDynamicType(sourceProperties.Values);

            ParameterExpression sourceItem = Expression.Parameter(source.ElementType, "t");
            IEnumerable<MemberBinding> bindings = dynamicType.GetFields().Select(p => Expression.Bind(p, Expression.Property(sourceItem, sourceProperties[p.Name]))).OfType<MemberBinding>();

            Expression selector = Expression.Lambda(Expression.MemberInit(
                Expression.New(dynamicType.GetConstructor(Type.EmptyTypes)), bindings), sourceItem);

            return source.Provider.CreateQuery(Expression.Call(typeof(Queryable), "Select", new Type[] { source.ElementType, dynamicType },
                         Expression.Constant(source), selector));
        }
    }
}
