using DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Restaurant.Commands.CreateRestaurant;
using Restaurant.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurant.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurant.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurant.Application.Restaurants.Queries.GetRestaurantById;
using Restaurant.Domain.Constants;

namespace Restaurant.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]

    public class RestaurantController : BaseController
    {
        private readonly IMediator mediator;

        public RestaurantController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("All_Restaurans")]
        public async Task<IActionResult> GetAll(GetAllRestaurantsQuery query)
        {
            var results = await mediator.Send(query);

            return HandleResult(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var results = await mediator.Send(new GetRestaurantByIdQuery(id));
            return HandleResult(results);


        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, UpdateRestaurantCommand command)
        {

            command.Id = id;

            var results = await mediator.Send(command);

            return HandleResult(results);
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
        {
            var reults = await mediator.Send(new DeleteRestaurantCommand(id));

            return HandleResult(reults);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Owner)]

        public async Task<IActionResult> CreateRestaurant(CreateRestaurantCommand command)
        {
            var results = await mediator.Send(command);
            return HandleResult(results);
        }
    }
}
