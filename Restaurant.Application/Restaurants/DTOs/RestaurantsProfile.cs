using Restaurant.Application.Restaurant.Commands.CreateRestaurant;

using AutoMapper;
using Restaurant.Application.Restaurants.Commands.UpdateRestaurant;
namespace DTOs
{
    public class RestaurantsProfile : Profile
    {
        public RestaurantsProfile()
        {

            CreateMap<CreateRestaurantCommand, Restaurant.Core.Entity.Restaurant.Restaurant>()
                .ForMember(d => d.Address, opt => opt.MapFrom(
                    src => new Restaurant.Core.Entity.Restaurant.Address
                    {
                        City = src.City,
                        PostalCode = src.PostalCode,
                        Street = src.Street
                    }));

            CreateMap<UpdateRestaurantCommand, Restaurant.Core.Entity.Restaurant.Restaurant>();
               
            CreateMap<Restaurant.Core.Entity.Restaurant.Restaurant, RestaurantDto>()
                .ForMember(d => d.City, opt =>
                    opt.MapFrom(src => src.Address == null ? null : src.Address.City))
                .ForMember(d => d.PostalCode, opt =>
                    opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
                .ForMember(d => d.Street, opt =>
                    opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
                .ForMember(d => d.Dishes, opt => opt.MapFrom(src => src.Dishes));
        }
    }
 }
