using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Core.Helpers.EmailTemplate;
using Restaurant.Core.Models.Account;
using Restaurant.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Restaurant.Application.Account.Commend.Login
{
    internal class LoginCommandHandler : IRequestHandler<LoginCommand, ServiceResult<object>>
    {
        private readonly IAuthRepository auth;
        private readonly IBackgroundJobClient jobClient;
        private readonly IJWT_TokenReposatory jwt_token;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<LoginCommandHandler> logger;

        public LoginCommandHandler(
            IUnitOfWork unit,
            IAuthRepository auth,
            IBackgroundJobClient _jobClient,
            IJWT_TokenReposatory jwt_token,
            UserManager<ApplicationUser> userManager,
            ILogger<LoginCommandHandler> logger)
        {
            this.auth = auth;
            jobClient = _jobClient;
            this.jwt_token = jwt_token;
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<ServiceResult<object>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Login attempt for email: {Email}", request.Model.Email);

            var token = await auth.LoginAsync(request.Model);

            if (token.Message == "null")
            {
                logger.LogWarning("Login failed - invalid credentials for email: {Email}", request.Model.Email);
                return new() { IsSuccess = false, Message = "Invalid username or password.", ErrorType = "Unauthorized" };
            }

            if (token.Message == "EmailNotConfirmed")
            {
                logger.LogWarning("Login failed - email not confirmed for: {Email}", request.Model.Email);
                return new() { IsSuccess = false, Message = "EmailNotConfirmed", ErrorType = "BadRequest" };
            }

            if (token.Message == "LockedOut")
            {
                logger.LogWarning("Login failed - account locked out for: {Email}", request.Model.Email);
                return new() { IsSuccess = false, Message = "Account LockedOut for 1 Minute", ErrorType = "BadRequest" };
            }
            var user = await userManager.FindByEmailAsync(request.Model.Email);
            if (user.TwoFactorEnabled)
            {
                logger.LogInformation("2FA enabled - sending OTP to email: {Email}", request.Model.Email);

                var code = await userManager.GenerateTwoFactorTokenAsync(
                    user,
                    TokenOptions.DefaultEmailProvider);
                logger.LogInformation("OTO Code : " + code);
                var emailBody = EmailTemplates.OtpCode(code);
                jobClient.Enqueue<IEmailSender>(sender =>
            sender.SendEmailAsync(user.Email, "Your Verification Code", emailBody));

                logger.LogInformation("OTP sent successfully to: {Email}", request.Model.Email);

                return new()
                {
                    IsSuccess = true,
                    Message = $"OTP Sent",

                    Data = new
                    {
                        UserId = user.Id
                    }

                };
            }
            logger.LogInformation("Login successful for email: {Email}", request.Model.Email);
            return new() { IsSuccess = true, Data = token };
        }
    }
}
