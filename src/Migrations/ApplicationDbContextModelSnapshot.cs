﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using RolleiShop.Data.Context;
using System;

namespace RolleiShop.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("RolleiShop.Models.Entities.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BuyerId");

                    b.HasKey("Id");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("RolleiShop.Models.Entities.CartItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CartId");

                    b.Property<int>("CatalogItemId");

                    b.Property<int>("Quantity");

                    b.Property<decimal>("UnitPrice");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.ToTable("CartItem");
                });

            modelBuilder.Entity("RolleiShop.Models.Entities.CatalogBrand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("CatalogBrand");
                });

            modelBuilder.Entity("RolleiShop.Models.Entities.CatalogItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AvailableStock");

                    b.Property<int>("CatalogBrandId");

                    b.Property<int>("CatalogTypeId");

                    b.Property<string>("Description");

                    b.Property<string>("ImageName");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<decimal>("Price");

                    b.HasKey("Id");

                    b.HasIndex("CatalogBrandId");

                    b.HasIndex("CatalogTypeId");

                    b.ToTable("Catalog");
                });

            modelBuilder.Entity("RolleiShop.Models.Entities.CatalogType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("CatalogType");
                });

            modelBuilder.Entity("RolleiShop.Models.Entities.Order.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BuyerId");

                    b.Property<DateTimeOffset>("OrderDate");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("RolleiShop.Models.Entities.Order.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("OrderId");

                    b.Property<decimal>("UnitPrice");

                    b.Property<int>("Units");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("RolleiShop.Models.Entities.CartItem", b =>
                {
                    b.HasOne("RolleiShop.Models.Entities.Cart")
                        .WithMany("Items")
                        .HasForeignKey("CartId");
                });

            modelBuilder.Entity("RolleiShop.Models.Entities.CatalogItem", b =>
                {
                    b.HasOne("RolleiShop.Models.Entities.CatalogBrand", "CatalogBrand")
                        .WithMany()
                        .HasForeignKey("CatalogBrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RolleiShop.Models.Entities.CatalogType", "CatalogType")
                        .WithMany()
                        .HasForeignKey("CatalogTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RolleiShop.Models.Entities.Order.Order", b =>
                {
                    b.OwnsOne("RolleiShop.Models.Entities.Order.Address", "ShipToAddress", b1 =>
                        {
                            b1.Property<int>("OrderId");

                            b1.Property<string>("City");

                            b1.Property<string>("Country");

                            b1.Property<string>("State");

                            b1.Property<string>("Street");

                            b1.Property<string>("ZipCode");

                            b1.ToTable("Orders");

                            b1.HasOne("RolleiShop.Models.Entities.Order.Order")
                                .WithOne("ShipToAddress")
                                .HasForeignKey("RolleiShop.Models.Entities.Order.Address", "OrderId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("RolleiShop.Models.Entities.Order.OrderItem", b =>
                {
                    b.HasOne("RolleiShop.Models.Entities.Order.Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId");

                    b.OwnsOne("RolleiShop.Models.Entities.Order.CatalogItemOrdered", "ItemOrdered", b1 =>
                        {
                            b1.Property<int>("OrderItemId");

                            b1.Property<int>("CatalogItemId");

                            b1.Property<string>("ImageUrl");

                            b1.Property<string>("ProductName");

                            b1.ToTable("OrderItems");

                            b1.HasOne("RolleiShop.Models.Entities.Order.OrderItem")
                                .WithOne("ItemOrdered")
                                .HasForeignKey("RolleiShop.Models.Entities.Order.CatalogItemOrdered", "OrderItemId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
