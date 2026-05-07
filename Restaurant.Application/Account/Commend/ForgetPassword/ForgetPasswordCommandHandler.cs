using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Account.DTOs.Email;
using Restaurant.Core.Helpers.EmailTemplate;
using Restaurant.Core.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.ForgetPassword
{
    public class ForgetPasswordCommandHandler: IRequestHandler<ForgetPasswordCommand, ServiceResult<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBackgroundJobClient jobClient;
  
        private readonly ILogger<ForgetPasswordCommandHandler> logger;

        public ForgetPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
           IBackgroundJobClient JobClient,
            ILogger<ForgetPasswordCommandHandler> logger)
        {
            this.userManager = userManager;
            jobClient = JobClient;
  
            this.logger = logger;
        }

        public async Task<ServiceResult<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Forget password request for: {Email}", request.Email.Email);

            var user = await userManager.FindByEmailAsync(request.Email.Email);
            if (user == null)
            {
                logger.LogWarning("Forget password failed - email not found: {Email}", request.Email.Email);
                return new() { IsSuccess = false, Message = "Email not found", ErrorType = "NotFound" };
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);
            var resetLink = $"{request.BaseUrl}/api/v2/account/reset-password?email={user.Email}&token={encodedToken}";
            var emailBody = EmailTemplates.ResetPassword(resetLink);
            jobClient.Enqueue<IEmailSender>(sender =>
        sender.SendEmailAsync(user.Email, "Reset Your Password", emailBody));
        


            logger.LogInformation("Password reset link sent to: {Email}", user.Email);
            return new() { IsSuccess = true, Message = "Reset password link has been sent to your email." };
        }
    }
}
