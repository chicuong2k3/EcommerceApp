using EcommerceApp.Domain.Constants;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

            modelBuilder.Entity<ProductVariant>()
                .HasKey(e => new { e.ProductId, e.VariantNumber });
            modelBuilder.Entity<ProductVariant>()
                .HasOne(e => e.Product).WithMany(e => e.ProductVariants);
            modelBuilder.Entity<ProductVariant>()
                .HasMany(e => e.CartItems).WithOne(e => e.ProductVariant);
            modelBuilder.Entity<ProductVariant>()
                .HasMany(e => e.ProductImages).WithOne(e => e.ProductVariant);

            // Seed data for Roles
            var adminRole = new IdentityRole()
            {
                Id = "7d9c829f-f420-4bcd-879d-247eb817adab",
                Name = UserRoleConstant.Admin,
                NormalizedName = UserRoleConstant.NormalizedAdmin
            };
            var customerRole = new IdentityRole()
            {
                Id = "9908864b-a330-49ab-a8bd-b2bac67263ff",
                Name = UserRoleConstant.Customer,
                NormalizedName = UserRoleConstant.NormalizedCustomer
            };
           

            modelBuilder.Entity<IdentityRole>().HasData(
                adminRole,
                customerRole
            );

            // Seed data for AppUsers

            var passwordHasher = new PasswordHasher<AppUser>();

            var admin = new AppUser()
            {
                Id = "aa2d56cd-d0dc-452d-916f-3d52977e6a8a",
                FirstName = "Minh",
                LastName = "Dương Văn",
                UserName = "admin12345",
                NormalizedUserName = "ADMIN12345",
                Email = "admin12345@gmail.com",
                NormalizedEmail = "ADMIN12345@GMAIL.COM",
                RegistrationDate = DateTime.Now
            };

            admin.PasswordHash = passwordHasher.HashPassword(admin, "admin12345");

            var customer = new AppUser()
            {
                Id = "bed414f3-12ee-4121-83db-60e8ebaa9d40",
                FirstName = "Hùng",
                LastName = "Trần Quốc",
                UserName = "customer123",
                NormalizedUserName = "CUSTOMER123",
                Email = "customer123@gmail.com",
                NormalizedEmail = "CUSTOMER123@GMAIL.COM",
                RegistrationDate = DateTime.Now
            };

            customer.PasswordHash = passwordHasher.HashPassword(customer, "customer123");

            modelBuilder.Entity<AppUser>().HasData(
                admin,
                customer
            );

            var cart = new Cart()
            {
                Id = Guid.NewGuid(),
                AppUserId = customer.Id
            };

            modelBuilder.Entity<Cart>().HasData(cart);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>()
                {
                    UserId = admin.Id,
                    RoleId = adminRole.Id
                },
                new IdentityUserRole<string>()
                {
                    UserId = customer.Id,
                    RoleId = customerRole.Id
                }
            );


            modelBuilder.Entity<Review>().HasOne(e => e.OrderLine)
                .WithMany(e => e.Reviews)
                .OnDelete(DeleteBehavior.NoAction);

            // Seeding data for Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Đồ Nữ", Slug = "do-nu" },
                new Category { Id = 2, Name = "Đồ Nam", Slug = "do-nam" },
                new Category { Id = 3, Name = "Áo Thun", Slug = "ao-thun" },
                new Category { Id = 4, Name = "Áo Polo", Slug = "ao-polo" }
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

            // Seeding data for ProductVariants
            modelBuilder.Entity<ProductVariant>().HasData(
                new ProductVariant { ProductId = product1.Id, VariantNumber = 1, ColourId = 1, SizeId = 1, QuantityInStock = 50 },
                new ProductVariant { ProductId = product1.Id, VariantNumber = 2, ColourId = 1, SizeId = 2, QuantityInStock = 40 },
                new ProductVariant { ProductId = product1.Id, VariantNumber = 3, ColourId = 2, SizeId = 1, QuantityInStock = 30 },
                new ProductVariant { ProductId = product1.Id, VariantNumber = 4, ColourId = 2, SizeId = 2, QuantityInStock = 20 },
                new ProductVariant { ProductId = product1.Id, VariantNumber = 5, ColourId = 2, SizeId = 3, QuantityInStock = 10 },
                new ProductVariant { ProductId = product1.Id, VariantNumber = 6, ColourId = 2, SizeId = 4, QuantityInStock = 5 },
                new ProductVariant { ProductId = product2.Id, VariantNumber = 1, ColourId = 1, SizeId = 1, QuantityInStock = 50 },
                new ProductVariant { ProductId = product2.Id, VariantNumber = 2, ColourId = 1, SizeId = 2, QuantityInStock = 40 },
                new ProductVariant { ProductId = product2.Id, VariantNumber = 3, ColourId = 3, SizeId = 2, QuantityInStock = 30 },
                new ProductVariant { ProductId = product2.Id, VariantNumber = 4, ColourId = 3, SizeId = 3, QuantityInStock = 20 },
                new ProductVariant { ProductId = product2.Id, VariantNumber = 5, ColourId = 3, SizeId = 4, QuantityInStock = 10 },
                new ProductVariant { ProductId = product3.Id, VariantNumber = 1, ColourId = 1, SizeId = 1, QuantityInStock = 50 },
                new ProductVariant { ProductId = product3.Id, VariantNumber = 2, ColourId = 1, SizeId = 2, QuantityInStock = 40 },
                new ProductVariant { ProductId = product3.Id, VariantNumber = 3, ColourId = 1, SizeId = 3, QuantityInStock = 30 },
                new ProductVariant { ProductId = product3.Id, VariantNumber = 4, ColourId = 4, SizeId = 3, QuantityInStock = 20 },
                new ProductVariant { ProductId = product3.Id, VariantNumber = 5, ColourId = 4, SizeId = 4, QuantityInStock = 10 },
                new ProductVariant { ProductId = product3.Id, VariantNumber = 6, ColourId = 5, SizeId = 3, QuantityInStock = 10 }
            );
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<PageVisit> PageVisits { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<ReviewImage> ReviewImages { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
    }
}