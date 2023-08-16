using Microsoft.AspNetCore.Mvc;
using RentCarSys.Application.DTO.VeiculosDTOs;
using RentCarSys.Application.Interfaces;
using RentCarSys.Application.Models;
using RentCarSys.Application.Repository;
using RentCarSys.Application.Services;

namespace RentCarSys.Application.Controllers
{
    [ApiController]
    [Route("/veiculos")]
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
            try
            {
                var result = await _veiculoService.BuscarTodosVeiculos();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
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
                return StatusCode(500, new { erro = ex.Message });
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
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CriarVeiculo([FromBody] VeiculoDTOCreate model)
        {
            try
            {
                var result = await _veiculoService.CriarVeiculo(model);
                return Created($"/veiculos/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
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
                return StatusCode(500, new { erro = ex.Message });
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
                return StatusCode(500, new { erro = ex.Message });
            }
        }
    }

}