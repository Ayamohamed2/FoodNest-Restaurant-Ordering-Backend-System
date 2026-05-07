using MediatR;
using NEEFRA.Core.DTO.Service;
using Restaurant.Application.Account.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.SendEmailConfirmation
{
    public class SendEmailConfirmationCommand : IRequest<ServiceResult<string>>
    {
        public EmailDTO Email { get; set; }
        public string BaseUrl { get; set; }

        public SendEmailConfirmationCommand(EmailDTO email, string baseUrl)
        {
            Email = email;
            BaseUrl = baseUrl;
        }
    }
}
