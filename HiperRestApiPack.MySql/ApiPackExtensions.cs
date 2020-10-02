using HiperRestApiPack.EF;
using Microsoft.Extensions.DependencyInjection;

namespace HiperRestApiPack
{
    public static class ApiPackExtensions
    {
        public static IServiceCollection AddHiperApiPackMysql(this IServiceCollection services)
        {
            services.AddTransient<IFilteredQuery, FilteredEfQuery>();
            return services;
        }
    }
}
