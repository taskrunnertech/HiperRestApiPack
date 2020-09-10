using HiperRestApiPack.EF;
using Microsoft.Extensions.DependencyInjection;

namespace HiperRestApiPack.Mongo
{
    public static class ApiPackExtensions
    {
        public static IServiceCollection AddHiperApiPack(this IServiceCollection services)
        {
            services.AddTransient<IFilteredQuery, FilteredEfQuery>();
            return services;
        }
    }
}
