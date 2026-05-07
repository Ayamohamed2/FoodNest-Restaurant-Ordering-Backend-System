using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Restaurant.Commands.CreateRestaurant;
using Restaurant.Core.Interfaces.IService.Redis;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Application.Commands.CreateRestaurant
{
    public class CreateDishCommandHandler : IRequestHandler<CreateDishCommand, ServiceResult<object>>
    {
        private readonly ILogger<CreateDishCommandHandler> logger;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unit;
        private readonly IRedisCacheService cache;

        public CreateDishCommandHandler(ILogger<CreateDishCommandHandler> logger,
    IMapper mapper,IUnitOfWork unit,IRedisCacheService cache)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.unit = unit;
            this.cache = cache;
        }

        public async Task<ServiceResult<object>> Handle(CreateDishCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating a new Dish {@Dish}", request);

            var restaurant = await unit.Restaurant.GetByIdAsync(request.RestaurantId);
            if (restaurant == null)
            {
                return new()
                {
                    IsSuccess = true,
                    Message = "restaurant not found"
                };
            }
            var dish = mapper.Map<Restaurant.Core.Entity.Restaurant.Dish>(request);
        
            unit.Dish.Create(dish);
            unit.save();
         
  
            return new()
            {
                IsSuccess = true,
                Message = "Dish created successfully"
            };
        }
    }
}
