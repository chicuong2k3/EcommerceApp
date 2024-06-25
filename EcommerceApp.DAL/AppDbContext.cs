using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Identity Configuration
            modelBuilder.Entity<AppUser>().ToTable("AppUsers");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Name = UserRoleConstant.Admin,
                    NormalizedName = UserRoleConstant.NormalizedAdmin
                },
                new IdentityRole()
                {
                    Name = UserRoleConstant.Customer,
                    NormalizedName = UserRoleConstant.NormalizedCustomer
                }
            );


            modelBuilder.Entity<Review>().HasOne(e => e.OrderLine)
                .WithMany(e => e.Reviews)
                .OnDelete(DeleteBehavior.NoAction);

            // Seeding data for Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Đồ Nữ" },
                new Category { Id = 2, Name = "Đồ Nam" },
                new Category { Id = 3, Name = "Áo Thun"},
                new Category { Id = 4, Name = "Áo Polo"}
            );


            var product1 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Women Airism UV Cut Mesh Hoodie",
                Description = "Light, cool, and comfortable. The perfect hoodie for outdoor activities.",
                ThumbUrl = "https://www.uniqlo.com/images/sg/uq/pc/images/445405/item/sg445405003.jpg",
                OriginalPrice = 29.90m,
                SalePrice = 25.5m
            };
                var product2 = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Women Ultra Light Down Vest",
                    Description = "With a durable water-repellent coating and water-repellent threads, it repels rain.",
                    ThumbUrl = "https://www.uniqlo.com/images/sg/uq/pc/images/429697/item/sg429697003.jpg",
                    OriginalPrice = 59.90m,
                    SalePrice = 50m
                };
                var product3 = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Men Dry Stretch Sweatpants",
                    Description = "Dries sweat quickly for a smooth, dry feel.",
                    ThumbUrl = "https://www.uniqlo.com/images/sg/uq/pc/images/438722/item/sg438722003.jpg",
                    OriginalPrice = 39.90m,
                    SalePrice = 36m
                };
                var product4 = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Men Extra Fine Cotton Broadcloth Long-Sleeve Shirt",
                    Description = "100% cotton broadcloth shirt for a dressy style.",
                    ThumbUrl = "https://www.uniqlo.com/images/sg/uq/pc/images/437286/item/sg437286003.jpg",
                    OriginalPrice = 29.90m,
                    SalePrice = 25m
                };
                var product5 = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Men Washed Jersey Crew Neck Short-Sleeve T-Shirt",
                    Description = "Crew neck T-shirt made from washed fabric for a casual look.",
                    ThumbUrl = "https://www.uniqlo.com/images/sg/uq/pc/images/443743/item/sg443743003.jpg",
                    OriginalPrice = 14.90m,
                    SalePrice = 13m
                };
                var product6 = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Women Fluffy Yarn Fleece Full-Zip Jacket",
                    Description = "Soft and warm, fluffy yarn fleece jacket.",
                    ThumbUrl = "https://www.uniqlo.com/images/sg/uq/pc/images/437847/item/sg437847003.jpg",
                    OriginalPrice = 39.90m,
                    SalePrice = 38.5m
                };
                var product7 = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Men EZY Ultra Stretch Color Jeans",
                    Description = "Made with ultra stretch fabric for comfort and ease of movement.",
                    ThumbUrl = "https://www.uniqlo.com/images/sg/uq/pc/images/430856/item/sg430856003.jpg",
                    OriginalPrice = 49.90m,
                    SalePrice = 49m
                };
                var product8 = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Women Ultra Light Down Compact Coat",
                    Description = "Warm, lightweight, and easy to move in. Pocketable design for easy carrying.",
                    ThumbUrl = "https://www.uniqlo.com/images/sg/uq/pc/images/429699/item/sg429699003.jpg",
                    OriginalPrice = 99.90m,
                    SalePrice = 98.8m
                }; 
                var product9 = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Men Dry Stretch Sweat Full-Zip Hoodie",
                    Description = "Dries sweat quickly for a smooth, dry feel.",
                    ThumbUrl = "https://www.uniqlo.com/images/sg/uq/pc/images/438723/item/sg438723003.jpg",
                    OriginalPrice = 49.90m,
                    SalePrice = 41m
                };

                var product10 = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Women Flannel Long-Sleeve Shirt",
                    Description = "Soft flannel material with a brushed surface for warmth and comfort.",
                    ThumbUrl = "https://www.uniqlo.com/images/sg/uq/pc/images/438687/item/sg438687003.jpg",
                    OriginalPrice = 29.90m,
                    SalePrice = 20.9m
                };

            // Seeding data for Products
            modelBuilder.Entity<Product>().HasData(
                product1, product2, product3, product4, product5, product6, product7, product8, product9, product10
            );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory
                {

                    Id = Guid.NewGuid(),
                    ProductId = product1.Id,
                    CategoryId = 1
                },
                new ProductCategory
                {

                    Id = Guid.NewGuid(),
                    ProductId = product2.Id,
                    CategoryId = 1
                },
                new ProductCategory
                {
                    Id = Guid.NewGuid(),
                    ProductId = product3.Id,
                    CategoryId = 2
                },
                new ProductCategory
                {

                    Id = Guid.NewGuid(),
                    ProductId = product4.Id,
                    CategoryId = 2
                },
                new ProductCategory
                {

                    Id = Guid.NewGuid(),
                    ProductId = product5.Id,
                    CategoryId = 2
                },
                new ProductCategory
                {
                    Id = Guid.NewGuid(),
                    ProductId = product6.Id,
                    CategoryId = 2
                },
                new ProductCategory
                {
                    Id = Guid.NewGuid(),
                    ProductId = product7.Id,
                    CategoryId = 1
                },
                new ProductCategory
                {
                    Id = Guid.NewGuid(),
                    ProductId = product8.Id,
                    CategoryId = 2
                },
                new ProductCategory
                {
                    Id = Guid.NewGuid(),
                    ProductId = product9.Id,
                    CategoryId = 1
                },
                new ProductCategory
                {
                    Id = Guid.NewGuid(),
                    ProductId = product10.Id,
                    CategoryId = 1
                }
            );

            modelBuilder.Entity<Colour>().HasData(
                new Colour { Id = 1, Value = "White" },
                new Colour { Id = 2, Value = "Gray" },
                new Colour { Id = 3, Value = "Black" },
                new Colour { Id = 4, Value = "Green" },
                new Colour { Id = 5, Value = "Purple" },
                new Colour { Id = 6, Value = "Navy" }
            );

            modelBuilder.Entity<Size>().HasData(
                new Size { Id = 1, Value = "XS" },
                new Size { Id = 2, Value = "S" },
                new Size { Id = 3, Value = "M" },
                new Size { Id = 4, Value = "L" },
                new Size { Id = 5, Value = "XL" },
                new Size { Id = 6, Value = "XXL" }
            );

            // Seeding data for ProductItems
            var productItem1 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product1.Id,
                ColourId = 1
            };

            var productItem2 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product1.Id,
                ColourId = 2
            };

            var productItem3 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product1.Id,
                ColourId = 3
            };

            var productItem4 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product1.Id,
                ColourId = 4
            };

            var productItem5 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product2.Id,
                ColourId = 1
            };

            var productItem6 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product2.Id,
                ColourId = 2
            };

            var productItem7 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product2.Id,
                ColourId = 3
            };

            var productItem8 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product2.Id,
                ColourId = 4
            };

            var productItem9 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product2.Id,
                ColourId = 5
            };

            var productItem10 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product2.Id,
                ColourId = 6
            };

            var productItem11 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product3.Id,
                ColourId = 1
            };

            var productItem12 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product1.Id,
                ColourId = 2
            };

            var productItem13 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product3.Id,
                ColourId = 3
            };

            var productItem14 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product4.Id,
                ColourId = 1
            };

            var productItem15 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product4.Id,
                ColourId = 2
            };

            var productItem16 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product4.Id,
                ColourId = 5
            };

            var productItem17 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product4.Id,
                ColourId = 6
            };

            var productItem18 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product5.Id,
                ColourId = 2
            };

            var productItem19 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product5.Id,
                ColourId = 3
            };

            var productItem20 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product5.Id,
                ColourId = 6
            };

            var productItem21 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product6.Id,
                ColourId = 2
            };

            var productItem22 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product6.Id,
                ColourId = 1
            };

            var productItem23 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product6.Id,
                ColourId = 3
            };

            var productItem24 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product7.Id,
                ColourId = 2
            };

            var productItem25 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product7.Id,
                ColourId = 4
            };

            var productItem26 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product8.Id,
                ColourId = 4
            };

            var productItem27 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product8.Id,
                ColourId = 6
            };

            var productItem28 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product9.Id,
                ColourId = 2
            };

            var productItem29 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product9.Id,
                ColourId = 3
            };

            var productItem30 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product9.Id,
                ColourId = 4
            };

            var productItem31 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product10.Id,
                ColourId = 1
            };

            var productItem32 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product10.Id,
                ColourId = 2
            };

            var productItem33 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product10.Id,
                ColourId = 5
            };

            var productItem34 = new ProductItem
            {
                Id = Guid.NewGuid(),
                ProductId = product10.Id,
                ColourId = 6
            };

            modelBuilder.Entity<ProductItem>().HasData(
                productItem1,
                productItem2,
                productItem3,
                productItem4,
                productItem5,
                productItem6,
                productItem7,
                productItem8,
                productItem9,
                productItem10,
                productItem11,
                productItem12,
                productItem13,
                productItem14,
                productItem15,
                productItem16,
                productItem17,
                productItem18,
                productItem19,
                productItem20,
                productItem21,
                productItem22,
                productItem23,
                productItem24,
                productItem25,
                productItem26,
                productItem27,
                productItem28,
                productItem29,
                productItem30,
                productItem31,
                productItem32,
                productItem33,
                productItem34

            );

            // Seeding data for ProductVariations
            modelBuilder.Entity<ProductVariation>().HasData(
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem1.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem1.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem1.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem1.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem1.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem1.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem2.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem2.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem2.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem2.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem2.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem3.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem3.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem3.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem3.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem3.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem3.Id, SizeId = 6, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem4.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem4.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem4.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem4.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem4.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem4.Id, SizeId = 6, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem5.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem5.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem5.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem5.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem5.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem5.Id, SizeId = 6, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem6.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem6.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem6.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem6.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem6.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem6.Id, SizeId = 6, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem7.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem7.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem7.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem7.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem7.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem7.Id, SizeId = 6, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem8.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem8.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem8.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem8.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem8.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem8.Id, SizeId = 6, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem9.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem9.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem9.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem9.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem9.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem9.Id, SizeId = 6, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem10.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem10.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem10.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem10.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem10.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem10.Id, SizeId = 6, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem11.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem11.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem11.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem11.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem11.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem11.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem12.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem12.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem12.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem12.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem12.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem12.Id, SizeId = 6, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem13.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem13.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem13.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem13.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem13.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem13.Id, SizeId = 6, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem14.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem14.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem14.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem14.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem14.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem14.Id, SizeId = 6, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem15.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem15.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem15.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem15.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem15.Id, SizeId = 6, QuantityInStock = 10 }, 
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem16.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem16.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem16.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem16.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem17.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem17.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem17.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem17.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem17.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem18.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem18.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem18.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem18.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem18.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem18.Id, SizeId = 6, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem19.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem19.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem19.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem19.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem19.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem19.Id, SizeId = 6, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem20.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem20.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem20.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem20.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem20.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem20.Id, SizeId = 6, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem21.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem21.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem21.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem21.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem21.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem21.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem22.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem22.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem22.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem22.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem22.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem22.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem23.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem23.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem23.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem23.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem24.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem24.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem24.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem24.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem24.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem24.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem25.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem25.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem25.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem25.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem26.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem26.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem26.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem26.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem26.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem26.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem27.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem27.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem27.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem27.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem27.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem27.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem28.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem28.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem28.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem28.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem28.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem29.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem29.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem29.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem29.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem29.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem29.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem30.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem30.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem30.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem30.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem30.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem30.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem31.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem31.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem31.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem31.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem31.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem31.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem32.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem32.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem32.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem32.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem32.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem32.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem33.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem33.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem33.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem33.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem33.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem33.Id, SizeId = 6, QuantityInStock = 5 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem34.Id, SizeId = 1, QuantityInStock = 50 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem34.Id, SizeId = 2, QuantityInStock = 40 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem34.Id, SizeId = 3, QuantityInStock = 30 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem34.Id, SizeId = 4, QuantityInStock = 20 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem34.Id, SizeId = 5, QuantityInStock = 10 },
                new ProductVariation { Id = Guid.NewGuid(), ProductItemId = productItem34.Id, SizeId = 6, QuantityInStock = 5 }
            );
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<PageVisit> PageVisits { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<ReviewImage> ReviewImages { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<ProductVariation> ProductVariations { get; set; }
    }
}