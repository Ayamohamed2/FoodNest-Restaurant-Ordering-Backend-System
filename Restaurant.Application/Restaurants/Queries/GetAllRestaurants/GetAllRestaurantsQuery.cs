using DTOs;
using MediatR;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Restaurants.Enum;
using Restaurant.Application.Restaurants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQuery:IRequest<ServiceResult<List<RestaurantDto>>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public RestaurantSortField SortField { get; set; } = RestaurantSortField.Id;
        public SortDirection SortDirection { get; set; } = SortDirection.Asc;

    }
}
