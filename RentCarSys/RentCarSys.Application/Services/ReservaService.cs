using AutoMapper;
using Localdorateste.Data;
using Localdorateste.Extensions;
using Localdorateste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCarSys.Application.DTO.ReservasDTOs;
using RentCarSys.Application.Extensions;
using RentCarSys.Application.Interfaces;
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

        public async Task<ResultViewModel<List<ReservaDTOGetAll>>> BuscarTodasReservas()
        {
            try
            {
                var reservas = await _repositorioReservas.ObterTodasReservasAsync();
                
                var reservaDto = _mapper.Map<List<ReservaDTOGetAll>>(reservas);
                return new ResultViewModel<List<ReservaDTOGetAll>>(reservaDto);
            }
            catch
            {
                return new ResultViewModel<List<ReservaDTOGetAll>>(erro: "05X05 - Falha interna no servidor!");
            }
        }

        public async Task<ResultViewModel<ReservaDTO>> BuscarReservaPorId(int reservaId)
        {
            try
            {
                var reserva = await _repositorioReservas.ObterReservaPorIdAsync(reservaId);
                if (reserva == null)
                {
                    return new ResultViewModel<ReservaDTO>(erro: "Reserva não encontrada, verifique se a reserva já foi cadastrada!");
                }

                var reservaDto = _mapper.Map<ReservaDTO>(reserva);
                return new ResultViewModel<ReservaDTO>(reservaDto);
            }
            catch
            {
                return new ResultViewModel<ReservaDTO>(erro: "Falha interna no servidor!");
            }
        }

        public async Task<ResultViewModel<ReservaDTO>> CriarReserva(ReservaDTOCreate model)
        {
            try
            {
                var cliente = await _repositorioClientes.ObterClientePorIdAsync(model.ClienteId);
                if (cliente == null)
                {
                    return new ResultViewModel<ReservaDTO>(erro: "Cliente não encontrado, verifique se o cliente já foi cadastrado!");
                }

                var veiculo = await _repositorioVeiculos.ObterVeiculoPorIdAsync(model.VeiculoId);
                if (veiculo == null)
                {
                    return new ResultViewModel<ReservaDTO>(erro: "Veiculo não encontrado, verifique se o veiculo já foi cadastrado!");
                }

                if (cliente.Status == ClienteStatus.Running)
                {
                    return new ResultViewModel<ReservaDTO>("Não foi possível alterar o cliente, possui reserva em andamento");
                }

                if (veiculo.Status == VeiculoStatus.Running)
                {
                    return new ResultViewModel<ReservaDTO>("Não foi possível alterar o veiculo, possui reserva em andamento");
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
                return new ResultViewModel<ReservaDTO>(reservaDto);

            }
            catch (DbUpdateException ex)
            {
                return new ResultViewModel<ReservaDTO>(erro: "05XE8 - Não foi possível criar a reserva!");
            }
            catch
            {
                return new ResultViewModel<ReservaDTO>(erro: "05X10 - Falha interna no servidor!");
            }
        }

        public async Task<ResultViewModel<ReservaDTOUpdate>> EditarReserva(int reservaId, ReservaDTOUpdate model)
        {

            try
            {
                var reserva = await _repositorioReservas.ObterReservaPorIdAsync(reservaId);
                if (reserva == null)
                {
                    return new ResultViewModel<ReservaDTOUpdate>("Cliente não encontrado!");
                }

                if (reserva.Status == ReservaStatus.Running)
                {
                    return new ResultViewModel<ReservaDTOUpdate>("Não foi possivel alterar a reserva, o veiculo já foi retirado e a reserva está em andamento!");
                }

                if (reserva.Status == ReservaStatus.Offline)
                {
                    return new ResultViewModel<ReservaDTOUpdate>("Não é possivel alterar uma reserva finalizada!");
                }

                reserva.DataRetirada = model.DataRetirada;
                reserva.DataEntrega = model.DataEntrega;
                reserva.ValorLocacao = model.ValorLocacao;

                var reservaDto = _mapper.Map<ReservaDTOUpdate>(reserva);

                await _repositorioReservas.AtualizarReservaAsync(reserva);
                
                return new ResultViewModel<ReservaDTOUpdate>(reservaDto);
            }
            catch (DbUpdateException ex)
            {
                return new ResultViewModel<ReservaDTOUpdate>(erro: "05XE8 - Não foi possível alterar a reserva!");
            }
            catch (Exception ex)
            {
                return new ResultViewModel<ReservaDTOUpdate>("05X11 - Falha interna no servidor!");
            }
        }

        public async Task<ResultViewModel<ReservaDTODelete>> ExcluirReserva(int reservaId)
        {
            try
            {
                var reserva = await _repositorioReservas.ObterReservaPorIdAsync(reservaId);
                if (reserva == null)
                {
                    return new ResultViewModel<ReservaDTODelete>("Reserva não encontrada!");
                }

                if (reserva.Status == ReservaStatus.Running)
                {
                    return new ResultViewModel<ReservaDTODelete>("Não foi possível excluir a reserva, possui reserva em andamento");
                }

                if (reserva.Status == ReservaStatus.Offline)
                {
                    return new ResultViewModel<ReservaDTODelete>("Não é possivel alterar uma reserva finalizada!");
                }

                var reservaDto = _mapper.Map<ReservaDTODelete>(reserva);

                await _repositorioReservas.ExcluirReservaAsync(reserva);

                return new ResultViewModel<ReservaDTODelete>(reservaDto);
            }
            catch
            {
                return new ResultViewModel<ReservaDTODelete>("05X12 - Falha interna no servidor!");
            }
        }
    }
}
