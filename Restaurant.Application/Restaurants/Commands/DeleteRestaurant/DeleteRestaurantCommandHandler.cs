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
    public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand, ServiceResult<object>>
    {
        private readonly ILogger<DeleteRestaurantCommandHandler> logger;
        private readonly IUnitOfWork unit;
        private readonly IRedisCacheService cache;

        public DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger,IUnitOfWork unit,IRedisCacheService cache)
        {
            this.logger = logger;
            this.unit = unit;
            this.cache = cache;
        }
        public async Task<ServiceResult<object>> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting restaurant with id: {RestaurantId}", request.Id);

            var spec = new BaseSpecification<Restaurant.Core.Entity.Restaurant.Restaurant>(r => r.Id == request.Id);

            var restaurant =await unit.Restaurant.GetEntityWithSpecAsync(spec);

            if(restaurant==null)
            {
                return new ServiceResult<object>
                {
                    IsSuccess = false,
                    Message = "Restaurant not found"
                };
                

            }
            await cache.RemoveAsync($"restaurant_{restaurant.Id}");
       

            unit.Restaurant.Delete(restaurant);
            unit.save();

         
            return new()
            {
                IsSuccess = true,
                Message = "Restaurant deleted Successfully"
            };
        }
    }
}
