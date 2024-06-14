using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace EcommerceApp.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    Name = "Laptop"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Điện thoại"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Đồng hồ"
                });

            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    Name = "Laptop mini Samsung N150",
                    CategoryId = 1,
                    Price = 2690000,
                    SalePrice = 2090000
                },
                new Product()
                {
                    Id = 2,
                    Name = "Laptop Dell Latitude. i5 M520 ram 8G 500Gb",
                    CategoryId = 1,
                    Price = 2690000,
                    SalePrice = 2090000
                },
                new Product()
                {
                    Id = 3,
                    Name = "Laptop cũ dell latitude e7280 i7 7600u ram 8gb",
                    CategoryId = 1,
                    Price = 2690000,
                    SalePrice = 2090000
                },
                new Product()
                {
                    Id = 4,
                    Name = "Laptop Dell bền bỉ i5 2.4Ghz",
                    CategoryId = 1,
                    Price = 2690000,
                    SalePrice = 2090000
                },
                new Product()
                {
                    Id = 5,
                    Name = "TOSHIBA RAM 8G. i5 4300M upto 3.3Ghz 2.6GHZ",
                    CategoryId = 1,
                    Price = 2690000,
                    SalePrice = 2090000
                },
                new Product()
                        {
                    Id = 6,
                    Name = "Điện thoại Xiaomi Redmi 9A 2GB-32GB",
                            CategoryId = 2,
                            Price = 2690000,
                            SalePrice = 2090000
                        },
                new Product()
                        {
                    Id = 7,
                    Name = "Điện thoại Oppo F11 Ram 8GB/ 256GB",
                            CategoryId = 2,
                            Price = 2690000,
                            SalePrice = 2090000
                        },
                new Product()
                        {
                    Id = 8,
                    Name = "Xiaomi Redmi 13C Ram 8GB/ 256GB",
                            CategoryId = 2,
                            Price = 2690000,
                            SalePrice = 2090000
                        },
                new Product()
                        {
                    Id = 9,
                    Name = "Realme C33 Ram 8GB/ 256GB",
                            CategoryId = 2,
                            Price = 2690000,
                            SalePrice = 2090000
                        },
                new Product()
                        {
                    Id = 10,
                    Name = "Điện thoại Oppo Reno 4z 5G Ram 12GB/256GB",
                            CategoryId = 2,
                            Price = 2690000,
                            SalePrice = 2090000
                        },
                new Product()
                        {
                    Id = 11,
                    Name = "ĐỒNG HỒ TRẺ EM COOBOS",
                            CategoryId = 3,
                            Price = 2690000,
                            SalePrice = 2090000
                        },
                new Product()
                {
                    Id = 12,
                    Name = "Đồng hồ Nam ROMATIC DOMINIC",
                    CategoryId = 3,
                    Price = 2690000,
                    SalePrice = 2090000
                },
                new Product()
                {
                    Id = 13,
                    Name = "Đồng Hồ Nam Thời Trang Doanh Nhân Dây Da PU",
                    CategoryId = 3,
                    Price = 2690000,
                    SalePrice = 2090000
                },
                new Product()
                {
                    Id = 14,
                    Name = "Đồng hồ Nữ VENUS Hàn Quốc",
                    CategoryId = 3,
                    Price = 2690000,
                    SalePrice = 2090000
                },
                new Product()
                {
                    Id = 15,
                    Name = "Đồng hồ đôi NEOS N-30932M",
                    CategoryId = 3,
                    Price = 2690000,
                    SalePrice = 2090000
                });

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}