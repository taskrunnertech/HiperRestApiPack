using System.Linq;

namespace HiperRestApiPack.FilterExtensions
{

    public static class QuearyExtensions
    {
        //public static IFilteredQuery<TResult> Select<TResult>(this IMongoQueryable<TResult> source, string fields)
        //    where TResult : class
        //{
        //    return new AsyncFilteredExpressionQuery<TResult>(source, fields);
        //}

        public static IFilteredQuery<TResult> Select<TResult>(this IQueryable<TResult> source, string fields)
            where TResult : class
        {
            return new FilteredQuery<TResult>(source, fields);
        }
    }
}
