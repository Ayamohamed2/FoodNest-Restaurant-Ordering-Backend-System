using AutoMapper;
using Restaurant.Application.Dish.Dtos;
using Restaurant.Application.Restaurant.Commands.CreateRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dish.Dtos
{
    public class DishesProfile : Profile
    {
        public DishesProfile()
        {
            CreateMap<CreateDishCommand, Restaurant.Core.Entity.Restaurant.Dish>();

            CreateMap<Restaurant.Core.Entity.Restaurant.Dish, DishDto>();
        }
    }
}
