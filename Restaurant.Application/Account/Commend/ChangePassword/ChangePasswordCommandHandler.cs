using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Core.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.ChangePassword
{
    public class ChangePasswordCommandHandler
    : IRequestHandler<ChangePasswordCommand, ServiceResult<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<ChangePasswordCommandHandler> logger;

        public ChangePasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            ILogger<ChangePasswordCommandHandler> logger)
        {
            this.userManager = userManager;
           this.logger = logger;
        }

        public async Task<ServiceResult<string>> Handle( ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Change password request for userId: {UserId}", request.UserId);

            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                logger.LogWarning("Change password failed - user not found: {UserId}", request.UserId);
                return new() { IsSuccess = false, Message = "User not found", ErrorType = "BadRequest" };
            }

            var result = await userManager.ChangePasswordAsync(user, request.Model.CurrentPassword, request.Model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join("\n", result.Errors.Select(e => e.Description));
                logger.LogWarning("Change password failed for userId: {UserId}. Errors: {Errors}", request.UserId, errors);
                return new() { IsSuccess = false, Message = errors, ErrorType = "BadRequest" };
            }

            logger.LogInformation("Password changed successfully for userId: {UserId}", request.UserId);
            return new() { IsSuccess = true, Message = "Password changed successfully." };

        }
   
    }
}
