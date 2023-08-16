using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using RentCarSys.Application.DTO.AutoMapper;
using RentCarSys.Application.DTO.ClientesDTOs;
using RentCarSys.Application.DTO.ReservasDTOs;
using RentCarSys.Application.Interfaces;
using RentCarSys.Application.Models;
using RentCarSys.Application.Models.Enums;
using RentCarSys.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarSys.Test.UnitTest.Services
{
    public class ReservaServiceTest
    {
        protected Mock<IClientesRepository> clientesRepository = new Mock<IClientesRepository>();
        protected Mock<IVeiculosRepository> veiculosRepository = new Mock<IVeiculosRepository>();
        protected Mock<IReservasRepository> reservasRepository = new Mock<IReservasRepository>();
        protected IMapper mapper;
        private readonly ReservaService reservaService;

        public ReservaServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntitiesDTOMappingProfile());
            });
            mapper = config.CreateMapper();
            reservaService = new ReservaService(clientesRepository.Object, veiculosRepository.Object, reservasRepository.Object, mapper);
        }

        [Fact]
        public async void BuscarTodasReservas_Fail()
        {
            reservasRepository.Setup(repo => repo.ObterTodasReservasAsync())
                             .Throws(new Exception("Exceção simulada"));            

            await Assert.ThrowsAsync<Exception>(async () => await reservaService.BuscarTodasReservas());
        }

        [Fact]
        public async void BuscarTodasReservas_Success()
        {
            var mockReservas = MockData.ReservaMockData.ReservasGetAll();

            reservasRepository.Setup(repo => repo.ObterTodasReservasAsync())
                 .ReturnsAsync(mockReservas);


            var result = await reservaService.BuscarTodasReservas();

            Assert.NotNull(result);
            Assert.IsType<List<ReservaDTOGetAll>>(result);
            Assert.Equal(mockReservas.Count, result.Count);
        }

        [Fact]
        public async void BuscarReservaPorId_Fail()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            int reservaId = mockReserva.Id;

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .Throws(new Exception("Exceção simulada"));          


            await Assert.ThrowsAsync<Exception>(async () => await reservaService.BuscarReservaPorId(reservaId));
        }

        [Fact]
        public async void BuscarReservaPorId_ReservaNula()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            int reservaId = mockReserva.Id;

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync((Reserva)null);

            await Assert.ThrowsAsync<Exception>(async () => await reservaService.BuscarReservaPorId(reservaId));
        }

        [Fact]
        public async void BuscarReservaPorId_Success()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            int reservaId = mockReserva.Id;

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync(mockReserva);

            var result = await reservaService.BuscarReservaPorId(reservaId);

            Assert.NotNull(result);
            Assert.IsType<ReservaDTO>(result);
        }

        [Fact]
        public async Task CriarReserva_ClienteNaoEncontrado()
        {
            var mockReservaDTOCreate = MockData.ReservaMockData.ReservaCreate();
            var clienteId = mockReservaDTOCreate.ClienteId;

            clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                             .ReturnsAsync((Cliente)null);

            await Assert.ThrowsAsync<Exception>(async () => await reservaService.CriarReserva(mockReservaDTOCreate));
        }

        [Fact]
        public async Task CriarReserva_VeiculoNaoEncontrado()
        {
            var mockReservaCreate = MockData.ReservaMockData.ReservaCreate();
            var clienteId = mockReservaCreate.ClienteId;
            var veiculoId = mockReservaCreate.VeiculoId;

            clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(mockReservaCreate.ClienteId))
                             .ReturnsAsync(new Cliente());
            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(mockReservaCreate.VeiculoId))
                             .ReturnsAsync((Veiculo)null);

            await Assert.ThrowsAsync<Exception>(async () => await reservaService.CriarReserva(mockReservaCreate));
        }


        [Fact]
        public async Task CriarReservaFail_ClienteRunnig()
        {
            var mockReservaCreate = MockData.ReservaMockData.ReservaCreate();
            var mockStatus = new Cliente { Status = ClienteStatus.Running };

            clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(mockReservaCreate.ClienteId))
                             .ReturnsAsync(mockStatus);
            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(mockReservaCreate.VeiculoId))
                             .ReturnsAsync(new Veiculo());
                       

            await Assert.ThrowsAsync<Exception>(async () => await reservaService.CriarReserva(mockReservaCreate));
        }

        [Fact]
        public async Task CriarReservaFail_VeiculoRunnig()
        {
            var mockReservaCreate = MockData.ReservaMockData.ReservaCreate();
            var mockStatus = new Veiculo { Status = VeiculoStatus.Running };

            clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(mockReservaCreate.ClienteId))
                             .ReturnsAsync(new Cliente());
            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(mockReservaCreate.VeiculoId))
                             .ReturnsAsync(mockStatus);


            await Assert.ThrowsAsync<Exception>(async () => await reservaService.CriarReserva(mockReservaCreate));
        }

        [Fact]
        public async Task CriarReservaFail_Exception()
        {            
            var mockReserva = MockData.ReservaMockData.ReservaCreate();

            clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(mockReserva.ClienteId))
                             .ReturnsAsync(new Cliente()); 
            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(mockReserva.VeiculoId))
                             .ReturnsAsync(new Veiculo()); 
            reservasRepository.Setup(repo => repo.AdicionarReservaAsync(It.IsAny<Reserva>()))
                             .Throws(new Exception("Exceção simulada"));

            await Assert.ThrowsAsync<Exception>(async () => await reservaService.CriarReserva(mockReserva));
        }

        [Fact]
        public async Task CriarReservaFail_DbUpdateExceptionn()
        {
            var mockReserva = MockData.ReservaMockData.ReservaCreate();

            clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(mockReserva.ClienteId))
                             .ReturnsAsync(new Cliente());
            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(mockReserva.VeiculoId))
                             .ReturnsAsync(new Veiculo());
            reservasRepository.Setup(repo => repo.AdicionarReservaAsync(It.IsAny<Reserva>()))
                             .Throws(new DbUpdateException("DbUpdate simulada"));

            await Assert.ThrowsAsync<DbUpdateException>(async () => await reservaService.CriarReserva(mockReserva));
        }

        [Fact]
        public async Task CriarReserva_Success()
        {           
            var mockReservaCreate = MockData.ReservaMockData.ReservaCreate();
            
            var mockVeiculoStatus = new Veiculo { Status = VeiculoStatus.Online };
            var mockClienteStatus = new Cliente { Status = ClienteStatus.Online };

            clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(mockReservaCreate.ClienteId))
                             .ReturnsAsync(mockClienteStatus);
            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(mockReservaCreate.VeiculoId))
                             .ReturnsAsync(mockVeiculoStatus);
                        
            var mockReserva = MockData.ReservaMockData.ReservaCreate();
            reservasRepository.Setup(repo => repo.AdicionarReservaAsync(It.IsAny<Reserva>()))
                             .Callback<Reserva>(reserva =>
                             {
                                 reserva.Id = mockReserva.Id;
                             })
                             .Returns(Task.CompletedTask);
                        
            var result = await reservaService.CriarReserva(mockReservaCreate);

            
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal(mockReserva.Id, result.Id);
        }


        [Fact]
        public async Task EditarReservaFail_ReservaNaoEncontrada()
        {
            var mockReserva = MockData.ReservaMockData.ReservaUpdate();
            var reservaId = mockReserva.Id;


            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync((Reserva)null);

            await Assert.ThrowsAsync<Exception>(async () => await reservaService.EditarReserva(reservaId, mockReserva));
        }

        [Fact]
        public async Task EditarReservaFail_ReservaRunning()
        {
            
            var mockReserva = MockData.ReservaMockData.ReservaUpdate();
            var reservaId = mockReserva.Id;
            var mockStatus = new Reserva { Status = ReservaStatus.Running };

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync(mockStatus);

            await Assert.ThrowsAsync<Exception>(async () => await reservaService.EditarReserva(reservaId, mockReserva));
        }

        [Fact]
        public async Task EditarReservaFail_ReservaOffline()
        {
            var mockReserva = MockData.ReservaMockData.ReservaUpdate();
            var reservaId = mockReserva.Id;
            var mockStatus = new Reserva { Status = ReservaStatus.Offline };

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync(mockStatus);

            await Assert.ThrowsAsync<Exception>(async () => await reservaService.EditarReserva(reservaId, mockReserva));
        }

        [Fact]
        public async Task EditarReservaFail_DbUpdateExceptionn()
        {
            var mockReserva = MockData.ReservaMockData.ReservaUpdate();
            var reservaId = mockReserva.Id;
            var mockStatus = new Reserva { Status = ReservaStatus.Offline };

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .Throws(new DbUpdateException("DbUpdate simulada"));

            await Assert.ThrowsAsync<DbUpdateException>(async () => await reservaService.EditarReserva(reservaId, mockReserva));
        }

        [Fact]
        public async Task EditarReservaFail_Exception()
        {
            var mockReserva = MockData.ReservaMockData.ReservaUpdate();
            var reservaId = mockReserva.Id;
            var mockStatus = new Reserva { Status = ReservaStatus.Offline };

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .Throws(new Exception("Exceção simulada"));

            await Assert.ThrowsAsync<Exception>(async () => await reservaService.EditarReserva(reservaId, mockReserva));
        }


        [Fact]
        public async Task EditarReserva_Success()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            var reservaId = mockReserva.Id;
            var mockReservaUpdate = MockData.ReservaMockData.ReservaUpdate();

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync(mockReserva);

            reservasRepository.Setup(repo => repo.AtualizarReservaAsync(It.IsAny<Reserva>()))
                         .Returns(Task.CompletedTask);

            var result = await reservaService.EditarReserva(reservaId, mockReservaUpdate);
            
            Assert.NotNull(result);
            Assert.Equal(mockReserva.Id, result.Id);
        }

        [Fact]
        public async Task ExcluirReservaFail_ReservaNaoEncontrada()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            var reservaId = mockReserva.Id;

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync((Reserva)null);

            await Assert.ThrowsAsync<Exception>(async () => await reservaService.ExcluirReserva(reservaId));
        }

        [Fact]
        public async Task ExcluirReservaFail_ReservaRunning()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            var reservaId = mockReserva.Id;
            var mockStatus = new Reserva { Status = ReservaStatus.Running };

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync(mockStatus);

            await Assert.ThrowsAsync<Exception>(async () => await reservaService.ExcluirReserva(reservaId));
        }

        [Fact]
        public async Task ExcluirReservaFail_ReservaOffline()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            var reservaId = mockReserva.Id;
            var mockStatus = new Reserva { Status = ReservaStatus.Offline };

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync(mockStatus);


            await Assert.ThrowsAsync<Exception>(async () => await reservaService.ExcluirReserva(reservaId));
        }

        [Fact]
        public async Task ExcluirReservaFail_Exception()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            var reservaId = mockReserva.Id;
            

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync(mockReserva);
            reservasRepository.Setup(repo => repo.ExcluirReservaAsync(It.IsAny<Reserva>()))
                     .Throws(new Exception("Exception simulada"));


            await Assert.ThrowsAsync<Exception>(async () => await reservaService.ExcluirReserva(reservaId));
        }

        [Fact]
        public async Task ExcluirReserva_Success()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            var reservaId = mockReserva.Id;            

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync(mockReserva);

            reservasRepository.Setup(repo => repo.ExcluirReservaAsync(It.IsAny<Reserva>()))
                         .Returns(Task.CompletedTask);

            var result = await reservaService.ExcluirReserva(reservaId);

            Assert.NotNull(result);
            Assert.Equal(mockReserva.Id, result.Id);
        }
    }
}

