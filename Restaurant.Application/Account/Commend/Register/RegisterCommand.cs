using MediatR;
using NEEFRA.Core.DTO.Service;
using Restaurant.Domain.DTO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.Register
{
    public class RegisterCommand:IRequest<ServiceResult<string>>
    {
        public RegisterDTO Model { get; set; }
        public string BaseUrl { get; set; }

        public RegisterCommand(RegisterDTO model, string baseUrl)
        {
            Model = model;
            BaseUrl = baseUrl;
        }
    }
}
