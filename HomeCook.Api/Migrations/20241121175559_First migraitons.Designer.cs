﻿// <auto-generated />
using System;
using HomeCook.Api.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HomeCook.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241121175559_First migraitons")]
    partial class Firstmigraitons
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HomeCook.Api.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("710de10b-cdf9-4b9f-a345-14d0703c4e07"),
                            Name = "Asian"
                        },
                        new
                        {
                            Id = new Guid("78845a1b-67c9-469e-813b-834cbe52a2df"),
                            Name = "Dessert"
                        });
                });

            modelBuilder.Entity("HomeCook.Api.Models.Food", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("AvailableDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("BuyerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("FoodImageId")
                        .HasColumnType("uuid");

                    b.Property<string>("Ingredients")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("QuantityAvailable")
                        .HasColumnType("integer");

                    b.Property<Guid>("SellerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SellerId");

                    b.ToTable("Foods");

                    b.HasData(
                        new
                        {
                            Id = new Guid("15ccf961-41c6-4b98-b454-37d79b3b4596"),
                            AvailableDate = new DateTime(2024, 11, 22, 17, 55, 58, 148, DateTimeKind.Utc).AddTicks(6221),
                            CategoryId = new Guid("710de10b-cdf9-4b9f-a345-14d0703c4e07"),
                            Description = "Cheese pizza with tomato sauce",
                            FoodImageId = new Guid("00000000-0000-0000-0000-000000000000"),
                            Ingredients = "Flour, Cheese, Tomato Sauce",
                            Name = "Pizza",
                            Price = 12.99m,
                            QuantityAvailable = 0,
                            SellerId = new Guid("f6fa2df0-a483-4879-8528-d9621521ee33")
                        },
                        new
                        {
                            Id = new Guid("8c410b0e-5f57-43d6-8fbe-69722a5eb053"),
                            AvailableDate = new DateTime(2024, 11, 23, 17, 55, 58, 148, DateTimeKind.Utc).AddTicks(6234),
                            CategoryId = new Guid("78845a1b-67c9-469e-813b-834cbe52a2df"),
                            Description = "Delicious chocolate brownie",
                            FoodImageId = new Guid("00000000-0000-0000-0000-000000000000"),
                            Ingredients = "Chocolate, Flour, Sugar, Butter",
                            Name = "Brownie",
                            Price = 5.99m,
                            QuantityAvailable = 0,
                            SellerId = new Guid("f6fa2df0-a483-4879-8528-d9621521ee33")
                        });
                });

            modelBuilder.Entity("HomeCook.Api.Models.FoodImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FoodId")
                        .HasColumnType("uuid");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FoodId");

                    b.ToTable("FoodImages");
                });

            modelBuilder.Entity("HomeCook.Api.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BuyerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FoodId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("SellerId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("FoodId");

                    b.HasIndex("SellerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("HomeCook.Api.Models.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FoodId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FoodId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("HomeCook.Api.Models.Profile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Bio")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProfileImage")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Profiles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("27a550fd-4a76-4997-9ad4-c530ab0cac0b"),
                            Address = "123 Seller Street",
                            City = "New York",
                            Country = "USA",
                            PhoneNumber = "1234567890",
                            PostCode = "10001",
                            UserId = new Guid("f6fa2df0-a483-4879-8528-d9621521ee33")
                        },
                        new
                        {
                            Id = new Guid("479dbf36-3f3d-4b1b-a7bf-b1922bb15ec6"),
                            Address = "456 Buyer Avenue",
                            City = "Los Angeles",
                            Country = "USA",
                            PhoneNumber = "0987654321",
                            PostCode = "90001",
                            UserId = new Guid("182f53a1-f29b-425e-91e0-1581cb9e319d")
                        });
                });

            modelBuilder.Entity("HomeCook.Api.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator().HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("HomeCook.Api.Models.Buyer", b =>
                {
                    b.HasBaseType("HomeCook.Api.Models.User");

                    b.HasDiscriminator().HasValue("Buyer");

                    b.HasData(
                        new
                        {
                            Id = new Guid("182f53a1-f29b-425e-91e0-1581cb9e319d"),
                            CreatedAt = new DateTime(2024, 11, 21, 17, 55, 58, 148, DateTimeKind.Utc).AddTicks(6022),
                            Email = "jane.smith@example.com",
                            FirstName = "Jane",
                            LastName = "Smith",
                            Role = 1
                        });
                });

            modelBuilder.Entity("HomeCook.Api.Models.Seller", b =>
                {
                    b.HasBaseType("HomeCook.Api.Models.User");

                    b.HasDiscriminator().HasValue("Seller");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f6fa2df0-a483-4879-8528-d9621521ee33"),
                            CreatedAt = new DateTime(2024, 11, 21, 17, 55, 58, 148, DateTimeKind.Utc).AddTicks(6010),
                            Email = "john.doe@example.com",
                            FirstName = "John",
                            LastName = "Doe",
                            Role = 2
                        });
                });

            modelBuilder.Entity("HomeCook.Api.Models.Food", b =>
                {
                    b.HasOne("HomeCook.Api.Models.Buyer", null)
                        .WithMany("Foods")
                        .HasForeignKey("BuyerId");

                    b.HasOne("HomeCook.Api.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeCook.Api.Models.Seller", "Seller")
                        .WithMany("Foods")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("HomeCook.Api.Models.FoodImage", b =>
                {
                    b.HasOne("HomeCook.Api.Models.Food", "Food")
                        .WithMany("FoodImage")
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Food");
                });

            modelBuilder.Entity("HomeCook.Api.Models.Order", b =>
                {
                    b.HasOne("HomeCook.Api.Models.Buyer", "Buyer")
                        .WithMany()
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeCook.Api.Models.Food", "Food")
                        .WithMany()
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeCook.Api.Models.Seller", "Seller")
                        .WithMany()
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Buyer");

                    b.Navigation("Food");

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("HomeCook.Api.Models.OrderItem", b =>
                {
                    b.HasOne("HomeCook.Api.Models.Food", "Food")
                        .WithMany()
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeCook.Api.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Food");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("HomeCook.Api.Models.Profile", b =>
                {
                    b.HasOne("HomeCook.Api.Models.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("HomeCook.Api.Models.Profile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HomeCook.Api.Models.Food", b =>
                {
                    b.Navigation("FoodImage");
                });

            modelBuilder.Entity("HomeCook.Api.Models.User", b =>
                {
                    b.Navigation("Profile");
                });

            modelBuilder.Entity("HomeCook.Api.Models.Buyer", b =>
                {
                    b.Navigation("Foods");
                });

            modelBuilder.Entity("HomeCook.Api.Models.Seller", b =>
                {
                    b.Navigation("Foods");
                });
#pragma warning restore 612, 618
        }
    }
}