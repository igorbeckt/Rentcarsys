using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCarSys.Application.DTO.ReservasDTOs;
using RentCarSys.Application.Interfaces;
using RentCarSys.Application.Services;

namespace RentCarSys.Application.Controllers
{
    [ApiController]
    [Route("reservas")]
    public class ReservaController : ControllerBase
    {
        private readonly ClienteService _clienteService;
        private readonly VeiculoService _veiculoService;
        private readonly ReservaService _reservaService;

        public ReservaController(ClienteService clienteService,
                                 VeiculoService veiculoService,
                                 ReservaService reservaService)
        {
            _clienteService = clienteService;
            _veiculoService = veiculoService;
            _reservaService = reservaService;
        }

        [HttpGet("buscarTodas")]
        public async Task<IActionResult> BuscarReservas()
        {
            var result = await _reservaService.BuscarTodasReservas();
            return Ok(result);
        }

        [HttpGet("buscarPorId/{reservaid:int}")]
        public async Task<IActionResult> BuscarReservaId(int reservaid)
        {
            try
            {
                var result = await _reservaService.BuscarReservaPorId(reservaid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(404, new { Erro = ex.Message });
            }
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CriarReservas([FromBody] ReservaDTOCreate model)
        {
            var result = await _reservaService.CriarReserva(model);
            return Created($"reservas/{result.Id}", result);
        }

        [HttpPut("alterar/{reservaid:int}")]
        public async Task<IActionResult> EditarReserva(int reservaid, [FromBody] ReservaDTOUpdate model)
        {
            try
            {
                var result = await _reservaService.EditarReserva(reservaid, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(405, new { Erro = ex.Message });
            }
        }

        [HttpDelete("excluir/{reservaid:int}")]
        public async Task<IActionResult> ExcluirReserva(int reservaid)
        {
            try
            {
                var result = await _reservaService.ExcluirReserva(reservaid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(405, new { Erro = ex.Message });
            }
        }
    }

}
