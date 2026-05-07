using System.ComponentModel.DataAnnotations;

namespace Restaurant.Application.Account.DTOs.Email
{
    public class EmailDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
