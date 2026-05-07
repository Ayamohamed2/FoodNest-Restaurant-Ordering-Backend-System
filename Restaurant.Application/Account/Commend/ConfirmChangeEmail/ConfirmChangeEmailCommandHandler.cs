using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Account.Commend.ExternalLogin;
using Restaurant.Core.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.ConfirmChangeEmail
{
    public class ConfirmChangeEmailCommandHandler
     : IRequestHandler<ConfirmChangeEmailCommand, ServiceResult<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<ConfirmChangeEmailCommandHandler> logger;

        public ConfirmChangeEmailCommandHandler(UserManager<ApplicationUser> userManager, ILogger<ConfirmChangeEmailCommandHandler> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<ServiceResult<string>> Handle( ConfirmChangeEmailCommand request ,CancellationToken cancellationToken)
        {
            logger.LogInformation("Confirm change email for userId: {UserId}, newEmail: {NewEmail}", request.UserId, request.NewEmail);

            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                logger.LogWarning("Confirm change email failed - user not found: {UserId}", request.UserId);
                return new() { IsSuccess = false, Message = "User not found", ErrorType = "BadRequest" };
            }

            var decodedToken = Uri.UnescapeDataString(request.Token);
            var result = await userManager.ChangeEmailAsync(user, request.NewEmail, decodedToken);
            if (!result.Succeeded)
            {
                logger.LogWarning("Confirm change email failed for userId: {UserId}", request.UserId);
                return new() { IsSuccess = false, Message = "Failed to change email", ErrorType = "BadRequest" };
            }

            user.EmailConfirmed = true;
            await userManager.UpdateAsync(user);

            if (user.UserName != null && user.UserName.Contains("@"))
                await userManager.SetUserNameAsync(user, request.NewEmail);

            logger.LogInformation("Email changed successfully for userId: {UserId} to: {NewEmail}", request.UserId, request.NewEmail);
            return new() { IsSuccess = true, Message = "Email changed and confirmed successfully." };


        }
   }
}
