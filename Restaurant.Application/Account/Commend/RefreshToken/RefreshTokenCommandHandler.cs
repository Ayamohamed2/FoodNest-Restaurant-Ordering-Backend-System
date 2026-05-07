using MediatR;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Domain.DTO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Restaurant.Application.Account.Commend.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ServiceResult<TokenDTO>>
    {
        private readonly IJWT_TokenReposatory jwt_token;
        private readonly ILogger<RefreshTokenCommandHandler> logger;

        public RefreshTokenCommandHandler(IJWT_TokenReposatory jwtToken, ILogger<RefreshTokenCommandHandler> logger)
        {
            jwt_token = jwtToken;
            this.logger = logger;
        }

        public async Task<ServiceResult<TokenDTO>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Token refresh attempt");

            var tokenDTOResponse = await jwt_token.RefreshAccessToken(request.Token);

            if (tokenDTOResponse == null || string.IsNullOrEmpty(tokenDTOResponse.AccessToken))
            {
                logger.LogWarning("Token refresh failed - invalid token");
                return new() { IsSuccess = false, Message = "Token Invalid", ErrorType = "BadRequest" };
            }

            logger.LogDebug("Token refreshed successfully");
            return new() { IsSuccess = true, Data = tokenDTOResponse };
        }
    }
}
