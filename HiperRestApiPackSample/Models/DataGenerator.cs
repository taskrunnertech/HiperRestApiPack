using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace HiperRestApiPackSample.Models
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SampleDbContext(
                serviceProvider.GetService<DbContextOptions<SampleDbContext>>()))
            {
                // Look for any board games.
                if (context.Products.Any())
                {
                    return;   // Data was already seeded
                }

                context.Products.AddRange(
                    new Product
                    {
                        Id = 1,
                        Name = "Candy Land 1",
                        Price = 2.2m,

                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Candy Land 2",
                        Price = 8.2m,

                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Candy Land 3",
                        Price = 7.2m,

                    },
                    new Product
                    {
                        Id = 4,
                        Name = "Candy Land 4",
                        Price = 2.2m,

                    },
                    new Product
                    {
                        Id = 5,
                        Name = "Candy Land 5",
                        Price = 4.2m,

                    },
                    new Product
                    {
                        Id = 6,
                        Name = "Candy Land 6",
                        Price = 5.2m,

                    },
                    new Product
                    {
                        Id = 7,
                        Name = "Candy Land 7",
                        Price = 6.2m,

                    }
                   );

                context.SaveChanges();
            }
        }
    }
}
