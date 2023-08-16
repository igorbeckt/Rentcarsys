using AutoMapper;
using Moq;
using RentCarSys.Application.DTO.AutoMapper;
using RentCarSys.Application.DTO.ClientesDTOs;
using RentCarSys.Application.DTO.VeiculosDTOs;
using RentCarSys.Application.Interfaces;
using RentCarSys.Application.Models;
using RentCarSys.Application.Models.Enums;
using RentCarSys.Application.Repository;
using RentCarSys.Application.Services;
using RentCarSys.Application.Services.RentCarSys.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarSys.Test.UnitTest.Services
{
    public class VeiculoServiceTest
    {
        protected Mock<IVeiculosRepository> veiculosRepository = new Mock<IVeiculosRepository>();
        protected IMapper mapper;
        private readonly VeiculoService veiculoService;

        public VeiculoServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntitiesDTOMappingProfile());
            });
            mapper = config.CreateMapper();
            veiculoService = new VeiculoService(veiculosRepository.Object, mapper);
        }

        [Fact]
        public async void BuscarTodosVeiculos_Fail()
        {
            veiculosRepository.Setup(repo => repo.ObterTodosVeiculosAsync())
                             .Throws(new Exception("Exceção simulada"));            

            await Assert.ThrowsAsync<Exception>(async () => await veiculoService.BuscarTodosVeiculos());
        }

        [Fact]
        public async void BuscarTodosVeiculos_Success()
        {
            var mockVeiculos = MockData.VeiculoMockData.VeiculosGetAll();

            veiculosRepository.Setup(repo => repo.ObterTodosVeiculosAsync())
                 .ReturnsAsync(mockVeiculos);


            var result = await veiculoService.BuscarTodosVeiculos();

            Assert.NotNull(result);
            Assert.IsType<List<VeiculoDTOGetAll>>(result);
            Assert.Equal(mockVeiculos.Count, result.Count);
        }

        [Fact]
        public async void BuscarVeiculoPorId_Fail()
        {
            var mockVeiculo = MockData.VeiculoMockData.VeiculoGetById();
            int veiculoId = mockVeiculo.Id;

            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(veiculoId))
                             .Throws(new Exception("Exceção simulada"));

            await Assert.ThrowsAsync<Exception>(async () => await veiculoService.BuscarVeiculoPorId(veiculoId));
        }

        [Fact]
        public async void BuscarVeiculoPorId_VeiculoNulo()
        {
            var mockVeiculo = MockData.VeiculoMockData.VeiculoGetById();
            int veiculoId = mockVeiculo.Id;

            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(veiculoId))
                             .ReturnsAsync((Veiculo)null);

            await Assert.ThrowsAsync<Exception>(async () => await veiculoService.BuscarVeiculoPorId(veiculoId));
        }

        [Fact]
        public async void BuscarVeiculoPorId_Success()
        {
            var mockVeiculo = MockData.VeiculoMockData.VeiculoGetById();
            int veiculoId = mockVeiculo.Id;

            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(veiculoId))
                             .ReturnsAsync(mockVeiculo);

            var result = await veiculoService.BuscarVeiculoPorId(veiculoId);

            Assert.NotNull(result);
            Assert.IsType<VeiculoDTO>(result);
        }

        [Fact]
        public async void BuscarVeiculoPorPlaca_Fail()
        {
            var mockVeiculo = MockData.VeiculoMockData.VeiculoGetById();
            string veiculoPlaca = mockVeiculo.Placa;

            veiculosRepository.Setup(repo => repo.ObterVeiculoPorPlacaAsync(veiculoPlaca))
                             .Throws(new Exception("Exceção simulada"));

            await Assert.ThrowsAsync<Exception>(async () => await veiculoService.BuscarVeiculoPorPlaca(veiculoPlaca));
        }

        [Fact]
        public async void BuscarVeiculoPorPlaca_VeiculoNulo()
        {
            var mockVeiculo = MockData.VeiculoMockData.VeiculoGetById();
            string veiculoPlaca = mockVeiculo.Placa;

            veiculosRepository.Setup(repo => repo.ObterVeiculoPorPlacaAsync(veiculoPlaca))
                             .ReturnsAsync((Veiculo)null);

            await Assert.ThrowsAsync<Exception>(async () => await veiculoService.BuscarVeiculoPorPlaca(veiculoPlaca));
        }

        [Fact]
        public async void BuscarVeiculoPorPlaca_Sucess()
        {
            var mockVeiculo = MockData.VeiculoMockData.VeiculoGetById();
            string veiculoId = mockVeiculo.Placa;

            veiculosRepository.Setup(repo => repo.ObterVeiculoPorPlacaAsync(veiculoId))
                             .ReturnsAsync(mockVeiculo);

            var result = await veiculoService.BuscarVeiculoPorPlaca(veiculoId);

            Assert.NotNull(result);
            Assert.IsType<VeiculoDTO>(result);
        }

        [Fact]
        public async void CriarVeiculoFail_Exception()
        {
            var mockVeiculoCreate = MockData.VeiculoMockData.VeiculoCreate();

            veiculosRepository.Setup(repo => repo.AdicionarVeiculoAsync(It.IsAny<Veiculo>()))
                             .Throws(new Exception("Exceção simulada"));

            await Assert.ThrowsAsync<Exception>(async () => await veiculoService.CriarVeiculo(mockVeiculoCreate));
        }

        [Fact]
        public async void CriarVeiculo_Success()
        {
            var mockVeiculoCreate = MockData.VeiculoMockData.VeiculoCreate();

            var mockVeiculo = MockData.VeiculoMockData.VeiculoGetById();

            veiculosRepository.Setup(repo => repo.AdicionarVeiculoAsync(It.IsAny<Veiculo>()))
                             .Callback<Veiculo>(veiculo =>
                             {
                                 veiculo.Id = mockVeiculo.Id;
                             })
                             .Returns(Task.CompletedTask);

            var result = await veiculoService.CriarVeiculo(mockVeiculoCreate);

            Assert.NotNull(result);
            Assert.Equal(mockVeiculo.Id, result.Id);
        }

        [Fact]
        public async void EditarVeiculo_Fail()
        {
            var MockId = MockData.VeiculoMockData.VeiculoGetById();
            var veiculoId = MockId.Id;
            var mockVeiculoUpdate = MockData.VeiculoMockData.VeiculoUpdate();

            var mockVeiculo = MockData.VeiculoMockData.VeiculoGetById();

            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(veiculoId))
                             .ReturnsAsync(mockVeiculo);

            veiculosRepository.Setup(repo => repo.AtualizarVeiculoAsync(It.IsAny<Veiculo>()))
                             .Throws(new Exception("Exceção simulada"));

            await Assert.ThrowsAsync<Exception>(async () => await veiculoService.EditarVeiculo(veiculoId, mockVeiculoUpdate));
        }

        [Fact]
        public async void EditarVeiculo_VeiculoNaoEncontrado()
        {
            var mockVeiculoUpdate = MockData.VeiculoMockData.VeiculoUpdate();
            var veiculoId = mockVeiculoUpdate.Id;            

            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(veiculoId))
                             .ReturnsAsync((Veiculo)null);

            await Assert.ThrowsAsync<Exception>(async () => await veiculoService.EditarVeiculo(veiculoId, mockVeiculoUpdate));
        }

        [Fact]
        public async void EditarVeiculo_VeiculoEmEstadoInvalido()
        {
            var mockVeiculoUpdate = MockData.VeiculoMockData.VeiculoUpdate(); ;
            var veiculoId = mockVeiculoUpdate.Id;
            var mockStatus = new Veiculo { Status = VeiculoStatus.Running };                       

            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(veiculoId))
                             .ReturnsAsync(mockStatus);

            await Assert.ThrowsAsync<Exception>(async () => await veiculoService.EditarVeiculo(veiculoId, mockVeiculoUpdate));
        }

        [Fact]
        public async void EditarVeiculo_Success()
        {
            var MockId = MockData.VeiculoMockData.VeiculoGetById();
            var veiculoId = MockId.Id;
            var mockVeiculoDTOUpdate = MockData.VeiculoMockData.VeiculoUpdate();

            var mockVeiculo = MockData.VeiculoMockData.VeiculoGetById();

            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(veiculoId))
                             .ReturnsAsync(mockVeiculo);

            veiculosRepository.Setup(repo => repo.AtualizarVeiculoAsync(It.IsAny<Veiculo>()))
                             .Returns(Task.CompletedTask);

            var result = await veiculoService.EditarVeiculo(veiculoId, mockVeiculoDTOUpdate);

            Assert.NotNull(result);
            Assert.Equal(mockVeiculo.Id, result.Id);
        }

        [Fact]
        public async void ExcluirVeiculo_Fail()
        {
            var mockVeiculo = MockData.VeiculoMockData.VeiculoGetById();
            var veiculoId = mockVeiculo.Id;

            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(veiculoId))
                             .ReturnsAsync(mockVeiculo);

            veiculosRepository.Setup(repo => repo.ExcluirVeiculoAsync(It.IsAny<Veiculo>()))
                             .Throws(new Exception("Exceção simulada"));

            await Assert.ThrowsAsync<Exception>(async () => await veiculoService.ExcluirVeiculo(veiculoId));
        }


        [Fact]
        public async void ExcluirVeiculo_VeiculoNaoEncontrado()
        {
            var mockVeiculo = MockData.VeiculoMockData.VeiculoGetById();
            var veiculoId = mockVeiculo.Id;

            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(veiculoId))
                           .ReturnsAsync((Veiculo)null);

            await Assert.ThrowsAsync<Exception>(async () => await veiculoService.ExcluirVeiculo(veiculoId));
        }

        [Fact]
        public async void ExcluirVeiculo_VeiculoEmEstadoInvalido()
        {
            var mockVeiculo = MockData.VeiculoMockData.VeiculoGetById();
            var veiculoId = mockVeiculo.Id;
            mockVeiculo.Status = VeiculoStatus.Running;

            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(veiculoId))
                             .ReturnsAsync(mockVeiculo);

            await Assert.ThrowsAsync<Exception>(async () => await veiculoService.ExcluirVeiculo(veiculoId));
        }

        [Fact]
        public async void ExcluirVeiculo_Success()
        {
            var mockVeiculo = MockData.VeiculoMockData.VeiculoGetById();
            var veiculoId = mockVeiculo.Id;

            veiculosRepository.Setup(repo => repo.ObterVeiculoPorIdAsync(veiculoId))
                             .ReturnsAsync(mockVeiculo);

            veiculosRepository.Setup(repo => repo.ExcluirVeiculoAsync(It.IsAny<Veiculo>()))
                             .Returns(Task.CompletedTask);

            var result = await veiculoService.ExcluirVeiculo(veiculoId);

            Assert.NotNull(result);
            Assert.Equal(mockVeiculo.Id, result.Id);
        }
    }
}
