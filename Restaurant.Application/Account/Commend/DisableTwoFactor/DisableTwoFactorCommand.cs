using MediatR;
using NEEFRA.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.DisableTwoFactor
{
    public class DisableTwoFactorCommand : IRequest<ServiceResult<string>>
    {
        public string UserId { get; set; }

        public DisableTwoFactorCommand(string userId)
        {
            UserId = userId;
        }
    }
}
