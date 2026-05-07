using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Restaurant.Core.Entity.Restaurant;

namespace Restaurant.Core.Models.Account
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? ImageURL { get; set; } = "/Images/default.png";
        public IFormFile? Imagefile { get; set; }

        public DateOnly? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
       public List<Restaurant.Core.Entity.Restaurant.Restaurant> OwnedRestaurants { get; set; } = [];
    }
}
