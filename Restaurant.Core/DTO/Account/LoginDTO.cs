using System.ComponentModel.DataAnnotations;

namespace Restaurant.Domain.DTO.Account
{
    public class LoginDTO
    {

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
