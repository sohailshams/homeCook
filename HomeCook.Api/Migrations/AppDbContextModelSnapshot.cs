﻿// <auto-generated />
using System;
using HomeCook.Api.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HomeCook.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                });

            modelBuilder.Entity("HomeCook.Api.Models.Food", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("AvailableDate")
                        .HasColumnType("timestamp with time zone");

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

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("HomeCook.Api.Models.FoodImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("FoodId")
                        .HasColumnType("uuid");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FoodId");

                    b.ToTable("RecipeImages");
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

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Profiles");
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

                    b.Property<Guid>("ProfileId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasDiscriminator().HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("HomeCook.Api.Models.Buyer", b =>
                {
                    b.HasBaseType("HomeCook.Api.Models.User");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.ToTable("Users", t =>
                        {
                            t.Property("Role")
                                .HasColumnName("Buyer_Role");
                        });

                    b.HasDiscriminator().HasValue("Buyer");
                });

            modelBuilder.Entity("HomeCook.Api.Models.Seller", b =>
                {
                    b.HasBaseType("HomeCook.Api.Models.User");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasDiscriminator().HasValue("Seller");
                });

            modelBuilder.Entity("HomeCook.Api.Models.Food", b =>
                {
                    b.HasOne("HomeCook.Api.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("HomeCook.Api.Models.FoodImage", b =>
                {
                    b.HasOne("HomeCook.Api.Models.Food", null)
                        .WithMany("FoodImage")
                        .HasForeignKey("FoodId");
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

            modelBuilder.Entity("HomeCook.Api.Models.User", b =>
                {
                    b.HasOne("HomeCook.Api.Models.Profile", "Profile")
                        .WithOne("User")
                        .HasForeignKey("HomeCook.Api.Models.User", "ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("HomeCook.Api.Models.Food", b =>
                {
                    b.Navigation("FoodImage");
                });

            modelBuilder.Entity("HomeCook.Api.Models.Profile", b =>
                {
                    b.Navigation("User")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
