using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Core.Helpers.EmailTemplate;
using Restaurant.Core.Models.Account;


namespace Restaurant.Application.Account.Commend.ChangeEmail
{
    public class ChangeEmailCommandHandler
    : IRequestHandler<ChangeEmailCommand, ServiceResult<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBackgroundJobClient _jobClient;
        private readonly ILogger<ChangeEmailCommandHandler> logger;

        public ChangeEmailCommandHandler(
            UserManager<ApplicationUser> userManager,
           IBackgroundJobClient _jobClien, ILogger<ChangeEmailCommandHandler>logger)
        {
            this.userManager = userManager;
            this._jobClient = _jobClien;
            this.logger = logger;
        }

        public async Task<ServiceResult<string>> Handle(
            ChangeEmailCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Change email request for userId: {UserId}", request.UserId);

            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                logger.LogWarning("Change email failed - user not found: {UserId}", request.UserId);
                return new() { IsSuccess = false, Message = "User not found", ErrorType = "BadRequest" };
            }

            var existingUser = await userManager.FindByEmailAsync(request.Model.Email);
            if (existingUser != null)
            {
                logger.LogWarning("Change email failed - email already in use: {Email}", request.Model.Email);
                return new() { IsSuccess = false, Message = "Email already in use", ErrorType = "BadRequest" };
            }

            var token = await userManager.GenerateChangeEmailTokenAsync(user, request.Model.Email);
            var confirmationLink = $"{request.BaseUrl}/api/v2/account/confirm-change-email?userId={user.Id}&newEmail={request.Model.Email}&token={Uri.EscapeDataString(token)}";
            var emailBody = EmailTemplates.ConfirmEmail(confirmationLink);

            _jobClient.Enqueue<IEmailSender>(sender =>
            sender.SendEmailAsync(user.Email, "Confirm your new email", emailBody));
          
            logger.LogInformation("Change email confirmation sent to: {NewEmail} for userId: {UserId}", request.Model.Email, request.UserId);
            return new() { IsSuccess = true, Message = "Confirmation email has been sent to your new address." };
        }
    }
}
