using Restaurant.Core.Models.Account;
using Restaurant.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Core.Entity.Restaurant
{
    public class Restaurant:BaseClass
    {
       
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Category { get; set; } = default!;
        public bool HasDelivery { get; set; }

        public string? ContactEmail { get; set; }
        public string? ContactNumber { get; set; }

        public Address? Address { get; set; }
        public List<Dish> Dishes { get; set; } = new();

        public ApplicationUser Owner { get; set; } = default!;
        public string OwnerId { get; set; } = default!;
        public string? LogoUrl { get; set; }
    }
}
