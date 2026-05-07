using DTOs;
using MediatR;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Dish.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Restaurants.Queries.GetRestaurantById
{
    public class GetDishByIdForRestaurantQuery(int restaurantId, int dishId) : IRequest<ServiceResult<DishDto>>
    {
        public int RestaurantId { get; } = restaurantId;
        public int DishId { get; } = dishId;
    }
}
