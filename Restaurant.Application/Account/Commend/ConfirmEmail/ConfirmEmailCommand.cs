using MediatR;
using NEEFRA.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<ServiceResult<string>>
    {
        public string UserId { get; set; }
        public string Token { get; set; }

        public ConfirmEmailCommand(string userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }
}
