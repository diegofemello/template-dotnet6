using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Api.Configuration;
using Domain.VO;
using Domain.VO.Request;
using Application.Services.Generic;

namespace API.Controllers
{
    [ApiController, Route("[controller]")]
    public class ExampleController : ControllerBase
    {
        private readonly IGenericService _genericService;

        public ExampleController(IGenericService genericService)
        {
            _genericService = genericService;
        }

        /// <response code="200">example específico pelo id.</response>
        /// <response code="204">Nenhum example encontrado.</response>
        /// <response code="401">Token está inválido ou expirado.</response>
        [Authorize, HttpGet("{id}")]
        [ProducesResponseType((200), Type = typeof(ExampleVO))]
        [ProducesResponseType(204)]
        [ProducesResponseType((401), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> GetByIdAsync([FromRoute]int id)
        {
            try
            {
                ExampleVO result = await _genericService.GetById<ExampleVO>(id);

                if (result == null) return NoContent();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao obter dados. {ex.Message}");
            }
        }

        /// <summary> Obtem todos os examples cadastrados. </summary>
        /// <response code="200">Uma lista de todos os examples.</response>
        /// <response code="204">Nenhum example encontrado.</response>
        /// <response code="401">Token está inválido ou expirado.</response>
        [Authorize, HttpGet]
        [ProducesResponseType((200), Type = typeof(List<ExampleVO>))]
        [ProducesResponseType(204)]
        [ProducesResponseType((401), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<ExampleVO> result = await _genericService.GetAll<ExampleVO>();
                if (result == null) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao obter dados. {ex.Message}");
            }
        }

        /// <summary> Cadastra example. </summary>
        /// <response code="201">Retorna o novo example cadastrado.</response>
        /// <response code="400">Objeto na requisição é nulo.</response>
        /// <response code="401">Token está inválido ou expirado.</response>
        [Authorize, HttpPost]
        [ProducesResponseType((201), Type = typeof(ExampleVO))]
        [ProducesResponseType((400), Type = typeof(ResponseEnvelope<>))]
        [ProducesResponseType((401), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> Create([FromBody]ExampleRequestVO body)
        {
            try
            {
                if (body == null) return BadRequest("Requisição inválida");

                ExampleVO result = await _genericService.Add(body);

                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar. {ex.Message}");
            }
        }

        /// <summary> Atualiza example. </summary>
        /// <response code="200">Retorna example atualizado.</response>
        /// <response code="400">Objeto na requisição é nulo.</response>
        /// <response code="401">Token está inválido ou expirado.</response>
        /// <response code="404">Não foi encontrado nenhum example com o id informado.</response>
        [Authorize, HttpPut("{id}")]
        [ProducesResponseType((200), Type = typeof(ExampleRequestVO))]
        [ProducesResponseType((400), Type = typeof(ResponseEnvelope<>))]
        [ProducesResponseType((401), Type = typeof(ResponseEnvelope<>))]
        [ProducesResponseType((404), Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] ExampleRequestVO body)
        {
            try
            {
                if (body == null) return BadRequest("Requisição inválida");

                ExampleVO check = await _genericService.GetById<ExampleVO>(id);

                if (check == null)
                    return NotFound();

                ExampleVO result = await _genericService.Update(id, body);
                if (result == null)
                    throw new Exception("Ocorreu um erro.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar. {ex.Message}");
            }
        }

        /// <summary> Deleta example. </summary>
        /// <response code="204">example foi deletado.</response>
        /// <response code="401">Token está inválido ou expirado.</response>
        /// <response code="404">Não foi encontrado nenhum example com o id informado.</response>
        [Authorize, HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401, Type = typeof(ResponseEnvelope<>))]
        [ProducesResponseType(404, Type = typeof(ResponseEnvelope<>))]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            try
            {
                ExampleVO result = await _genericService.GetById<ExampleVO>(id);
                if (result == null) return NotFound();

                return await _genericService.Delete<ExampleVO>(id) ?
                    NoContent() : throw new Exception("Ocorreu um erro.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Não foi possível deletar. {ex.Message}");
            }
        }

    }
}
