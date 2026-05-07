using MediatR;
using NEEFRA.Core.DTO.Service;
using Restaurant.Domain.DTO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.Login
{
    public class LoginCommand : IRequest<ServiceResult<object>>
    {
        public LoginDTO Model { get; set; }

        public LoginCommand(LoginDTO model)
        {
            Model = model;
        }
    }
}
