using MediatR;
using NEEFRA.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteDishesForRestaurantCommand(int restaurantId) : IRequest<ServiceResult<object>>
    {
        public int RestaurantId { get; } = restaurantId;

    }
}
