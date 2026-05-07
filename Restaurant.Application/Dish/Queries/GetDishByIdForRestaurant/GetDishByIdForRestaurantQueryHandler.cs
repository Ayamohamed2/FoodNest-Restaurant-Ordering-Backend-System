using AutoMapper;
using BookFlightTickets.Core.Domain.Specifications;
using DTOs;
using MediatR;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Dish.Dtos;
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
    public class GetDishByIdForRestaurantQueryHandler : IRequestHandler<GetDishByIdForRestaurantQuery, ServiceResult<DishDto>>
    {
        private readonly ILogger<GetDishByIdForRestaurantQueryHandler> logger;
        private readonly IUnitOfWork unit;
        private readonly IMapper mapper;
        private readonly IRedisCacheService cache;

        public GetDishByIdForRestaurantQueryHandler(ILogger<GetDishByIdForRestaurantQueryHandler> logger,IUnitOfWork unit,IMapper mapper,IRedisCacheService cache)
        {
            this.logger = logger;
            this.unit = unit;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<ServiceResult<DishDto>> Handle(GetDishByIdForRestaurantQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieving dish: {DishId}, for restaurant with id: {RestaurantId}",
                       request.DishId,
                       request.RestaurantId);
            
            var cacheKey = $"dish_{request.RestaurantId}_{request.DishId}";
            var cachedData = await cache.GetAsync<DishDto>(cacheKey);
            if (cachedData != null)
            {
                return new()
                {
                    IsSuccess = true,
                    Data = cachedData
                };
            }
            var spec = new BaseSpecification<Restaurant.Core.Entity.Restaurant.Restaurant>(r => r.Id == request.RestaurantId);
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
            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId);

            if (dish == null)
            {
                return new()
                {
                    IsSuccess = false,
                    Message = "Dish not found in the specified resaurant"
                };
            }
            var dishDto = mapper.Map<DishDto>(dish);

            await cache.SetAsync(cacheKey, dishDto, TimeSpan.FromMinutes(10));

            return new()
            {
                IsSuccess = true,
                Data = dishDto
            };
        }
    }
}
