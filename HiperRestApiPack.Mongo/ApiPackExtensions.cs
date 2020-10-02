using HiperRestApiPack.Mongo;
using Microsoft.Extensions.DependencyInjection;

namespace HiperRestApiPack
{
    public static class ApiPackExtensions
    {
        public static IServiceCollection AddHiperApiPackMongo(this IServiceCollection services)
        {
            services.AddTransient<IFilteredQuery, FilteredMongoQuery>();
            return services;
        }
    }
}
