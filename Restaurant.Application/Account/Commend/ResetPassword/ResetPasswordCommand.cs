using MediatR;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Account.DTOs.Acoount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.ResetPassword
{
    public class ResetPasswordCommand : IRequest<ServiceResult<string>>
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public ResetPasswordDTO Model { get; set; }

        public ResetPasswordCommand(string email, string token, ResetPasswordDTO model)
        {
            Email = email;
            Token = token;
            Model = model;
        }
    }
}
