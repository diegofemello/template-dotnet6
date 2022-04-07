using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Configuration;
using Application.Services.Interfaces;
using Application.DTO;
using Application.DTO.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.Generic;

namespace Api.Controllers
{
    [ApiController, Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <response code="200">usuário específico pelo id.</response>
        /// <response code="204">Nenhum usuário encontrado.</response>
        /// <response code="401">Token está inválido ou expirado.</response>
        /// <response code="403">O usuário não tem permissão para acessar este endpoint.</response>
        [Authorize, HttpGet("{id}")]
        [ProducesResponseType((200), Type = typeof(UserDTO))]
        [ProducesResponseType(204)]
        [ProducesResponseType((401), Type = typeof(ResponseEnvelope<>))]
        [ProducesResponseType((403), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            try
            {
                UserDTO user = await _userService.GetById(id);
                if (user == null) return NoContent();

                if (User.IsInRole("Cliente") && user.Email != User.Identity.Name)
                    return StatusCode(StatusCodes.Status403Forbidden,
                        "Você não tem permissão para acessar os dados deste usuário!");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar obter os dados do usuário. {ex.Message}");
            }
        }

        /// <response code="200">Uma lista de todos os usuários.</response>
        /// <response code="204">Nenhum usuário encontrado.</response>
        /// <response code="401">Token está inválido ou expirado.</response>
        [Authorize(Roles = "Admin"), HttpGet]
        [ProducesResponseType((200), Type = typeof(List<UserDTO>))]
        [ProducesResponseType(204)]
        [ProducesResponseType((401), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                List<UserDTO> users = await _userService.GetAll();
                if (users == null) return NoContent();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar obter os dados dos usuários. {ex.Message}");
            }
        }

        /// <response code="200">usuário específico pelo id.</response>
        /// <response code="204">Nenhum usuário encontrado.</response>
        /// <response code="401">Token está inválido ou expirado.</response>
        /// <response code="403">O usuário não tem permissão para acessar este endpoint.</response>
        [Authorize(Roles = "Admin"), HttpGet("Username/{userName}")]
        [ProducesResponseType((200), Type = typeof(UserDTO))]
        [ProducesResponseType(204)]
        [ProducesResponseType((401), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> GetUserByUserNameAsync(string userName)
        {
            try
            {
                UserDTO user = await _userService.GetByUserName(userName);
                if (user == null) return NoContent();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar obter os dados do usuário. {ex.Message}");
            }
        }

        /// <response code="201">Retorna o novo usuário cadastrado.</response>
        /// <response code="400">Objeto na requisição é nulo ou quando é informado email/senha de outro usuário.</response>
        /// <response code="401">Token está inválido ou expirado.</response>
        [AllowAnonymous, HttpPost]
        [ProducesResponseType((201), Type = typeof(UserDTO))]
        [ProducesResponseType(204)]
        [ProducesResponseType((400), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> Create(UserCreateDTO model)
        {
            try
            {
                if (model == null) return BadRequest("Requisição inválida");

                UserDTO userCheck = new()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                };

                UserDTO checkResult = await _userService.CheckExist(userCheck, Guid.Empty);

                if (checkResult != null)
                {
                    if (checkResult.Email.ToLower() == model.Email.ToLower())
                        return BadRequest($"O Email informado já está cadastrado.");

                    if (checkResult.UserName.ToLower() == model.UserName.ToLower())
                        return BadRequest($"O UserName informado já está cadastrado.");
                }

                UserDTO user = await _userService.Create(model);

                return StatusCode(StatusCodes.Status201Created, user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar usuário. {ex.Message}");
            }
        }

        /// <response code="200">Retorna usuário atualizado.</response>
        /// <response code="400">Objeto na requisição é nulo ou quando é informado dados já cadastrados.</response>
        /// <response code="401">Token está inválido ou expirado.</response>
        /// <response code="403">O usuário não tem acesso ao dado do usuário informado ou tenta alterar a role sem ter permissão.</response>
        /// <response code="404">Não foi encontrado nenhum usuário com o id informado.</response>
        [Authorize, HttpPut("{id}")]
        [ProducesResponseType((200), Type = typeof(UserDTO))]
        [ProducesResponseType(204)]
        [ProducesResponseType((400), Type = typeof(ResponseEnvelope<>))]
        [ProducesResponseType((401), Type = typeof(ResponseEnvelope<>))]
        [ProducesResponseType((403), Type = typeof(ResponseEnvelope<>))]
        [ProducesResponseType((404), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> Update(Guid id, UserUpdateDTO model)
        {
            try
            {
                if (model == null)
                    return BadRequest("Requisição inválida.");

                UserDTO user = await _userService.GetById(id);
                if (user == null) return NotFound();

                if (!User.IsInRole("Admin") && user.Email != User.Identity.Name)
                    return StatusCode(StatusCodes.Status403Forbidden,
                        "Você não tem permissão para acessar os dados deste usuário!");

                if (model.UserRole.ToString() != user.UserRole)
                {
                    if (!User.IsInRole("Admin"))
                        return StatusCode(StatusCodes.Status403Forbidden,
                            "Você não tem permissão para alterar a permissão deste usuário.");

                    if (!Enum.IsDefined(typeof(Domain.Model.UserRole), model.UserRole))
                        return BadRequest("Role inválida.");
                }

                UserDTO userCheck = new()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                };

                UserDTO checkResult = await _userService.CheckExist(userCheck, id);

                if (checkResult != null)
                {
                    if (checkResult.Email.ToLower() == model.Email.ToLower())
                        return BadRequest($"O Email informado já está cadastrado.");

                    if (checkResult.UserName.ToLower() == model.UserName.ToLower())
                        return BadRequest($"O Login informado já está cadastrado.");
                }

                UserDTO userReturn = await _userService.Update(id, model);
                return Ok(userReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar editar usuário. {ex.Message}");
            }
        }


        /// <response code="204">usuário foi deletado.</response>
        /// <response code="401">Token está inválido ou expirado.</response>
        /// <response code="404">Não foi encontrado nenhum usuário com o id informado.</response>
        [Authorize, HttpDelete("{id}")]
        [ProducesResponseType((204), Type = typeof(string))]
        [ProducesResponseType((400), Type = typeof(ResponseEnvelope<>))]
        [ProducesResponseType((401), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                UserDTO user = await _userService.GetById(id);
                if (user == null) return NotFound();

                return await _userService.Delete(id) ?
                    NoContent() :
                    throw new Exception("Ocorreu um erro.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Não foi possível deletar usuário. {ex.Message}");
            }
        }
    }
}
