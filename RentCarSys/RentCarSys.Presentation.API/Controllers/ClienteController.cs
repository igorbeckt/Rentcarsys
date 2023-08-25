using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RentCarSys.Application.DTOs.ClientesDTO;
using RentCarSys.Application.Models;
using RentCarSys.Application.Services;
using System.Net;

namespace RentCarSys.Application.Controllers
{
    [ApiController]
    [Route("cliente")]
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
            var result = await _clienteService.BuscarTodosClientes();
            return Ok(result);
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
                return StatusCode(404, new { Erro = ex.Message });
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
                return StatusCode(404, new { Erro = ex.Message });
            }
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CriarClientes([FromBody] ClienteDTOCreate model)
        {
            var result = await _clienteService.CriarCliente(model);
            return Created($"clientes/{result.Id}", result);
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
                return StatusCode(405, new { Erro = ex.Message });
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
                return StatusCode(404, new { Erro = ex.Message });
            }
        }
    }

}
