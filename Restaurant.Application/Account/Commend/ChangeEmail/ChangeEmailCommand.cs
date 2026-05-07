using MediatR;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Account.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.ChangeEmail
{
    public class ChangeEmailCommand : IRequest<ServiceResult<string>>
    {
        public string UserId { get; set; }
        public EmailDTO Model { get; set; }
        public string BaseUrl { get; set; }

        public ChangeEmailCommand(string userId, EmailDTO model, string baseUrl)
        {
            UserId = userId;
            Model = model;
            BaseUrl = baseUrl;
        }
    }
}
