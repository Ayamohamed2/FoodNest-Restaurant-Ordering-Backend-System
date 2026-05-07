using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Application.Account.DTOs.Acoount
{
    public class ResetPasswordDTO
    {


        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
