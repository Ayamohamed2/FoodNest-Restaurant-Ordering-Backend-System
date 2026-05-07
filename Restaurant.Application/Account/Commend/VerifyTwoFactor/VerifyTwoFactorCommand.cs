using MediatR;
using NEEFRA.Core.DTO.Service;
using Restaurant.Domain.DTO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.VerifyTwoFactor
{
    public class VerifyTwoFactorCommand : IRequest<ServiceResult<TokenDTO>>
    {
        public string UserId { get; set; }
        public string Code { get; set; }

        public VerifyTwoFactorCommand(string userId, string code)
        {
            UserId = userId;
            Code = code;
        }
    }
}
