using AutoMapper;
using BookFlightTickets.Core.Domain.Specifications;
using DTOs;
using MediatR;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Dish.Dtos;
using Restaurant.Application.Dish.Enum;
using Restaurant.Application.Restaurants.Enums;
using Restaurant.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurant.Core.Interfaces.IService.Redis;
using Restaurant.Infrastructure.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Application.Queries.GetAllRestaurants
{
    public class GetDishesForRestaurantQueryHandler : IRequestHandler<GetDishesForRestaurantQuery, ServiceResult<List<DishDto>>>
    {
        private readonly ILogger<GetDishesForRestaurantQueryHandler> logger;
        private readonly IUnitOfWork unit;
        private readonly IMapper mapper;
        private readonly IRedisCacheService cache;

        public GetDishesForRestaurantQueryHandler(ILogger<GetDishesForRestaurantQueryHandler> logger,IUnitOfWork unit,IMapper mapper, IRedisCacheService cache)
        {
            this.logger = logger;
            this.unit = unit;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<ServiceResult<List<DishDto>>> Handle(GetDishesForRestaurantQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieving dishes for restaurant with id: {RestaurantId}", request.RestaurantId);


            var spec = new BaseSpecification<Restaurant.Core.Entity.Restaurant.Dish>(
       d =>
           d.RestaurantId == request.RestaurantId &&
           (string.IsNullOrEmpty(request.Search) ||
            d.Name.Contains(request.Search))
   );

         
            switch (request.SortField)
            {
                case DishSortField.Name:
                    if (request.SortDirection == SortDirection.Asc)
                        spec.OrderByAsc(d => d.Name);
                    else
                        spec.OrderByDesc(d => d.Name);
                    break;

                case DishSortField.Price:
                    if (request.SortDirection == SortDirection.Asc)
                        spec.OrderByAsc(d => d.Price);
                    else
                        spec.OrderByDesc(d => d.Price);
                    break;

                case DishSortField.Id:
                default:
                    if (request.SortDirection == SortDirection.Asc)
                        spec.OrderByAsc(d => d.Id);
                    else
                        spec.OrderByDesc(d => d.Id);
                    break;
            }

            // 📄 Pagination
            spec.ApplyPaging((request.PageIndex - 1) * request.PageSize, request.PageSize);

            var dishes = await unit.Dish
                                   .GetAllWithSpecAsync(spec);

            var dishesDtos = mapper.Map<List<DishDto>>(dishes);
            return new()
            {
                IsSuccess = true,
                Data = dishesDtos
            };

        }
    }
}
