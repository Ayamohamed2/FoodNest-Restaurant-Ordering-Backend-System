using BookFlightTickets.Core.Domain.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurant.Core.Interfaces.IService.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Application.Commands.DeleteRestaurant
{
    public class DeleteDishesForRestaurantCommandHandler : IRequestHandler<DeleteDishesForRestaurantCommand, ServiceResult<object>>
    {
        private readonly ILogger<DeleteDishesForRestaurantCommandHandler> logger;
        private readonly IUnitOfWork unit;
        private readonly IRedisCacheService cache;

        public DeleteDishesForRestaurantCommandHandler(ILogger<DeleteDishesForRestaurantCommandHandler> logger,IUnitOfWork unit,IRedisCacheService cache)
        {
            this.logger = logger;
            this.unit = unit;
            this.cache = cache;
        }
        public async Task<ServiceResult<object>> Handle(DeleteDishesForRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogWarning("Removing all dishes from restaurant: {RestaurantId}", request.RestaurantId);

            var spec = new BaseSpecification<Restaurant.Core.Entity.Restaurant.Dish>(r => r.RestaurantId == request.RestaurantId);

            var Dish = await unit.Dish.GetAllWithSpecAsync(spec);

            if(Dish == null)
            {
                return new ServiceResult<object>
                {
                    IsSuccess = false,
                    Message = "Dish not found"
                };
                

            }
            foreach(var  d in Dish)
            {
                await cache.RemoveAsync($"dish_{request.RestaurantId}_{d.Id}");

            }
          
            unit.Dish.RemoveRange(Dish);
            unit.save();
     

            return new()
            {
                IsSuccess = true,
                Message = "Dish deleted Successfully"
            };
        }
    }
}
