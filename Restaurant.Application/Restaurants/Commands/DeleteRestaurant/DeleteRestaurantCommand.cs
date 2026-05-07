using MediatR;
using NEEFRA.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommand(int id): IRequest<ServiceResult<object>>
    {
        public int Id { get; } = id;

    }
}
