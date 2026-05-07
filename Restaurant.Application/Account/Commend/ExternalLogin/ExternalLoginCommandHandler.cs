using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Core.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Restaurant.Application.Account.Commend.ExternalLogin
{
    public class ExternalLoginCommandHandler: IRequestHandler<ExternalLoginCommand, ServiceResult<object>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJWT_TokenReposatory jwt_token;
        private readonly ILogger<ExternalLoginCommandHandler> logger;

        public ExternalLoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJWT_TokenReposatory jwt_token,
            ILogger<ExternalLoginCommandHandler> logger)
        {
            this.userManager = userManager;
            this.jwt_token = jwt_token;
            this.logger = logger;
        }

        public async Task<ServiceResult<object>> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("External login callback for provider: {Provider}", request.Provider);

            var result = await request.HttpContext.AuthenticateAsync("ExternalCookies");
            if (!result.Succeeded)
            {
                logger.LogWarning("External authentication failed for provider: {Provider}",request.Provider );
                return new() { IsSuccess = false, Message = "External authentication failed", ErrorType = "BadRequest" };
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var providerKey = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                logger.LogWarning("External login failed - no email provided by provider: {Provider}", request.Provider);
                return new() { IsSuccess = false, Message = "Email not provided by provider", ErrorType = "BadRequest" };
            }

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                logger.LogInformation("Creating new user from external login: {Email}", email);

                user = new ApplicationUser { UserName = email, Email = email, Name = name, EmailConfirmed = true };
                var createResult = await userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    logger.LogWarning("External login - user creation failed for: {Email}. Errors: {Errors}", email, errors);
                    return new() { IsSuccess = false, Message = errors, ErrorType = "BadRequest" };
                }

                var addLoginResult = await userManager.AddLoginAsync(user, new UserLoginInfo(request.Provider, providerKey, request.Provider));
                if (!addLoginResult.Succeeded)
                {
                    logger.LogWarning("External login - failed to link provider {Provider} for: {Email}", request.Provider, email);
                    return new() { IsSuccess = false, Message = "Failed to link external login", ErrorType = "BadRequest" };
                }
            }
            else
            {
                var logins = await userManager.GetLoginsAsync(user);
                var isLinked = logins.Any(l => l.LoginProvider == request.Provider && l.ProviderKey == providerKey);
                if (!isLinked)
                {
                    logger.LogWarning("External login - email {Email} already registered with another method", email);
                    return new() { IsSuccess = false, Message = "This email is already registered with another method.", ErrorType = "BadRequest" };
                }
            }

            string jwtTokenId = $"JTI{Guid.NewGuid()}";
            var accessToken = await jwt_token.GenerateToken(user, jwtTokenId);
            var refreshToken = jwt_token.CreateNewRefreshToken(user.Id, jwtTokenId);

            logger.LogInformation("External login successful for: {Email} via {Provider}", email, request.Provider);
            return new() { IsSuccess = true, Data = (object)new { email, accessToken, refreshToken } };
        }
    }
}
