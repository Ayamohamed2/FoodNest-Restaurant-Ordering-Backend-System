using MediatR;
using NEEFRA.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Restaurant.Commands.CreateRestaurant
{
    public class CreateDishCommand : IRequest<ServiceResult<object>>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }

        public int? KiloCalories { get; set; }
        public int RestaurantId { get; set; }
    }
}
