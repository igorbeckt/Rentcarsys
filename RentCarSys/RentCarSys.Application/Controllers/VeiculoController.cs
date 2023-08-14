using Microsoft.AspNetCore.Mvc;
using RentCarSys.Application.DTO.VeiculosDTOs;
using RentCarSys.Application.Extensions;
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
            var result = await _veiculoService.BuscarTodosVeiculos();
            if (result.Erros.Count > 0)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }


        [HttpGet("buscarPorId/{veiculoid:int}")]
        public async Task<IActionResult> BuscarVeiculoPorId(
        [FromRoute] int veiculoid)
        {
            var result = await _veiculoService.BuscarVeiculoPorId(veiculoid);
            if (result.Erros.Count > 0)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }

        [HttpGet("buscarPorPlaca/{placa}")]
        public async Task<IActionResult> BuscarVeiculoPorPlaca(
        [FromRoute] string placa)
        {
            var result = await _veiculoService.BuscarVeiculoPorPlaca(placa);
            if (result.Erros.Count > 0)
            {
                return StatusCode(500, result);
            }

            return Ok(result);

        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CriarVeiculo(
        [FromBody] VeiculoDTOCreate model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<VeiculoDTOCreate>(ModelState.PegarErros()));
            }

            var result = await _veiculoService.CriarVeiculo(model);

            if (result.Erros.Count > 0)
            {
                return StatusCode(500, result);
            }

            return Created($"veiculos/{result.Dados.Id}", result);
        }

        [HttpPut("alterar/{veiculoid:int}")]
        public async Task<IActionResult> EditarVeiculo(
        [FromRoute] int veiculoid,
        [FromBody] VeiculoDTOUpdate model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Veiculo>(ModelState.PegarErros()));
            }


            var result = await _veiculoService.EditarVeiculo(veiculoid, model);

            if (result.Erros.Count > 0)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }

        [HttpDelete("excluir/{veiculoid:int}")]
        public async Task<IActionResult> ExcluirVeiculo(
        [FromRoute] int veiculoid)
        {
            var result = await _veiculoService.ExcluirVeiculo(veiculoid);

            if (result.Erros.Count > 0)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }
    }
}
