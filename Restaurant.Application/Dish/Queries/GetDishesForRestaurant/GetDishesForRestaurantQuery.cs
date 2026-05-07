using DTOs;
using MediatR;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Dish.Dtos;
using Restaurant.Application.Dish.Enum;
using Restaurant.Application.Restaurants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetDishesForRestaurantQuery : IRequest<ServiceResult<List<DishDto>>>
    {
        public int RestaurantId { get; set; } 
        public string? Search { get; set; }

        // 📄 Pagination
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // 🔃 Sorting
        public DishSortField SortField { get; set; } = DishSortField.Id;
        public SortDirection SortDirection { get; set; } = SortDirection.Asc;

    }
}
