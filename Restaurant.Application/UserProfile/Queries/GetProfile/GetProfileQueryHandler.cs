using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Core.Interfaces.IService.Redis;
using Restaurant.Core.Models.Account;
using Restaurant.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Restaurant.Application.UserProfile.Queries.GetProfile
{
    public class GetProfileQueryHandler
    : IRequestHandler<GetProfileQuery, ServiceResult<object>>
    {
        private readonly IUnitOfWork unit;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<GetProfileQueryHandler> logger;
        private readonly IRedisCacheService cache;

        public GetProfileQueryHandler(IUnitOfWork unit, UserManager<ApplicationUser> userManager, ILogger<GetProfileQueryHandler> logger, IRedisCacheService cache)
        {
            this.unit = unit;
            this.userManager = userManager;
            this.logger = logger;
            this.cache = cache;
        }

        public async Task<ServiceResult<object>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Get profile for userId: {UserId}", request.UserId);
            var cachekey = $"Profile:{request.UserId}";
            ApplicationUser user = await cache.GetAsync<ApplicationUser>(cachekey);
            if (user == null)
            {
                user = await userManager.FindByIdAsync(request.UserId);
                var profile = new
                {
                    user.Email,
                    user.Name,
                    user.PhoneNumber,
                    user.ImageURL
                };
                await cache.SetAsync(cachekey, profile, TimeSpan.FromMinutes(30));


            }

            if (user == null)
            {
                logger.LogWarning("Get profile failed - user not found: {UserId}", request.UserId);
                return new() { IsSuccess = false, Message = "User not found", ErrorType = "BadRequest" };
            }

            var imageUrl = string.IsNullOrEmpty(user.ImageURL) ? null : request.BaseUrl + user.ImageURL;


            logger.LogDebug("Profile retrieved successfully for userId: {UserId}", request.UserId);
            return new()
            {
                IsSuccess = true,
                Data = (object)new
                {
                    user.Email,
                    user.Name,
                    user.PhoneNumber,
                    imageUrl
                }
            };
        }
    }
}
