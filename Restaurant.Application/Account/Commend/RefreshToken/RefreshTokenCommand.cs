using MediatR;
using NEEFRA.Core.DTO.Service;
using Restaurant.Domain.DTO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.RefreshToken
{
    public class RefreshTokenCommand : IRequest<ServiceResult<TokenDTO>>
    {
        public TokenDTO Token { get; set; }

        public RefreshTokenCommand(TokenDTO token)
        {
            Token = token;
        }
    }
}
