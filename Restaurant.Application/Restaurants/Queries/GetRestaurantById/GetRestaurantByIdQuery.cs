using DTOs;
using MediatR;
using NEEFRA.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Restaurants.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQuery(int id) : IRequest<ServiceResult<RestaurantDto>>
    {
        public int Id { get; } = id;
    }
}
