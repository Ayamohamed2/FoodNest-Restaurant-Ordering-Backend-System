using AutoMapper;
using BookFlightTickets.Core.Domain.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Restaurant.Commands.CreateRestaurant;
using Restaurant.Core.Interfaces.IService.Redis;
using Restaurant.Core.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;

namespace Application.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, ServiceResult<object>>
    {
        private readonly ILogger<CreateRestaurantCommandHandler> logger;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unit;
        private readonly IRedisCacheService cache;

        public CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper,IUnitOfWork unit,IRedisCacheService cache)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.unit = unit;
            this.cache = cache;
        }

        public async Task<ServiceResult<object>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating a new restaurant {@Restaurant}", request);

            var restaurant = mapper.Map<Restaurant.Core.Entity.Restaurant.Restaurant>(request);
            ApplicationUser owner = await unit.User.GetEntityWithSpecAsync(new BaseSpecification<ApplicationUser>(u=>u.Id==request.UserId));
            restaurant.Owner = owner;
            restaurant.OwnerId = request.UserId;
            unit.Restaurant.Create(restaurant);
            unit.save();
       
            return new()
            {
                IsSuccess = true,
                Message = "Restaurant created successfully"
            };
        }
    }
}
