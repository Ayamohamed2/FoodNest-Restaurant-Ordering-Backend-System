using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NEEFRA.API.Helpers;
using NEEFRA.Core.DTO.Service;
using Restaurant.API.Controllers;
using Restaurant.Application.UserProfile.Command.UpdateProfile;
using Restaurant.Application.UserProfile.Dtos.Profie;
using Restaurant.Application.UserProfile.Queries.GetProfile;
using Restaurant.Core.Interfaces.IService;
using Restaurant.Core.Models.Account;
using Restaurant.Domain.Constants;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.UnassignUserRole;
using System.Security.Claims;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Villa_API_Project.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    [Authorize]
    public class UserProfileController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;

        public UserProfileController(IMediator mediator, IWebHostEnvironment env)
        {
            _mediator = mediator;
            _env = env;
        }



        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _mediator.Send(
                new GetProfileQuery(UserId, BaseUrl));

            return HandleResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromForm] UserProfileDTO profileDTO)
        {
            var result = await _mediator.Send(
                new UpdateProfileCommand(UserId, profileDTO, BaseUrl, _env));

            return HandleResult(result);
        }

        [HttpPost("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
        {
            var result=await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UnassignUserRole(UnassignUserRoleCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
}

