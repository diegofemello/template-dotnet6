using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Configuration;
using Application.Services.Interfaces;
using Application.DTO;
using Application.DTO.Request;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Api.Controllers
{
    [ApiController, Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        /// <response code="200">Retorna usuário logado e token.</response>
        /// <response code="400">Email não está valido ou requisição está vazia.</response>
        /// <response code="401">Token está inválido ou expirado.</response>
        [AllowAnonymous, HttpPost("Signin")]
        [ProducesResponseType((200), Type = typeof(TokenDTO))]
        [ProducesResponseType((400), Type = typeof(ResponseEnvelope<>))]
        [ProducesResponseType((401), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> Signin([FromBody] AuthRequestDTO body)
        {
            try
            {
                if (body == null) return BadRequest("Requisição inválida");
                TokenDTO token = await _authService.ValidateCredentials(body);

                if (token == null) return Unauthorized();

                if (!token.User.EmailConfirmed)
                {
                    return BadRequest("Email não está validado");
                }

                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar logar. {ex.Message}");
            }
        }

        /// <response code="200">Retorna usuário atualizado.</response>
        /// <response code="400">Objeto na requisição é nulo.</response>
        /// <response code="401">Token está inválido ou expirado.</response>
        [AllowAnonymous, HttpPost("Refresh")]
        [ProducesResponseType((200), Type = typeof(TokenDTO))]
        [ProducesResponseType((400), Type = typeof(ResponseEnvelope<>))]
        [ProducesResponseType((401), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDTO body)
        {
            try
            {
                if (body is null) return BadRequest("Requisição inválida");

                var accessToken = HttpContext.GetTokenAsync("access_token")?.Result;
                if (accessToken is null) return Unauthorized();
                
                TokenDTO token = await _authService.ValidateCredentials(accessToken, body.RefreshToken);

                return token == null ? Unauthorized("Token Invalido ou expirado") : Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar o token. {ex.Message}");
            }
        }


        /// <response code="204">Token foi revogado.</response>
        /// <response code="401">Não foi informado um token.</response>
        [Authorize, HttpGet("Signout")]
        [ProducesResponseType(204)]
        [ProducesResponseType((400), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> Revoke()
        {
            string username = User.Identity.Name;
            await _authService.RevokeToken(username);

            return NoContent();
        }

        /// <response code="200">Retorna usuário atualizado.</response>
        /// <response code="400">Objeto na requisição é nulo.</response>
        /// <response code="404">Não foi encontrado nenhum usuário com o email informado.</response>
        [AllowAnonymous, HttpPost("GenerateEmailConfirmation")]
        [ProducesResponseType((200), Type = typeof(ResponseEnvelope<>))]
        [ProducesResponseType((400), Type = typeof(ResponseEnvelope<>))]
        [ProducesResponseType((404), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> GenerateEmailConfirm([FromBody] EmailRequestDTO body)
        {
            try
            {
                if (body == null) return BadRequest("Requisição inválida");

                UserDTO user = await _userService.GetByEmail(body.Email);
                if (user == null) return NotFound();

                if (user.EmailConfirmed && body.Email == user.Email)
                {
                    EmailConfirmDTO emailConfirm = new()
                    {
                        Email = body.Email,
                        EmailSent = false,
                        IsConfirmed = true
                    };

                    return Ok(emailConfirm);
                }
                await _authService.GenerateEmailConfirmationByUser(user);

                return Ok("Email de confirmação enviado.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar enviar email de confirmação. {ex.Message}");
            }
        }

        /// <response code="200">Retorna se o email foi confirmado.</response>
        /// <response code="400">Objeto na requisição é nulo ou o token informado é inválido.</response>
        [AllowAnonymous, HttpPost("ConfirmEmail")]
        [ProducesResponseType((200), Type = typeof(EmailConfirmDTO))]
        [ProducesResponseType((400), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmRequestDTO body)
        {
            try
            {
                if (body is null) return BadRequest("Requisição inválida");

                if (!string.IsNullOrEmpty(body.Token))
                {
                    UserDTO checkConfirm = await _userService
                        .ConfirmEmailAsync(body.UserUid, body.Token);

                    if (checkConfirm != null)
                    {
                        EmailConfirmDTO result = new()
                        {
                            Email = checkConfirm.Email,
                            IsConfirmed = true
                        };
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest("Token inválido.");
                    }
                }
                return BadRequest("Requisição inválida.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar confirmar email. {ex.Message}");
            }
        }
        /// <response code="200">Enviado email de recuperação de senha.</response>
        /// <response code="404">Não foi encontrado nenhum usuário com o email informado.</response>
        [AllowAnonymous, HttpPost("GenerateEmailForgotPassword")]
        [ProducesResponseType((200), Type = typeof(ResponseEnvelope<>))]
        [ProducesResponseType((404), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> GenerateEmailForgot([FromBody]EmailRequestDTO body)
        {
            try
            {
                UserDTO user = await _userService.GetByEmail(body.Email);
                if (user == null) return NotFound();

                await _authService.GenerateForgotPasswordTokenAsync(user);


                return Ok("Email de recuperação de senha enviado com sucesso!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar enviar email de recuperação. {ex.Message}");
            }
        }

        /// <response code="200">Retorna usuário com senha atualizada atualizado.</response>
        /// <response code="400">Objeto na requisição é nulo.</response>
        [AllowAnonymous, HttpPost("ResetPassword")]
        [ProducesResponseType((200), Type = typeof(UserDTO))]
        [ProducesResponseType((400), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDTO body)
        {
            try
            {
                if (body == null) return BadRequest("Requisição inválida");
                UserDTO result = await _userService.ResetPasswordAsync(body);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar resetar senha. {ex.Message}");
            }
        }
    }
}