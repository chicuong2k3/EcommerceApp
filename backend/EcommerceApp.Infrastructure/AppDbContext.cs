using EcommerceApp.Common.Constants;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure.TableConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EcommerceApp.Infrastructure
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Table Configuration
            modelBuilder.Entity<AppUser>().ToTable("AppUsers");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");

            
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderLineConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStatusConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductItemConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());
            #endregion

            #region Data seeding
            #region Users and Roles
            var adminRoleId = Guid.NewGuid().ToString();
            var customerRoleId = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole() 
                { 
                    Id = adminRoleId,
                    Name = UserRoleConstant.Admin, 
                    NormalizedName = UserRoleConstant.NormalizedAdmin
                },
                new IdentityRole()
                {
                    Id = customerRoleId,
                    Name = UserRoleConstant.Customer,
                    NormalizedName = UserRoleConstant.NormalizedCustomer
                }
            );


            var passwordHasher = new PasswordHasher<AppUser>();

            var user1 = new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Dododo",
                LastName = "Golem",
                UserName = "admin12345",
                RegistrationDate = DateTime.Now
            };
            user1.PasswordHash = passwordHasher.HashPassword(user1, "admin12345");

            var user2 = new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Gagaga",
                LastName = "Magician",
                UserName = "customer12345",
                RegistrationDate = DateTime.Now
            };
            user2.PasswordHash = passwordHasher.HashPassword(user2, "customer12345");

            modelBuilder.Entity<AppUser>().HasData(
                user1, user2
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>()
                {
                    RoleId = adminRoleId,
                    UserId = user1.Id
                },
                new IdentityUserRole<string>()
                {
                    RoleId = customerRoleId,
                    UserId = user2.Id
                }
            );
            #endregion

            #region Catalog

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    Name = "áo nam",
                    Slug = "ao-nam"
                },
                new Category()
                {
                    Id = 2,
                    Name = "quần nam",
                    Slug = "quan-nam"
                },
                new Category()
                {
                    Id = 3,
                    Name = "áo nữ",
                    Slug = "ao-nu"
                },
                new Category()
                {
                    Id = 4,
                    Name = "quần nữ",
                    Slug = "quan-nu"
                }
            );

            #endregion
            #endregion
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<ProductVariation> ProductVariations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
    }
}