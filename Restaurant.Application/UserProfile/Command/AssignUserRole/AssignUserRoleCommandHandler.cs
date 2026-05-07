using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NEEFRA.Core.DTO.Service;
using Restaurant.Core.Models.Account;


namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(ILogger<AssignUserRoleCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager) : IRequestHandler<AssignUserRoleCommand, ServiceResult<object>>
{
    public async Task<ServiceResult<object>> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Assigning user role: {@Request}", request);
        var user = await userManager.FindByEmailAsync(request.UserEmail);
        if (user == null)
        {
            return new()
            {
                IsSuccess = false,
                Message = "User not found"
            };
        }

        var role = await roleManager.FindByNameAsync(request.RoleName);
        if (role == null)
        {
            return new()
            {
                IsSuccess = false,
                Message = "Role not found"
            };
        }
        await userManager.AddToRoleAsync(user, role.Name!);
        return new()
        {
            IsSuccess = true,
            Message = "Role assigned successfully"
        };
    }
}
