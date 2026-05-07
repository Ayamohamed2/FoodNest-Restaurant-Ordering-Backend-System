using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Core.Helpers.EmailTemplate;
using Restaurant.Core.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.SendEmailConfirmation
{
    public class SendEmailConfirmationCommandHandler : IRequestHandler<SendEmailConfirmationCommand, ServiceResult<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBackgroundJobClient jobClient;
        private readonly ILogger<SendEmailConfirmationCommandHandler> logger;

        public SendEmailConfirmationCommandHandler(
            UserManager<ApplicationUser> userManager,
          IBackgroundJobClient _jobClient,
            ILogger<SendEmailConfirmationCommandHandler> logger)
        {
            this.userManager = userManager;
            jobClient = _jobClient;
            this.logger = logger;
        }

        public async Task<ServiceResult<string>> Handle(SendEmailConfirmationCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Resend email confirmation for: {Email}", request.Email.Email);

            var user = await userManager.FindByEmailAsync(request.Email.Email);
            if (user == null)
            {
                logger.LogWarning("Resend confirmation failed - email not found: {Email}", request.Email.Email);
                return new() { IsSuccess = false, Message = "Email not Found", ErrorType = "BadRequest" };
            }

            var emailtoken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"{request.BaseUrl}/api/v2/account/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(emailtoken)}";
            var emailBody = EmailTemplates.ConfirmEmail(confirmationLink);
            jobClient.Enqueue<IEmailSender>(sender =>
            sender.SendEmailAsync(user.Email, "Resend EmailConfirmation", emailBody));
    

            logger.LogInformation("Confirmation email sent to: {Email}", user.Email);
            return new() { IsSuccess = true, Message = "Email sent successfully please confirm your email" };
        }
    }

 }
