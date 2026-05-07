using System.ComponentModel.DataAnnotations;

namespace Restaurant.Application.Account.DTOs.Acoount
{
    public class ChangePasswordDTO
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}
