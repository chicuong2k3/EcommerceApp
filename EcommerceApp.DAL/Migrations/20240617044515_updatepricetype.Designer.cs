﻿// <auto-generated />
using System;
using EcommerceApp.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EcommerceApp.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240617044515_updatepricetype")]
    partial class updatepricetype
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EcommerceApp.Domain.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Laptop"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Điện thoại"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Đồng hồ"
                        });
                });

            modelBuilder.Entity("EcommerceApp.Domain.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("EcommerceApp.Domain.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SalePrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Name = "Laptop mini Samsung N150",
                            Price = 8690000m,
                            SalePrice = 8290000m
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 1,
                            Name = "Laptop Dell Latitude. i5 M520 ram 8G 500Gb",
                            Price = 12690000m,
                            SalePrice = 11690000m
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 1,
                            Name = "Laptop cũ dell latitude e7280 i7 7600u ram 8gb",
                            Price = 5690000m,
                            SalePrice = 5090000m
                        },
                        new
                        {
                            Id = 4,
                            CategoryId = 1,
                            Name = "Laptop Dell bền bỉ i5 2.4Ghz",
                            Price = 3690000m,
                            SalePrice = 3300000m
                        },
                        new
                        {
                            Id = 5,
                            CategoryId = 1,
                            Name = "TOSHIBA RAM 8G. i5 4300M upto 3.3Ghz 2.6GHZ",
                            Price = 10690000m,
                            SalePrice = 9690000m
                        },
                        new
                        {
                            Id = 6,
                            CategoryId = 2,
                            Name = "Điện thoại Xiaomi Redmi 9A 2GB-32GB",
                            Price = 4690000m,
                            SalePrice = 3700000m
                        },
                        new
                        {
                            Id = 7,
                            CategoryId = 2,
                            Name = "Điện thoại Oppo F11 Ram 8GB/ 256GB",
                            Price = 1690000m,
                            SalePrice = 1500000m
                        },
                        new
                        {
                            Id = 8,
                            CategoryId = 2,
                            Name = "Xiaomi Redmi 13C Ram 8GB/ 256GB",
                            Price = 5690000m,
                            SalePrice = 4690000m
                        },
                        new
                        {
                            Id = 9,
                            CategoryId = 2,
                            Name = "Realme C33 Ram 8GB/ 256GB",
                            Price = 1700000m,
                            SalePrice = 1500000m
                        },
                        new
                        {
                            Id = 10,
                            CategoryId = 2,
                            Name = "Điện thoại Oppo Reno 4z 5G Ram 12GB/256GB",
                            Price = 3200000m,
                            SalePrice = 3100000m
                        },
                        new
                        {
                            Id = 11,
                            CategoryId = 3,
                            Name = "ĐỒNG HỒ TRẺ EM COOBOS",
                            Price = 590000m,
                            SalePrice = 520000m
                        },
                        new
                        {
                            Id = 12,
                            CategoryId = 3,
                            Name = "Đồng hồ Nam ROMATIC DOMINIC",
                            Price = 1590000m,
                            SalePrice = 1290000m
                        },
                        new
                        {
                            Id = 13,
                            CategoryId = 3,
                            Name = "Đồng Hồ Nam Thời Trang Doanh Nhân Dây Da PU",
                            Price = 2690000m,
                            SalePrice = 2090000m
                        },
                        new
                        {
                            Id = 14,
                            CategoryId = 3,
                            Name = "Đồng hồ Nữ VENUS Hàn Quốc",
                            Price = 3690000m,
                            SalePrice = 3190000m
                        },
                        new
                        {
                            Id = 15,
                            CategoryId = 3,
                            Name = "Đồng hồ đôi NEOS N-30932M",
                            Price = 4200000m,
                            SalePrice = 4000000m
                        });
                });

            modelBuilder.Entity("EcommerceApp.Domain.Models.ShoppingCart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("EcommerceApp.Domain.Models.ShoppingCartItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("ShoppingCartId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShoppingCartId");

                    b.ToTable("ShoppingCartItems");
                });

            modelBuilder.Entity("EcommerceApp.Domain.Models.Product", b =>
                {
                    b.HasOne("EcommerceApp.Domain.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("EcommerceApp.Domain.Models.ShoppingCart", b =>
                {
                    b.HasOne("EcommerceApp.Domain.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("EcommerceApp.Domain.Models.ShoppingCartItem", b =>
                {
                    b.HasOne("EcommerceApp.Domain.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EcommerceApp.Domain.Models.ShoppingCart", null)
                        .WithMany("ShoppingCartItems")
                        .HasForeignKey("ShoppingCartId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("EcommerceApp.Domain.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("EcommerceApp.Domain.Models.ShoppingCart", b =>
                {
                    b.Navigation("ShoppingCartItems");
                });
#pragma warning restore 612, 618
        }
    }
}
