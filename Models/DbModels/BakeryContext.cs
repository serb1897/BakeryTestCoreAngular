using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Bakery.Models.DbModels
{
    public class BakeryContext : DbContext
    {
        public BakeryContext(DbContextOptions<BakeryContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .Build();

            optionsBuilder.UseSqlite(config.GetConnectionString("DefaultConnection"));
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                    new Category { Id = 1, Name = "Хліб" },
                    new Category { Id = 2, Name = "Тістечка" },
                    new Category { Id = 3, Name = "Випічка" }
            );

            modelBuilder.Entity<OrderStatus>().HasData(
                    new OrderStatus { Id = 1, Name = "Створений" },
                    new OrderStatus { Id = 2, Name = "Прийнятий" },
                    new OrderStatus { Id = 3, Name = "Готується" },
                    new OrderStatus { Id = 4, Name = "Пакується" },
                    new OrderStatus { Id = 5, Name = "Доставляється" },
                    new OrderStatus { Id = 6, Name = "Виконаний" }
            );

            modelBuilder.Entity<Product>().HasData(
                    new Product { 
                        Id = 1,
                        Name = "Батон",
                        CategoryId = 1,
                        CreationDate = DateTime.Now,
                        Description = "Хрусткий та свіжий",
                        IsAvailable = true,
                        Price = 30.0M
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Чізкейк",
                        CategoryId = 2,
                        CreationDate = DateTime.Now,
                        Description = "Солодкий та м'який",
                        IsAvailable = true,
                        Price = 75.0M
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Печіво",
                        CategoryId = 3,
                        CreationDate = DateTime.Now,
                        Description = "Домашнє із родзинками",
                        IsAvailable = true,
                        Price = 45.0M
                    }
            );
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
