using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Core.Models.Account;
using Restaurant.Domain.DTO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Restaurant.Application.Account.Commend.VerifyTwoFactor
{
    public class VerifyTwoFactorCommandHandler
     : IRequestHandler<VerifyTwoFactorCommand, ServiceResult<TokenDTO>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJWT_TokenReposatory jwt_token;
        private readonly ILogger<VerifyTwoFactorCommandHandler> logger;

        public VerifyTwoFactorCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJWT_TokenReposatory jwt_token,
            ILogger<VerifyTwoFactorCommandHandler> logger)
        {
            this.userManager = userManager;
            this.jwt_token = jwt_token;
            this.logger = logger;
        }

        public async Task<ServiceResult<TokenDTO>> Handle(
            VerifyTwoFactorCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Verify 2FA attempt for userId: {UserId}", request.UserId);

            var user = await userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                logger.LogWarning("Verify 2FA failed - user not found: {UserId}", request.UserId);
                return new() { IsSuccess = false, Message = "User not found" };
            }

            var isValid = await userManager.VerifyTwoFactorTokenAsync(
                user,
                TokenOptions.DefaultEmailProvider,
                request.Code);

            if (!isValid)
            {
                logger.LogWarning("Verify 2FA failed - invalid code for userId: {UserId}", request.UserId);
                return new() { IsSuccess = false, Message = "Invalid Code" };
            }

            logger.LogInformation("2FA verified successfully for userId: {UserId}", request.UserId);

            string jwtTokenId = $"JTI{Guid.NewGuid()}";

            var accessToken = await jwt_token.GenerateToken(user, jwtTokenId);
            var refreshToken = jwt_token.CreateNewRefreshToken(user.Id, jwtTokenId);

            return new()
            {
                IsSuccess = true,
                Data = new TokenDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }
            };
        }
    }
}
