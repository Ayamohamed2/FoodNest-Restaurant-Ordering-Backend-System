using MediatR;
using MediatR.Registration;
using NEEFRA.Core.DTO.Service;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommand : IRequest<ServiceResult<object>>
{
    public string UserEmail { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}
