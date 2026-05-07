using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Restaurant.Core.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace Restaurant.Core.Entity.Account.Configuration
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant.Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant.Restaurant> builder)
        {
            builder
            .OwnsOne(r => r.Address);

            builder
                .HasMany(r => r.Dishes)
                .WithOne()
                .HasForeignKey(d => d.RestaurantId); ;
        }
    }
}
