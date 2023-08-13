using AutoMapper;
using Localdorateste.Extensions;
using Localdorateste.Models;
using Microsoft.AspNetCore.Mvc;
using RentCarSys.Application.DTO.ClienteDTOs;
using RentCarSys.Application.DTO.ClientesDTOs;
using RentCarSys.Application.Extensions;
using RentCarSys.Application.Interfaces;
using RentCarSys.Application.Services;

namespace RentCarSys.Application.Controllers
{
    [ApiController]
    [Route("/clientes")]
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
            if (result.Erros.Count > 0)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }

        [HttpGet("buscarPorId/{clienteid:int}")]
        public async Task<IActionResult> BuscarClientesId(
        [FromRoute] int clienteid)
        {
            var result = await _clienteService.BuscarClientePorId(clienteid);
            if (result.Erros.Count > 0)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }

        [HttpGet("buscarPorCpf/{cpf}")]
        public async Task<IActionResult> BuscarClientesCPF(
        [FromRoute] long cpf)
        {
            var result = await _clienteService.BuscarClientePorCPF(cpf);
            if (result.Erros.Count > 0)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CriarClientes(
        [FromBody] ClienteDTOCreate model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<ClienteDTOCreate>(ModelState.PegarErros()));
            }

            var result = await _clienteService.CriarCliente(model);

            if (result.Erros.Count > 0)
            {
                return StatusCode(500, result);
            }

            return Created($"clientes/{result.Dados.Id}", result);
        }

        [HttpPut("alterar/{clienteid:int}")]
        public async Task<IActionResult> EditarClientes(
        [FromRoute] int clienteid,
        [FromBody] ClienteDTOUpdate model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Cliente>(ModelState.PegarErros()));
            }


            var result = await _clienteService.EditarCliente(clienteid, model);

            if (result.Erros.Count > 0)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }

        [HttpDelete("excluir/{clienteid:int}")]
        public async Task<IActionResult> ExcluirClientes(
        [FromRoute] int clienteid)
        {
            var result = await _clienteService.ExcluirCliente(clienteid);

            if (result.Erros.Count > 0)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }
    }
}
