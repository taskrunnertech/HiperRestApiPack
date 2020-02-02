using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiperRestApiPackSample.Models
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext()
        { }

        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Product> Products { get; set; }

    }
}
