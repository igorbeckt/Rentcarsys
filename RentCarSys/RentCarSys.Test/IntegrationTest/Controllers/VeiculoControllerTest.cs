using AutoMapper;
using Moq;
using RentCarSys.Application.Controllers;
using RentCarSys.Application.DTO.ClientesDTOs;
using RentCarSys.Application.Interfaces;
using RentCarSys.Application.Services;
using RentCarSys.Test.IntegrationTest.MockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RentCarSys.Application.DTO.AutoMapper;
using RentCarSys.Application.DTO.VeiculosDTOs;
using Newtonsoft.Json;
using RentCarSys.Application.Models.Enums;

namespace RentCarSys.Test.IntegrationTest.Controllers
{
    public class VeiculoControllerTest
    {
        protected Mock<IVeiculosRepository> veiculosRepository = new Mock<IVeiculosRepository>();
        protected IMapper mapper;
        private readonly VeiculoService veiculoService;
        private readonly VeiculoController veiculoController;

        public VeiculoControllerTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntitiesDTOMappingProfile());
            });
            mapper = config.CreateMapper();
            veiculoService = new VeiculoService(veiculosRepository.Object, mapper);
            veiculoController = new VeiculoController(veiculoService);
        }

        [Fact]
        public async Task BuscarTodosVeiculos_Success()
        {
            await using var application = new RentCarSysApplication();
            await VeiculoMockData.CreateVeiculos(application, true);

            var url = "veiculo/buscarTodos";
            var veiculo = application.CreateClient();

            var response = await veiculo.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<VeiculoDTOGetAll>>();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Placa == "Placaa1");
            Assert.Contains(result, c => c.Placa == "Placaa2");
        }

        [Fact]
        public async Task BuscarVeiculoPorId_Fail()
        {
            await using var application = new RentCarSysApplication();

            var url = "veiculo/buscarPorId/1";
            var veiculo = application.CreateClient();

            var response = await veiculo.GetAsync(url);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("Veiculo não encontrado, verifique se o veiculo já foi cadastrado!", errorResponse);
        }

        [Fact]
        public async Task BuscarVeiculoPorId_Sucess()
        {
            await using var application = new RentCarSysApplication();
            await VeiculoMockData.CreateVeiculos(application, true);

            var url = "veiculo/buscarPorId/1";
            var veiculo = application.CreateClient();

            var response = await veiculo.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<VeiculoDTO>();
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task BuscarVeiculoPorPlaca_Fail()
        {
            await using var application = new RentCarSysApplication();            

            var url = "veiculo/buscarPorPlaca/Placaa1";
            var veiculo = application.CreateClient();

            var response = await veiculo.GetAsync(url);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("Veiculo não encontrado, verifique se a placa está correta!", errorResponse);
        }

        [Fact]
        public async Task BuscarVeiculoPorPlaca_Sucess()
        {
            await using var application = new RentCarSysApplication();
            await VeiculoMockData.CreateVeiculos(application, true);

            var url = "veiculo/buscarPorPlaca/Placaa1";
            var veiculo = application.CreateClient();

            var response = await veiculo.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<VeiculoDTO>();

            Assert.NotNull(result);
            Assert.Equal("Placaa1", result.Placa);
        }

        [Fact]
        public async Task CriarVeiculo_Sucess()
        {
            await using var application = new RentCarSysApplication();
            var url = "veiculo/cadastrar";
            var veiculo = application.CreateClient();

            var veiculoCreateModel = new VeiculoDTOCreate
            {
                Placa = "Placaa1",
                Marca = "Marca1",
                Modelo = "Modelo1",
                AnoFabricacao = "Ano1",
                KM = "KM1",
                QuantidadePortas = 2,
                Cor = "Cor1",
                Automatico = "Automatico1"
            };

            var content = new StringContent(JsonConvert.SerializeObject(veiculoCreateModel), Encoding.UTF8, "application/json");

            var response = await veiculo.PostAsync(url, content);

            response.EnsureSuccessStatusCode();

            var createdResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<VeiculoDTO>(createdResponse);

            Assert.NotNull(result);
            Assert.Equal(veiculoCreateModel.Placa, result.Placa);
            Assert.Equal(veiculoCreateModel.Marca, result.Marca);
            Assert.Equal(veiculoCreateModel.Modelo, result.Modelo);
            Assert.Equal(veiculoCreateModel.AnoFabricacao, result.AnoFabricacao);
            Assert.Equal(veiculoCreateModel.KM, result.KM);
            Assert.Equal(veiculoCreateModel.QuantidadePortas, result.QuantidadePortas);
            Assert.Equal(veiculoCreateModel.Cor, result.Cor);
            Assert.Equal(veiculoCreateModel.Automatico, result.Automatico);
        }

        [Fact]
        public async Task EditarVeiculo_Sucess()
        {
            await using var application = new RentCarSysApplication();
            var url = "veiculo/alterar/1"; 
            var veiculo = application.CreateClient();

            var veiculoUpdateModel = new VeiculoDTOUpdate
            {   
                Placa = "Placaa1",
                Marca = "Marca1",
                Modelo = "Modelo1",
                AnoFabricacao = "Ano1",
                KM = "KM1",
                QuantidadePortas = 2,
                Cor = "Cor1",
                Automatico = "Automatico1"
            };

            var content = new StringContent(JsonConvert.SerializeObject(veiculoUpdateModel), Encoding.UTF8, "application/json");
            
            var response = await veiculo.PostAsync(url, content);
           
            response.EnsureSuccessStatusCode();

            var updatedResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<VeiculoDTO>(updatedResponse);

            Assert.NotNull(result);
            Assert.Equal(veiculoUpdateModel.Placa, result.Placa);
            Assert.Equal(veiculoUpdateModel.Marca, result.Marca);
            Assert.Equal(veiculoUpdateModel.Modelo, result.Modelo);
            Assert.Equal(veiculoUpdateModel.KM, result.KM);
            Assert.Equal(veiculoUpdateModel.QuantidadePortas, result.QuantidadePortas);
            Assert.Equal(veiculoUpdateModel.Cor, result.Cor);
            Assert.Equal(veiculoUpdateModel.Automatico, result.Automatico);
        }

        [Fact]
        public async Task ExcluirVeiculo_Success()
        {
            await using var application = new RentCarSysApplication();
            await VeiculoMockData.CreateVeiculos(application, true);

            var url = "veiculo/excluir/1";
            var veiculo = application.CreateClient();

            var response = await veiculo.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<VeiculoDTO>();

            Assert.NotNull(result);
        }
    }
}
