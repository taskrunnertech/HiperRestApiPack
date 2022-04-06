using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;


namespace HiperRestApiPack.Mongo
{

    public class FilteredMongoQuery : IFilteredQuery
    {

        public async Task<ApiResponse> FirstOrDefault<TSource>(IQueryable<TSource> query, PagedRequest request)
        {
            IMongoQueryable<TSource> q = query as IMongoQueryable<TSource>;
            var selectedFieldQuery = SelectDynamic(q, request.Select);

            var n = (selectedFieldQuery as IMongoQueryable).GetEnumerator();
            n.MoveNext();
            var result = await Task.FromResult(n.Current);
            return CreatedResult(result);
        }

        public async Task<ApiResponse> ToApiResult<TSource>(IQueryable<TSource> query, PagedRequest request, string filterSelect = null)
        {
            var page = await ToPageList(query, request, filterSelect);
            return CreatedResult(page);

        }
        public async Task<ApiResponse> ToApiResult<TSource, TResult>(IQueryable<TSource> query, PagedRequest request, Func<TSource, TResult> mapper) where TResult : class, new()
        {
            var page = await ToPageList(query, request, mapper);
            return CreatedResult(page);
        }

        public async Task<Page> ToPageList<TSource>(IQueryable<TSource> query, PagedRequest request, string filterSelect = null)
        {
            if (query == null)
            {
                return new Page(Enumerable.Empty<TSource>(), 1, request.PageSize, 0);
            }

            IMongoQueryable<TSource> q = query as IMongoQueryable<TSource>;

            var tempQuery =  Order(q, request) 
              .Skip((request.Page - 1) * request.PageSize)
              .Take(request.PageSize) as IMongoQueryable<TSource>;

            var selectedFieldQuery = SelectDynamic(tempQuery, request.Select, filterSelect);

            var result = SomeDynamic(selectedFieldQuery, request);
            var page = new Page(result, request.Page, request.PageSize, await q.CountAsync());
            return page;

        }

        public async Task<Page> ToPageList<TSource, TResult>(IQueryable<TSource> query, PagedRequest request, Func<TSource, TResult> mapper) where TResult : class, new()
        {
            if (query == null)
            {
                return new Page(Enumerable.Empty<TResult>(), 1, request.PageSize, 0);
            }

            IMongoQueryable<TSource> q = query as IMongoQueryable<TSource>;
            var history = await (Order(q, request) as IMongoQueryable<TSource>)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).ToListAsync();
            var result = history.Select(x => mapper(x)).ToList();
            return new Page(result, request.Page, request.PageSize, await q.CountAsync());
        }

        public IQueryable<TSource> Order<TSource>(IQueryable<TSource> query, PagedRequest request)
        {
            if (string.IsNullOrEmpty(request.OrderBy))
            {
                return query;
            }
            query = query.OrderBy(request.OrderBy, request.Order == OrderType.Asc);
            return query;
        }

        private ApiResponse CreatedResult(object data)
        {
            return new ApiResponse
            {
                Data = data,
                Success = true
            };
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
            string[] fieldNames = new string[0];
            if (!string.IsNullOrEmpty(filterSelect))
            {
                fieldNames = filterSelect.Split(",", StringSplitOptions.RemoveEmptyEntries);
            }

            if (!string.IsNullOrWhiteSpace(select))
            {
                fieldNames = select.Split(",", StringSplitOptions.RemoveEmptyEntries);
            }

           
            Dictionary<string, PropertyInfo> sourceProperties = fieldNames.ToDictionary(name => name, name => source.ElementType.GetProperty(name));
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
