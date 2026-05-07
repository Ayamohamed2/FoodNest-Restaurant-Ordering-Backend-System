using FluentValidation;
using Restaurant.Application.Account.DTOs.Acoount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.DTOs.Acoount.Validation
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDTO>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Current password is required");


            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("New password is required");


            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty()
                .WithMessage("Confirm password is required")
                .Equal(x => x.NewPassword)
                .WithMessage("Passwords do not match");
        }
    }
}
