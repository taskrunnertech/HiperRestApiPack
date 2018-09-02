using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiperRestApiPack.Mongo
{

    public interface IFilteredMongoQuery<TResult> where TResult : class
    {
        Task<List<TResult>> ToListAsync();

        Task<TResult> FirstAsync();

        Task<TResult> FirstOrDefaultAsync();

        Task<IPage<TResult>> ToPageAsync(PagedRequest request);

    }

    public class FilteredMongoQuery<TResult> : IFilteredMongoQuery<TResult> where TResult : class
    {

        private IMongoQueryable<TResult> Source;
        private string _fields;
        public FilteredMongoQuery(IMongoQueryable<TResult> source, string fields)
        {
            Source = source;
            _fields = fields;
        }
        public Task<TResult> FirstAsync()
        {
            return CreateMongoSelect().FirstAsync();
        }


        public Task<TResult> FirstOrDefaultAsync()
        {
            return CreateMongoSelect().FirstOrDefaultAsync();
        }



        public Task<List<TResult>> ToListAsync()
        {
            return CreateMongoSelect().ToListAsync();
        }

        public async Task<IPage<TResult>> ToPageAsync(PagedRequest request)
        {
            if (request == null)
            {
                throw new PagingException($"You need to initialize a paging request before paging on a list. The parameter request should be initialized.");
            }
            var result = CreateMongoSelect();

            IOrderedMongoQueryable<TResult> orderd;
            SortExpressionBuilder<TResult> sortExpression = null;
            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                sortExpression = new SortExpressionBuilder<TResult>
                {
                    SortFieldName = request.OrderBy
                };
            }
            if (!request.Page.HasValue)
            {
                if (!string.IsNullOrEmpty(request.OrderBy))
                {

                    if (request.Order == OrderType.Asc)
                    {
                        orderd = result.OrderBy(sortExpression.SortExpression);
                    }
                    else
                    {
                        orderd = result.OrderByDescending(sortExpression.SortExpression);
                    }
                    return new Page<TResult>(await orderd.ToListAsync(), 0, 0, 0);
                }
                return new Page<TResult>(await result.ToListAsync(), 0, 0, 0);
            }

            if (string.IsNullOrEmpty(request.OrderBy))
            {
                throw new PagingException($"In order to use paging extensions you need to supply an OrderBy parameter.");
            }
            if (request.Order == OrderType.Asc)
            {
                orderd = result.OrderBy(sortExpression.SortExpression);
            }
            else
            {
                orderd = result.OrderByDescending(sortExpression.SortExpression);
            }

            int skip = (request.Page.Value - 1) * request.PageSize;
            int take = request.PageSize;
            int totalItemCount = await Source.CountAsync();



            return new Page<TResult>(await orderd.Skip(skip).Take(take).ToListAsync(), request.Page.Value, request.PageSize, totalItemCount);

        }


        public IMongoQueryable<TResult> CreateMongoSelect()
        {
            var a = Source
                .Select(
                new DynamicMongoSelectBuilder<TResult>().CreateNewStatement(_fields));
            return a;
        }
    }

    public static class QuearyExtensions
    {

        public static IFilteredMongoQuery<TResult> Select<TResult>(this IMongoQueryable<TResult> source, string fields)
            where TResult : class
        {
            return new FilteredMongoQuery<TResult>(source, fields);
        }
    }


}
