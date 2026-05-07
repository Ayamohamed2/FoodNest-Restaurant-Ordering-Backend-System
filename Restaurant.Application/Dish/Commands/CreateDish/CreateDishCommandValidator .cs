using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Restaurant.Commands.CreateRestaurant
{
    public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
    {
        private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];

        public CreateDishCommandValidator()
        {
            RuleFor(dish => dish.Price)
      .GreaterThanOrEqualTo(0)
      .WithMessage("Price must be a non-negative number.");


            RuleFor(dish => dish.KiloCalories)
                .GreaterThanOrEqualTo(0)
                .WithMessage("KiloCalories must be a non-negative number.");
        }
        }
    }
