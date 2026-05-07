using MediatR;
using NEEFRA.Core.DTO.Service;
using Restaurant.Domain.DTO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.Logout
{
    public class LogoutCommand : IRequest<ServiceResult<string>>
    {
        public TokenDTO Token { get; set; }

        public LogoutCommand(TokenDTO token)
        {
            Token = token;
        }
    }
}
