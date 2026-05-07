using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using NEEFRA.API.Helpers;
using NEEFRA.Core.DTO.Service;
using Restaurant.API.Controllers;
using Restaurant.Application.Account.Commend.ChangeEmail;
using Restaurant.Application.Account.Commend.ChangePassword;
using Restaurant.Application.Account.Commend.ConfirmChangeEmail;
using Restaurant.Application.Account.Commend.ConfirmEmail;
using Restaurant.Application.Account.Commend.DisableTwoFactor;
using Restaurant.Application.Account.Commend.EnableTwoFactor;
using Restaurant.Application.Account.Commend.ExternalLogin;
using Restaurant.Application.Account.Commend.ForgetPassword;
using Restaurant.Application.Account.Commend.Login;
using Restaurant.Application.Account.Commend.Logout;
using Restaurant.Application.Account.Commend.RefreshToken;
using Restaurant.Application.Account.Commend.Register;
using Restaurant.Application.Account.Commend.ResetPassword;
using Restaurant.Application.Account.Commend.SendEmailConfirmation;
using Restaurant.Application.Account.Commend.VerifyTwoFactor;
using Restaurant.Application.Account.DTOs.Acoount;
using Restaurant.Application.Account.DTOs.Email;
using Restaurant.Core.Interfaces.IService;
using Restaurant.Core.Models.Account;
using Restaurant.Domain.DTO.Account;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Villa_API_Project.DataAccess.Reposatory.IReposatory;
using Villa_API_Project.Models;

namespace Villa_API_Project.Controllers.V2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]

    public class AccountController : BaseController
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            var result = await _mediator.Send(
                new RegisterCommand(model, BaseUrl));

            return HandleResult(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var result = await _mediator.Send(
                new LoginCommand(model));

            return HandleResult(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenDTO dto)
        {
            var result = await _mediator.Send(
                new RefreshTokenCommand(dto));

            return HandleResult(result);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] TokenDTO dto)
        {
            var result = await _mediator.Send(
                new LogoutCommand(dto));

            return HandleResult(result);
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _mediator.Send(
                new ConfirmEmailCommand(userId, token));

            return HandleResult(result);
        }

        [HttpPost("email-confirmation")]
        public async Task<IActionResult> EmailForConfirmation(EmailDTO email)
        {
            var result = await _mediator.Send(
                new SendEmailConfirmationCommand(email, BaseUrl));

            return HandleResult(result);
        }


        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(EmailDTO model)
        {
            var result = await _mediator.Send(
                new ForgetPasswordCommand(model, BaseUrl));

            return HandleResult(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string token, ResetPasswordDTO model)
        {
            var result = await _mediator.Send(
                new ResetPasswordCommand(email, token, model));

            return HandleResult(result);
        }

        // ExternalLogin لازم يفضل في الكنترولر لأن Challenge() مش ممكن يتعمل في السيرفس
        [HttpGet("external-login")]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = $"{Request.Scheme}://{Request.Host}/api/v2/Account/external-login-callback?provider={provider}";
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }

        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback(string provider)
        {
            var result = await _mediator.Send(
                new ExternalLoginCommand(provider, HttpContext));

            return HandleResult(result);
        }

        [Authorize]
        [HttpPost("change-email")]
        public async Task<IActionResult> ChangeEmail(EmailDTO model)
        {
            var result = await _mediator.Send(
                new ChangeEmailCommand(UserId, model, BaseUrl));

            return HandleResult(result);
        }

        [HttpGet("confirm-change-email")]
        public async Task<IActionResult> ConfirmChangeEmail(string userId, string newEmail, string token)
        {
            var result = await _mediator.Send(
                new ConfirmChangeEmailCommand(userId, newEmail, token));

            return HandleResult(result);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            var result = await _mediator.Send(
                new ChangePasswordCommand(UserId, model));

            return HandleResult(result);
        }

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FA(string code, string userId)
        {
            var result = await _mediator.Send(
                new VerifyTwoFactorCommand(userId, code));

            return HandleResult(result);
        }


        [Authorize]
        [HttpPost("enable-2fa")]
        public async Task<IActionResult> Enable2FA()
        {
            var result = await _mediator.Send(
                new EnableTwoFactorCommand(UserId));

            return HandleResult(result);
        }


        [Authorize]
        [HttpPost("disable-2fa")]
        public async Task<IActionResult> Disable2FA()
        {
            var result = await _mediator.Send(
                new DisableTwoFactorCommand(UserId));

            return HandleResult(result);
        }

    }
}
