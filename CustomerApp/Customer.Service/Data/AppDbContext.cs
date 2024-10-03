using Customer.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Customer.Service.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Elektronika" },
                new Category { CategoryId = 2, Name = "Kitoblar" },
                new Category { CategoryId = 3, Name = "Kiyimlar" },
                new Category { CategoryId = 4, Name = "O'yinchoqlar" },
                new Category { CategoryId = 6, Name = "Sport buyumlari" },
                new Category { CategoryId = 7, Name = "Mebel" },
                new Category { CategoryId = 8, Name = "Oshxona buyumlari" }
            );
        }
    }
}
