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

namespace Restaurant.Application.Account.Commend.ResetPassword
{
    public class ResetPasswordCommandHandler: IRequestHandler<ResetPasswordCommand, ServiceResult<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<ResetPasswordCommandHandler> logger;

        public ResetPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            ILogger<ResetPasswordCommandHandler> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<ServiceResult<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Reset password attempt for: {Email}",request.Email );

            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                logger.LogWarning("Reset password failed - email not found: {Email}", request.Email);
                return new() { IsSuccess = false, Message = "Invalid Email", ErrorType = "BadRequest" };
            }

            var decodedToken = System.Web.HttpUtility.UrlDecode(request.Token);
            var result = await userManager.ResetPasswordAsync(user, decodedToken, request.Model.NewPassword);

            if (result.Succeeded)
            {
                logger.LogInformation("Password reset successfully for: {Email}", request.Email);
                return new() { IsSuccess = true, Message = "Password has been reset successfully." };
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogWarning("Reset password failed for: {Email}. Errors: {Errors}", request.Email, errors);
            return new() { IsSuccess = false, Message = errors, ErrorType = "BadRequest" };
        }
    }
}
