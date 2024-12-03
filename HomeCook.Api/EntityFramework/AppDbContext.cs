 using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
    {
        //public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        //{

        //}
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodImage> FoodImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-one relationship between User and Profile
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.UserId);  // Profile is the dependent side, and UserId is the foreign key
        }
        //    //base.OnModelCreating(modelBuilder);
        //    //modelBuilder.Entity<User>().HasKey(u => u.Id);
        //    //Seed Categories
        //    var category1 = new Category
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "Asian"
        //    };
        //    var category2 = new Category
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "Dessert"
        //    };

        //    modelBuilder.Entity<Category>().HasData(category1, category2);

        //    //Seed Users(Sellers and Buyers)
        //    var seller1 = new Seller
        //    {
        //        //Id = Guid.NewGuid(),
        //        FirstName = "John",
        //        LastName = "Doe",
        //        Email = "john.doe@example.com",
        //        CreatedAt = DateTime.UtcNow,
        //        Role = Enums.UserRole.Seller,
        //    };
        //    var buyer1 = new Buyer
        //    {
        //        //Id = Guid.NewGuid(),
        //        FirstName = "Jane",
        //        LastName = "Smith",
        //        Email = "jane.smith@example.com",
        //        CreatedAt = DateTime.UtcNow,
        //        Role = Enums.UserRole.Buyer,
        //    };

        //    modelBuilder.Entity<Seller>().HasData(seller1);
        //    modelBuilder.Entity<Buyer>().HasData(buyer1);

        //    //Seed Profiles
        //    var profile1 = new Profile
        //    {
        //        Id = Guid.NewGuid(),
        //        Address = "123 Seller Street",
        //        PhoneNumber = "1234567890",
        //        City = "New York",
        //        Country = "USA",
        //        PostCode = "10001",
        //        UserId = seller1.Id // Associate profile with seller
        //    };
        //    var profile2 = new Profile()
        //    {
        //        Id = Guid.NewGuid(),
        //        Address = "456 Buyer Avenue",
        //        PhoneNumber = "0987654321",
        //        City = "Los Angeles",
        //        Country = "USA",
        //        PostCode = "90001",
        //        UserId = buyer1.Id // Associate profile with seller
        //    };

        //    modelBuilder.Entity<Profile>().HasData(profile1, profile2);

        //    //Seed Foods
        //    var food1 = new Food
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "Pizza",
        //        Description = "Cheese pizza with tomato sauce",
        //        Price = 12.99m,
        //        Ingredients = "Flour, Cheese, Tomato Sauce",
        //        AvailableDate = DateTime.UtcNow.AddDays(1),
        //        CategoryId = category1.Id, // Reference the category
        //        SellerId = seller1.Id // Reference the seller
        //    };

        //    var food2 = new Food
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "Brownie",
        //        Description = "Delicious chocolate brownie",
        //        Price = 5.99m,
        //        Ingredients = "Chocolate, Flour, Sugar, Butter",
        //        AvailableDate = DateTime.UtcNow.AddDays(2),
        //        CategoryId = category2.Id, // Reference the category
        //        SellerId = seller1.Id // Reference the seller
        //    };

        //    modelBuilder.Entity<Food>().HasData(food1, food2);
        //}
    }
}

