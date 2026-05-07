using MediatR;
using NEEFRA.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.ConfirmChangeEmail
{
    public class ConfirmChangeEmailCommand : IRequest<ServiceResult<string>>
    {
        public string UserId { get; set; }
        public string NewEmail { get; set; }
        public string Token { get; set; }

        public ConfirmChangeEmailCommand(string userId, string newEmail, string token)
        {
            UserId = userId;
            NewEmail = newEmail;
            Token = token;
        }
    }
}
