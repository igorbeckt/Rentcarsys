using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RentCarSys.Application.DTOs.VeiculosDTO;
using RentCarSys.Application.Models;
using RentCarSys.Application.Services;
using System.Net;

namespace RentCarSys.Application.Controllers
{
    [ApiController]
    [Route("veiculo")]
    public class VeiculoController : ControllerBase
    {
        private readonly VeiculoService _veiculoService;

        public VeiculoController(VeiculoService veiculoService)
        {
            _veiculoService = veiculoService;
        }

        [HttpGet("buscarTodos")]
        public async Task<IActionResult> BuscarVeiculos()
        {
            var result = await _veiculoService.BuscarTodosVeiculos();
            return Ok(result);
        }

        [HttpGet("buscarPorId/{veiculoid:int}")]
        public async Task<IActionResult> BuscarVeiculoPorId([FromRoute] int veiculoid)
        {
            try
            {
                var result = await _veiculoService.BuscarVeiculoPorId(veiculoid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(404, new { erro = ex.Message });
            }
        }

        [HttpGet("buscarPorPlaca/{placa}")]
        public async Task<IActionResult> BuscarVeiculoPorPlaca([FromRoute] string placa)
        {
            try
            {
                var result = await _veiculoService.BuscarVeiculoPorPlaca(placa);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(404, new { erro = ex.Message });
            }
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CriarVeiculo([FromBody] VeiculoDTOCreate model)
        {
            var result = await _veiculoService.CriarVeiculo(model);
            return Created($"/veiculos/{result.Id}", result);
        }

        [HttpPut("alterar/{veiculoid:int}")]
        public async Task<IActionResult> EditarVeiculo([FromRoute] int veiculoid, [FromBody] VeiculoDTOUpdate model)
        {
            try
            {
                var result = await _veiculoService.EditarVeiculo(veiculoid, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(405, new { erro = ex.Message });
            }
        }

        [HttpDelete("excluir/{veiculoid:int}")]
        public async Task<IActionResult> ExcluirVeiculo([FromRoute] int veiculoid)
        {
            try
            {
                var result = await _veiculoService.ExcluirVeiculo(veiculoid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(404, new { erro = ex.Message });
            }
        }
    }

}