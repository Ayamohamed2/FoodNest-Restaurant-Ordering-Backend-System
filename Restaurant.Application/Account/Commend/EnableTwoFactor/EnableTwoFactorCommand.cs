using MediatR;
using NEEFRA.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.EnableTwoFactor
{
    public class EnableTwoFactorCommand : IRequest<ServiceResult<string>>
    {
        public string UserId { get; set; }

        public EnableTwoFactorCommand(string userId)
        {
            UserId = userId;
        }
    }
}
