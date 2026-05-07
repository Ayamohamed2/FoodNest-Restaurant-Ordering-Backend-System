using MediatR;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Account.DTOs.Acoount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.ChangePassword
{
    public class ChangePasswordCommand : IRequest<ServiceResult<string>>
    {
        public string UserId { get; set; }
        public ChangePasswordDTO Model { get; set; }

        public ChangePasswordCommand(string userId, ChangePasswordDTO model)
        {
            UserId = userId;
            Model = model;
        }
    }
}
