using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiperRestApiPack.Mongo
{
    public static class ApiPackExtensions
    {
        public static IServiceCollection AddHiperApiPack(this IServiceCollection services)
        {
            services.AddTransient<IFilteredQuery, FilteredMongoQuery>();
            return services;
        }
    }
}
