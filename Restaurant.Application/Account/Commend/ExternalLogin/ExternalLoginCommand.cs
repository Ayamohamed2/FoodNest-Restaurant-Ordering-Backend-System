using MediatR;
using Microsoft.AspNetCore.Http;
using NEEFRA.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Account.Commend.ExternalLogin
{
    public class ExternalLoginCommand : IRequest<ServiceResult<object>>
    {
        public string Provider { get; set; }
        public HttpContext HttpContext { get; set; }

        public ExternalLoginCommand(string provider, HttpContext httpContext)
        {
            Provider = provider;
            HttpContext = httpContext;
        }
    }
}
