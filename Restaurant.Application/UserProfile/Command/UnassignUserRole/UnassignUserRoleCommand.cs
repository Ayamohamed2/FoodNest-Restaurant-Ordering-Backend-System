using MediatR;
using NEEFRA.Core.DTO.Service;

namespace Restaurants.Application.Users.Commands.UnassignUserRole;

public class UnassignUserRoleCommand : IRequest<ServiceResult<object>>
{
    public string UserEmail { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}
