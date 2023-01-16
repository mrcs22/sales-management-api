using Microsoft.EntityFrameworkCore;
using tech_test_payment_api.Models;

namespace tech_test_payment_api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Seller> Sellers {get; set;}
        public DbSet<Product> Products {get; set;}
        public DbSet<Sale> Sales {get; set;}

        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }

    }
}