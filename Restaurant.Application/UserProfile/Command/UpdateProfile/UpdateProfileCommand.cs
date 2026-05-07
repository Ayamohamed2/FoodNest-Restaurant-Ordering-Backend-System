using MediatR;
using Microsoft.AspNetCore.Hosting;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.UserProfile.Dtos.Profie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.UserProfile.Command.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<ServiceResult<object>>
    {
        public string UserId { get; set; }
        public UserProfileDTO Profile { get; set; }
        public string BaseUrl { get; set; }
        public IWebHostEnvironment Env { get; set; }

        public UpdateProfileCommand(string userId, UserProfileDTO profile, string baseUrl, IWebHostEnvironment env)
        {
            UserId = userId;
            Profile = profile;
            BaseUrl = baseUrl;
            Env = env;
        }
    }
}
