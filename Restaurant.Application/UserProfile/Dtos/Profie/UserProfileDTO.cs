using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Restaurant.Application.UserProfile.Dtos.Profie
{
    public class UserProfileDTO
    {
        public string Name { get; set; }
        public string? phoneNumber { get; set; }
        [ValidateNever]
        public IFormFile? imagefile { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
    }
}
