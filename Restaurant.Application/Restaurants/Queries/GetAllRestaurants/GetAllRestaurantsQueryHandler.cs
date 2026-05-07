using AutoMapper;
using BookFlightTickets.Core.Domain.Specifications;
using DTOs;
using MediatR;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Restaurants.Enum;
using Restaurant.Application.Restaurants.Enums;
using Restaurant.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurant.Core.Interfaces.IService.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Application.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, ServiceResult<List<RestaurantDto>>>
    {
        private readonly ILogger<GetAllRestaurantsQueryHandler> logger;
        private readonly IUnitOfWork unit;
        private readonly IMapper mapper;
        private readonly IRedisCacheService cache;

        public GetAllRestaurantsQueryHandler(ILogger<GetAllRestaurantsQueryHandler>logger,IUnitOfWork unit,IMapper mapper, IRedisCacheService cache)
        {
            this.logger = logger;
            this.unit = unit;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<ServiceResult<List<RestaurantDto>>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all restaurants");

            // 🔥 Searching (Criteria)
            var spec = new BaseSpecification<Restaurant.Core.Entity.Restaurant.Restaurant>(
                r => string.IsNullOrEmpty(request.Search) || r.Name.ToLower().Contains(request.Search.ToLower())
            );

            // Include
            spec.Includes.Add(r => r.Dishes);

            // 🔥 Sorting باستخدام Enum
            switch (request.SortField)
            {
                case RestaurantSortField.Name:
                    if (request.SortDirection == SortDirection.Asc)
                        spec.OrderByAsc(r => r.Name);
                    else
                        spec.OrderByDesc(r => r.Name);
                    break;

                case RestaurantSortField.Id:
                default:
                    if (request.SortDirection == SortDirection.Asc)
                        spec.OrderByAsc(r => r.Id);
                    else
                        spec.OrderByDesc(r => r.Id);
                    break;
            }

            // 🔥 Pagination
            spec.ApplyPaging((request.PageIndex - 1) * request.PageSize, request.PageSize);

            var restaurants = await unit.Restaurant.GetAllWithSpecAsync(spec);

            var restaurantsDtos = mapper.Map<List<RestaurantDto>>(restaurants);
            return new()
            {
                IsSuccess = true,
                Data = restaurantsDtos
            };

        }
    }
}
