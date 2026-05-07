using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Core.Models.Account;
using Restaurant.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Restaurant.Application.Account.Commend.Register
{
    public class RegisterCommandHandler :IRequestHandler<RegisterCommand,ServiceResult<string>>
    {
        private readonly IAuthRepository auth;
      
        private readonly ILogger<RegisterCommandHandler> logger;

        public RegisterCommandHandler(
            IAuthRepository auth,
            ILogger<RegisterCommandHandler> logger)
        {
            this.auth = auth;

            this.logger = logger;
        }

        public async Task<ServiceResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Register attempt for email: {Email}", request.Model.Email);

            var result = await auth.Register(request.Model,request.BaseUrl);

            if (result.Succeeded)
            {
                logger.LogInformation("User registered successfully: {Email}", request.Model.Email);
                return new() { IsSuccess = true, Message = "User registered successfully. pls confirm email" };
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogWarning("Register failed for email: {Email}. Errors: {Errors}", request.Model.Email, errors);
            return new() { IsSuccess = false, Message = errors, ErrorType = "BadRequest" };
        }
    }
}
