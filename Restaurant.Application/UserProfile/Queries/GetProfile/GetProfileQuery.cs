using MediatR;
using NEEFRA.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.UserProfile.Queries.GetProfile
{
    public class GetProfileQuery : IRequest<ServiceResult<object>>
    {
        public string UserId { get; set; }
        public string BaseUrl { get; set; }

        public GetProfileQuery(string userId, string baseUrl)
        {
            UserId = userId;
            BaseUrl = baseUrl;
        }
    }
}
