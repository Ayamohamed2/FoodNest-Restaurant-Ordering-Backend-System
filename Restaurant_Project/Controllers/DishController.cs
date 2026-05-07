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

namespace Restaurant.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]

    public class DishController : BaseController
    {
        private readonly IMediator mediator;

        public DishController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetAll/{restaurantId}")]
        public async Task<IActionResult> GetAll([FromRoute] int restaurantId, GetDishesForRestaurantQuery query)
        {
            query.RestaurantId = restaurantId;

            var results = await mediator.Send(query);

            return HandleResult(results);
        }

        [HttpGet("{restaurantId}/{dishId}")]
        public async Task<IActionResult> GetById([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var results = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
            return HandleResult(results);


        }


        [HttpDelete("{restaurantId}")]

        public async Task<IActionResult> Delete([FromRoute] int restaurantId)
        {
            var reults = await mediator.Send(new DeleteDishesForRestaurantCommand(restaurantId));

            return HandleResult(reults);
        }

        [HttpPost("{restaurantId}")]
        public async Task<IActionResult> Create([FromRoute] int restaurantId, CreateDishCommand command)
        {
            command.RestaurantId = restaurantId;
            var results = await mediator.Send(command);
            return HandleResult(results);
        }
    }
}
