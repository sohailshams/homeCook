using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HomeCook.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeCook.Api.EntityFramework
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
    {
        //public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        //{

        //}
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodImage> FoodImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-one relationship between User and Profile
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.UserId);  // Profile is the dependent side, and UserId is the foreign key

            // Full Text Search setup for Food
            modelBuilder.Entity<Food>()
                .HasGeneratedTsVectorColumn(
                    f => f.SearchVector,
                    "english",
                    f => new { f.Name, f.Description })
                .HasIndex(f => f.SearchVector)
                .HasMethod("GIN");
        }
    }
}
