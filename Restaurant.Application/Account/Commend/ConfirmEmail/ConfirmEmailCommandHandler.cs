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

namespace Restaurant.Application.Account.Commend.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ServiceResult<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<ConfirmEmailCommandHandler> logger;

        public ConfirmEmailCommandHandler(
            UserManager<ApplicationUser> userManager,
            ILogger<ConfirmEmailCommandHandler> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<ServiceResult<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Email confirmation attempt for userId: {UserId}", request.UserId);

            if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Token))
            {
                logger.LogWarning("Email confirmation failed - missing userId or token");
                return new() { IsSuccess = false, Message = "Invalid email confirmation request.", ErrorType = "BadRequest" };
            }

            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                logger.LogWarning("Email confirmation failed - user not found: {UserId}", request.UserId);
                return new() { IsSuccess = false, Message = "User not found", ErrorType = "NotFound" };
            }

            var result = await userManager.ConfirmEmailAsync(user, request.Token);
            if (result.Succeeded)
            {
                logger.LogInformation("Email confirmed successfully for: {Email}", user.Email);
                return new() { IsSuccess = true, Message = "Email confirmed successfully!" };
            }

            logger.LogWarning("Email confirmation failed for: {Email}", user.Email);
            return new() { IsSuccess = false, Message = "Email confirmation failed.", ErrorType = "BadRequest" };
        }
    }
}
