using CodinovaTask.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodinovaTask.DataEntities
{
    public class CodinovaContextEntities : DbContext
    {
        public CodinovaContextEntities()
        {

        }

        public CodinovaContextEntities(DbContextOptions<CodinovaContextEntities> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=example;Initial Catalog=Codinovadb;User Id=example; Password=example");
            }
        }

        public DbSet<UserDetails> Users { get; set; }
        public DbSet<ProductDetails> Products { get; set; }
        public DbSet<OrderDetails> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");
        }
    }
}
