using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCarSys.Application.DTO.ReservasDTOs;
using RentCarSys.Application.Interfaces;
using RentCarSys.Application.Models;
using RentCarSys.Application.Models.Enums;

namespace RentCarSys.Application.Services
{
    public class ReservaService
    {
        private readonly IClientesRepository _repositorioClientes;
        private readonly IVeiculosRepository _repositorioVeiculos;
        private readonly IReservasRepository _repositorioReservas;
        private readonly IMapper _mapper;

        public ReservaService(IClientesRepository repositorioClientes,
                              IVeiculosRepository repositorioVeiculos,
                              IReservasRepository repositorioReservas,
                              IMapper mapper)
        {
            _repositorioClientes = repositorioClientes;
            _repositorioVeiculos = repositorioVeiculos;
            _repositorioReservas = repositorioReservas;
            _mapper = mapper;
        }

        public async Task<List<ReservaDTOGetAll>> BuscarTodasReservas()
        {
            var reservas = await _repositorioReservas.ObterTodasReservasAsync();

            var reservaDto = _mapper.Map<List<ReservaDTOGetAll>>(reservas);
            return reservaDto;
        }

        public async Task<ReservaDTO> BuscarReservaPorId(int reservaId)
        {
            var reserva = await _repositorioReservas.ObterReservaPorIdAsync(reservaId);
            if (reserva == null)
            {
                throw new Exception("Reserva não encontrada, verifique se a reserva já foi cadastrada!");
            }

            var reservaDto = _mapper.Map<ReservaDTO>(reserva);
            return reservaDto;
        }

        public async Task<ReservaDTO> CriarReserva(ReservaDTOCreate model)
        {
            var cliente = await _repositorioClientes.ObterClientePorIdAsync(model.ClienteId);
            if (cliente == null)
            {
                throw new Exception("Cliente não encontrado, verifique se o cliente já foi cadastrado!");
            }

            var veiculo = await _repositorioVeiculos.ObterVeiculoPorIdAsync(model.VeiculoId);
            if (veiculo == null)
            {
                throw new Exception("Veiculo não encontrado, verifique se o veiculo já foi cadastrado!");
            }

            if (cliente.Status == ClienteStatus.Running || veiculo.Status == VeiculoStatus.Running)
            {
                throw new Exception("Não foi possível criar a reserva, cliente ou veículo possui reserva em andamento");
            }

            var reserva = new Reserva
            {
                Status = ReservaStatus.Online,
                DataReserva = model.DataReserva,
                ValorLocacao = model.ValorLocacao,
                DataRetirada = model.DataRetirada,
                DataEntrega = model.DataRetirada,
                Cliente = new List<Cliente> { cliente },
                Veiculo = new List<Veiculo> { veiculo }
            };

            await _repositorioReservas.AdicionarReservaAsync(reserva);

            var reservaDto = _mapper.Map<ReservaDTO>(reserva);
            return reservaDto;
        }

        public async Task<ReservaDTOUpdate> EditarReserva(int reservaId, ReservaDTOUpdate model)
        {
            var reserva = await _repositorioReservas.ObterReservaPorIdAsync(reservaId);
            if (reserva == null)
            {
                throw new Exception("Reserva não encontrada!");
            }

            if (reserva.Status == ReservaStatus.Running)
            {
                throw new Exception("Não foi possivel alterar a reserva, o veiculo já foi retirado e a reserva está em andamento!");
            }

            if (reserva.Status == ReservaStatus.Offline)
            {
                throw new Exception("Não é possivel alterar uma reserva finalizada!");
            }

            reserva.DataRetirada = model.DataRetirada;
            reserva.DataEntrega = model.DataEntrega;
            reserva.ValorLocacao = model.ValorLocacao;

            await _repositorioReservas.AtualizarReservaAsync(reserva);

            var reservaDto = _mapper.Map<ReservaDTOUpdate>(reserva);
            return reservaDto;
        }

        public async Task<ReservaDTODelete> ExcluirReserva(int reservaId)
        {
            var reserva = await _repositorioReservas.ObterReservaPorIdAsync(reservaId);
            if (reserva == null)
            {
                throw new Exception("Reserva não encontrada!");
            }

            if (reserva.Status == ReservaStatus.Running)
            {
                throw new Exception("Não foi possível excluir a reserva, possui reserva em andamento");
            }

            if (reserva.Status == ReservaStatus.Offline)
            {
                throw new Exception("Não é possivel alterar uma reserva finalizada!");
            }

            await _repositorioReservas.ExcluirReservaAsync(reserva);

            var reservaDto = _mapper.Map<ReservaDTODelete>(reserva);
            return reservaDto;
        }
    }

}
