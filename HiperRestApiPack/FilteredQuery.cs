using System.Collections.Generic;
using System.Linq;

namespace HiperRestApiPack
{
    public interface IFilteredQuery<TResult> where TResult : class
    {
        List<TResult> ToList();

        TResult First();

        TResult FirstOrDefault();

        IPage<TResult> ToPage(PagedRequest request);

    }

    public class FilteredQuery<TResult> : IFilteredQuery<TResult> where TResult : class
    {

        private IQueryable<TResult> _source;
        private string _fields;
        public FilteredQuery(IQueryable<TResult> source, string fields)
        {
            _source = source;
            _fields = fields;
        }
        public TResult First()
        {
            return CreateSelect().First();
        }
       
        public TResult FirstOrDefault()
        {
            return CreateSelect().FirstOrDefault();
        }

        public List<TResult> ToList()
        {
            return CreateSelect().ToList();
        }
     
        public IPage<TResult> ToPage(PagedRequest request)
        {
            if (request == null)
            {
                throw new PagingException($"You need to initialize a paging request before paging on a list. The parameter request should be initialized.");
            }
           var result = CreateSelect();
            IOrderedQueryable<TResult> orderd;

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
                return new Page<TResult>(orderd.ToList(), 0, 0, 0);
                }
                return new Page<TResult>(result.ToList(), 0, 0, 0);
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
            int totalItemCount = _source.Count();

            return new Page<TResult>(orderd.Skip(skip).Take(take).ToList(), request.Page.Value, request.PageSize, totalItemCount);

        }

       
        public IQueryable<TResult> CreateSelect()
        {
            return _source
                .Select(
                new DynamicSelectBuilder<TResult>().CreateNewStatement(_fields)).AsQueryable();
        }


    }
}
