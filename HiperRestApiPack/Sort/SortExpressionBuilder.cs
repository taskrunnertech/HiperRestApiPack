using System;
using System.Linq.Expressions;

namespace HiperRestApiPack
{
    public class SortExpressionBuilder<T>

    {
        public string SortFieldName { get; set; }

        public Expression<Func<T, object>> SortExpression
        {
            get
            {
                if (string.IsNullOrEmpty(SortFieldName))
                {
                    return null;
                }
                else
                {
                    ParameterExpression parameter = Expression.Parameter(typeof(T));
                    MemberExpression property = Expression.Property(parameter, SortFieldName);
                    Type funcType = typeof(Func<,>).MakeGenericType(typeof(T), typeof(object));

                    LambdaExpression lambda;
                    lambda = Expression.Lambda(funcType, Expression.Convert(property, typeof(Object)), parameter);
                   
                    return (Expression<Func<T, object>>)lambda;
                }
            }
        }
    }
}
