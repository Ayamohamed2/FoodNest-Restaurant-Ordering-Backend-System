using Restaurant.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Core.Entity.Restaurant
{
    public class Dish :BaseClass
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }

        public int? KiloCalories { get; set; }

        public int RestaurantId { get; set; }
    }
}
