using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using RentCarSys.Application.DTO.AutoMapper;
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

            var result = await reservaService.BuscarTodasReservas();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
        }

        [Fact]
        public async void BuscarTodasReservas_Success()
        {
            var mockReservas = MockData.ReservaMockData.ReservasGetAll();

            reservasRepository.Setup(repo => repo.ObterTodasReservasAsync())
                 .ReturnsAsync(mockReservas);


            var result = await reservaService.BuscarTodasReservas();

            Assert.NotNull(result);
            Assert.Empty(result.Erros);
            Assert.Equal(mockReservas.Count, result.Dados.Count);
        }

        [Fact]
        public async void BuscarReservaPorId_Fail()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            int reservaId = mockReserva.Id;

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .Throws(new Exception("Exceção simulada"));


            var result = await reservaService.BuscarReservaPorId(reservaId);


            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
        }

        [Fact]
        public async void BuscarReservaPorId_ReservaNula()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            int reservaId = mockReserva.Id;

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync((Reserva)null);

            var result = await reservaService.BuscarReservaPorId(reservaId);


            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
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
            Assert.Empty(result.Erros);
            Assert.Equal(mockReserva.Id, result.Dados.Id);
        }

        [Fact]
        public async Task CriarReserva_ClienteNaoEncontrado()
        {
            
            var mockReserva = MockData.ReservaMockData.ReservaCreate();
            clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(mockReserva.ClienteId))
                             .ReturnsAsync((Cliente)null);
            var result = await reservaService.CriarReserva(mockReserva);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
        }

        [Fact]
        public async Task CriarReserva_VeiculoNaoEncontrado()
        {
            var mockReserva = MockData.ReservaMockData.ReservaCreate();

            clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(mockReserva.ClienteId))
                             .ReturnsAsync(new Cliente());
            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(mockReserva.VeiculoId))
                             .ReturnsAsync((Veiculo)null);

            var result = await reservaService.CriarReserva(mockReserva);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
        }

        [Fact]
        public async Task CriarReservaFail_ClienteRunnig()
        {
            var mockReserva = MockData.ReservaMockData.ReservaCreate();
            var mockStatus = new Cliente { Status = ClienteStatus.Running };

            clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(mockReserva.ClienteId))
                             .ReturnsAsync(mockStatus);
            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(mockReserva.VeiculoId))
                             .ReturnsAsync(new Veiculo());
            

            var result = await reservaService.CriarReserva(mockReserva);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
        }

        [Fact]
        public async Task CriarReservaFail_VeiculoRunnig()
        {
            var mockReserva = MockData.ReservaMockData.ReservaCreate();
            var mockStatus = new Veiculo { Status = VeiculoStatus.Running };

            clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(mockReserva.ClienteId))
                             .ReturnsAsync(new Cliente());
            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(mockReserva.VeiculoId))
                             .ReturnsAsync(mockStatus);


            var result = await reservaService.CriarReserva(mockReserva);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
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

            var result = await reservaService.CriarReserva(mockReserva);
            
            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);            
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

            var result = await reservaService.CriarReserva(mockReserva);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
            Assert.Contains("05XE8 - Não foi possível criar a reserva!", result.Erros);
        }

        [Fact]
        public async Task CriarReserva_Success()
        {

            var mockReserva = MockData.ReservaMockData.ReservaCreate();
            clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(mockReserva.ClienteId))
                             .ReturnsAsync(new Cliente()); 
            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(mockReserva.VeiculoId))
                             .ReturnsAsync(new Veiculo()); 

            var result = await reservaService.CriarReserva(mockReserva);

            Assert.NotNull(result);
            Assert.Empty(result.Erros);
        }

        [Fact]
        public async Task EditarReservaFail_ReservaNaoEncontrada()
        {
            var mockReserva = MockData.ReservaMockData.ReservaUpdate();
            var reservaId = mockReserva.Id;


            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync((Reserva)null);
                        
            var result = await reservaService.EditarReserva(reservaId, mockReserva);
                        
            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
        }

        [Fact]
        public async Task EditarReservaFail_ReservaRunning()
        {
            
            var mockReserva = MockData.ReservaMockData.ReservaUpdate();
            var reservaId = mockReserva.Id;
            var mockStatus = new Reserva { Status = ReservaStatus.Running };

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync(mockStatus);
            
            var result = await reservaService.EditarReserva(reservaId, mockReserva);
            
            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
        }

        [Fact]
        public async Task EditarReservaFail_ReservaOffline()
        {
            var mockReserva = MockData.ReservaMockData.ReservaUpdate();
            var reservaId = mockReserva.Id;
            var mockStatus = new Reserva { Status = ReservaStatus.Offline };

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync(mockStatus);
            
            var result = await reservaService.EditarReserva(reservaId, mockReserva);
            
            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
        }

        [Fact]
        public async Task EditarReservaFail_DbUpdateExceptionn()
        {
            var mockReserva = MockData.ReservaMockData.ReservaUpdate();
            var reservaId = mockReserva.Id;
            var mockStatus = new Reserva { Status = ReservaStatus.Offline };

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .Throws(new DbUpdateException("DbUpdate simulada"));

            var result = await reservaService.EditarReserva(reservaId, mockReserva);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
            Assert.Contains("05XE8 - Não foi possível alterar a reserva!", result.Erros);
        }

        [Fact]
        public async Task EditarReservaFail_Exception()
        {
            var mockReserva = MockData.ReservaMockData.ReservaUpdate();
            var reservaId = mockReserva.Id;
            var mockStatus = new Reserva { Status = ReservaStatus.Offline };

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .Throws(new Exception("Exceção simulada"));

            var result = await reservaService.EditarReserva(reservaId, mockReserva);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
        }

        [Fact]
        public async Task EditarReserva_Success()
        {
            var mockReserva = MockData.ReservaMockData.ReservaUpdate();
            var reservaId = mockReserva.Id;
            var mockStatus = new Reserva { Status = ReservaStatus.Online };

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync(mockStatus);
            
            var result = await reservaService.EditarReserva(reservaId, mockReserva);
            
            Assert.NotNull(result);
            Assert.Empty(result.Erros);           
        }

        [Fact]
        public async Task ExcluirReservaFail_ReservaNaoEncontrada()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            var reservaId = mockReserva.Id;

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync((Reserva)null);
            
            var result = await reservaService.ExcluirReserva(reservaId);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
        }

        [Fact]
        public async Task ExcluirReservaFail_ReservaRunning()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            var reservaId = mockReserva.Id;
            var mockStatus = new Reserva { Status = ReservaStatus.Running };

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync(mockStatus);

            var result = await reservaService.ExcluirReserva(reservaId);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
        }

        [Fact]
        public async Task ExcluirReservaFail_ReservaOffline()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            var reservaId = mockReserva.Id;
            var mockStatus = new Reserva { Status = ReservaStatus.Offline };

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync(mockStatus);


            var result = await reservaService.ExcluirReserva(reservaId);
          
            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
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


            var result = await reservaService.ExcluirReserva(reservaId);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Erros);
            Assert.Contains("05X12 - Falha interna no servidor!", result.Erros);
        }

        [Fact]
        public async Task ExcluirReserva_Success()
        {
            var mockReserva = MockData.ReservaMockData.ReservaById();
            var reservaId = mockReserva.Id;
            var mockStatus = new Reserva { Status = ReservaStatus.Online };

            reservasRepository.Setup(repo => repo.ObterReservaPorIdAsync(reservaId))
                             .ReturnsAsync(mockStatus);

            var result = await reservaService.ExcluirReserva(reservaId);

            Assert.NotNull(result);
            Assert.Empty(result.Erros);            
        }
    }
}

