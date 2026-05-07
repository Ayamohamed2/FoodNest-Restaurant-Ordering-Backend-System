using AutoMapper;
using BookFlightTickets.Core.Domain.Specifications;
using DTOs;
using MediatR;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Restaurants.Queries.GetRestaurantById;
using Restaurant.Core.Interfaces.IService.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Application.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQueryHandler:IRequestHandler<GetRestaurantByIdQuery,ServiceResult<RestaurantDto>>
    {
        private readonly ILogger<GetRestaurantByIdQueryHandler> logger;
        private readonly IUnitOfWork unit;
        private readonly IMapper mapper;
        private readonly IRedisCacheService cache;

        public GetRestaurantByIdQueryHandler(ILogger<GetRestaurantByIdQueryHandler>logger,IUnitOfWork unit,IMapper mapper,IRedisCacheService cache)
        {
            this.logger = logger;
            this.unit = unit;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<ServiceResult<RestaurantDto>> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting restaurant {RestaurantId}", request.Id);
            var cacheKey = $"restaurant_{request.Id}";
            var cachedData = await cache.GetAsync<RestaurantDto>(cacheKey);
            if (cachedData != null)
            {
                return new()
                {
                    IsSuccess = true,
                    Data = cachedData
                };
            }
            var spec = new BaseSpecification<Restaurant.Core.Entity.Restaurant.Restaurant>(r => r.Id == request.Id);
            spec.Includes.Add(r => r.Dishes);
            var restaurant = await unit.Restaurant.GetEntityWithSpecAsync(spec);


            if (restaurant == null)
            {
                return new()
                {
                    IsSuccess = false,
                    Message = "Restaurant not found"
                
                };


            }

            var restaurantDto = mapper.Map<RestaurantDto>(restaurant);

            await cache.SetAsync(cacheKey, restaurantDto, TimeSpan.FromMinutes(10));

            return new()
            {
                IsSuccess = true,
                Data = restaurantDto
            };
        }
    }
}
