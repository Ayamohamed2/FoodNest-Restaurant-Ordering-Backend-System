using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Core.Entity.Account.Configuration;
using Restaurant.Core.Entity.Restaurant;
using Restaurant.Core.Models.Account;
using System.Data;

namespace Villa_API_Project.DataAccess.Data
{
    public class Context: IdentityDbContext<ApplicationUser>
    {
        public Context()
        {
            
        }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        
        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<RevokedTokens> RevokedTokens { get; set; }
        public DbSet<Restaurant.Core.Entity.Restaurant.Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)


        {
            optionsBuilder.UseSqlServer(@"Data source= DESKTOP-76BH0R4 ; Initial catalog =Restaurant; Integrated security= true;TrustServerCertificate=True;");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());


            modelBuilder.ApplyConfiguration(new RestaurantConfiguration());

        }
    }

}
