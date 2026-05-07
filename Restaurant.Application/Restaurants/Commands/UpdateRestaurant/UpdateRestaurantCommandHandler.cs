using AutoMapper;
using BookFlightTickets.Core.Domain.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurant.Core.Interfaces.IService.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Application.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler: IRequestHandler<UpdateRestaurantCommand,ServiceResult<object>>
    {
        private readonly ILogger<UpdateRestaurantCommandHandler> logger;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unit;
        private readonly IRedisCacheService cache;

        public UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler>logger,IMapper mapper,IUnitOfWork unit,IRedisCacheService cache)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.unit = unit;
            this.cache = cache;
        }

        public async Task<ServiceResult<object>> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating restaurant with id: {RestaurantId} with {@UpdatedRestaurant}", request.Id, request);

            var spec = new BaseSpecification<Restaurant.Core.Entity.Restaurant.Restaurant>(r => r.Id == request.Id);
            var restaurant =await unit.Restaurant.GetEntityWithSpecAsync(spec);
            if (restaurant == null)
            {
                return new ()
                {
                    IsSuccess = false,
                    Message = "Restaurant not found"
                };


            }
            mapper.Map(request, restaurant);
            unit.Restaurant.Update(restaurant);
            unit.save();
            await cache.RemoveAsync($"restaurant_{restaurant.Id}");
          
            return new()
            {
                IsSuccess = true,
                Message = "Restaurant updated Successfully",
                Data=restaurant
            };
        }
    }
}
