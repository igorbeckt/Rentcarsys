using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RentCarSys.Application.DTO.ClientesDTOs;
using RentCarSys.Application.Interfaces;
using RentCarSys.Application.Models;
using RentCarSys.Application.Services;
using RentCarSys.Application.Services.RentCarSys.Application.Services;

namespace RentCarSys.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet("buscarTodos")]
        public async Task<IActionResult> BuscarClientes()
        {
            try
            {
                var result = await _clienteService.BuscarTodosClientes();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

        [HttpGet("buscarPorId/{clienteid:int}")]
        public async Task<IActionResult> BuscarClientesId([FromRoute] int clienteid)
        {
            try
            {
                var result = await _clienteService.BuscarClientePorId(clienteid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

        [HttpGet("buscarPorCpf/{cpf}")]
        public async Task<IActionResult> BuscarClientesCPF([FromRoute] long cpf)
        {
            try
            {
                var result = await _clienteService.BuscarClientePorCPF(cpf);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CriarClientes([FromBody] ClienteDTOCreate model)
        {
            try
            {
                var result = await _clienteService.CriarCliente(model);
                return Created($"clientes/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

        [HttpPut("alterar/{clienteid:int}")]
        public async Task<IActionResult> EditarClientes([FromRoute] int clienteid, [FromBody] ClienteDTOUpdate model)
        {
            try
            {
                var result = await _clienteService.EditarCliente(clienteid, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

        [HttpDelete("excluir/{clienteid:int}")]
        public async Task<IActionResult> ExcluirClientes([FromRoute] int clienteid)
        {
            try
            {
                var result = await _clienteService.ExcluirCliente(clienteid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = ex.Message });
            }
        }
    }

}
