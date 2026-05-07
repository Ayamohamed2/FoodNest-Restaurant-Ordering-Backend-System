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

namespace Restaurant.Application.Account.Commend.DisableTwoFactor
{
    public class DisableTwoFactorCommandHandler: IRequestHandler<DisableTwoFactorCommand, ServiceResult<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<DisableTwoFactorCommandHandler> logger;

        public DisableTwoFactorCommandHandler(UserManager<ApplicationUser> userManager,ILogger<DisableTwoFactorCommandHandler>logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<ServiceResult<string>> Handle(DisableTwoFactorCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Disable 2FA request for userId: {UserId}", request.UserId);

            var user = await userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                logger.LogWarning("Disable 2FA failed - user not found: {UserId}", request.UserId);
                return new() { IsSuccess = false, Message = "User not found" };
            }

            user.TwoFactorEnabled = false;

            await userManager.UpdateAsync(user);

            logger.LogInformation("2FA disabled successfully for userId: {UserId}", request.UserId);

            return new()
            {
                IsSuccess = true,
                Message = "Two Factor Disabled Successfully"
            };
        }
    }
}
