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

namespace Restaurant.Application.Account.Commend.EnableTwoFactor
{
    public class EnableTwoFactorCommandHandler
    : IRequestHandler<EnableTwoFactorCommand, ServiceResult<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<EnableTwoFactorCommandHandler> logger;

        public EnableTwoFactorCommandHandler(UserManager<ApplicationUser> userManager,ILogger<EnableTwoFactorCommandHandler> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<ServiceResult<string>> Handle(EnableTwoFactorCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Enable 2FA request for userId: {UserId}", request.UserId);

            var user = await userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                logger.LogWarning("Enable 2FA failed - user not found: {UserId}", request.UserId);
                return new() { IsSuccess = false, Message = "User not found" };
            }

            user.TwoFactorEnabled = true;

            await userManager.UpdateAsync(user);

            logger.LogInformation("2FA enabled successfully for userId: {UserId}", request.UserId);

            return new()
            {
                IsSuccess = true,
                Message = "Two Factor Enabled Successfully"
            };
        }
    }
}
