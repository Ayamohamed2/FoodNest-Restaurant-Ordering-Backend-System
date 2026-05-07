using MediatR;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Restaurant.Application.Account.Commend.Logout
{

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ServiceResult<string>>
    {
        private readonly IJWT_TokenReposatory jwt_token;
        private readonly ILogger<LogoutCommandHandler> logger;

        public LogoutCommandHandler(IJWT_TokenReposatory jwtToken, ILogger<LogoutCommandHandler> logger)
        {
            jwt_token = jwtToken;
            this.logger = logger;
        }

        public async Task<ServiceResult<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Logout - revoking tokens");
            await jwt_token.RevokeAllTokens(request.Token);
            logger.LogInformation("Tokens revoked successfully");
            return new() { IsSuccess = true };
        }
    }
}
