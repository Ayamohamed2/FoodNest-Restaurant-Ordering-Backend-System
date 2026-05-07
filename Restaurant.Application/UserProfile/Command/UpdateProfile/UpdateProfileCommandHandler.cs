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

namespace Restaurant.Application.UserProfile.Command.UpdateProfile
{
    public class UpdateProfileCommandHandler
     : IRequestHandler<UpdateProfileCommand, ServiceResult<object>>
    {
        private readonly IUnitOfWork unit;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<UpdateProfileCommandHandler> logger;
        private readonly IRedisCacheService cache;

        public UpdateProfileCommandHandler(IUnitOfWork unit, UserManager<ApplicationUser> userManager, ILogger<UpdateProfileCommandHandler> logger, IRedisCacheService cache)
        {
            this.unit = unit;
            this.userManager = userManager;
            this.logger = logger;
            this.cache = cache;
        }

        public async Task<ServiceResult<object>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Update profile for userId: {UserId}", request.UserId);

            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                logger.LogWarning("Update profile failed - user not found: {UserId}", request.UserId);
                return new() { IsSuccess = false, Message = "User not found", ErrorType = "BadRequest" };
            }

            if (request.Profile.Name != null)
                user.Name = request.Profile.Name;

            user.PhoneNumber = request.Profile.phoneNumber;
            user.DateOfBirth = request.Profile.DateOfBirth;
            user.Nationality = request.Profile.Nationality;

            if (request.Profile.imagefile != null && request.Profile.imagefile.Length > 0)
            {
                logger.LogDebug("Updating profile image for userId: {UserId}", request.UserId);
                if (user.ImageURL != "/Images/default.png")
                    unit.User.DeleteImageMethod(user.ImageURL, request.Env);
                user.ImageURL = unit.User.GetImageURL(request.Profile.imagefile, user.Id, request.Env);
            }

            await userManager.UpdateAsync(user);

            var imageUrl = string.IsNullOrEmpty(user.ImageURL) ? null :request.BaseUrl  + user.ImageURL;

            logger.LogInformation("Profile updated successfully for userId: {UserId}", request.UserId);

            await cache.RemoveAsync($"Profile:{request.UserId}");
            return new()
            {
                IsSuccess = true,
                Data = (object)new
                {
                    user.Email,
                    user.Name,
                    user.PhoneNumber,
                    user.DateOfBirth,
                    user.Nationality,
                    imageUrl
                }
            };
        }
    }
}
