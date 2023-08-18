using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Moq;
using Newtonsoft.Json;
using RentCarSys.Application.Controllers;
using RentCarSys.Application.DTO.AutoMapper;
using RentCarSys.Application.DTO.VeiculosDTOs;
using RentCarSys.Application.Interfaces;
using RentCarSys.Application.Models;
using RentCarSys.Application.Models.Enums;
using RentCarSys.Application.Services;
using RentCarSys.Test.IntegrationTest.MockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RentCarSys.Test.IntegrationTest.Controllers
{
    public class VeiculoControllersTest
    {
        protected Mock<IVeiculosRepository> veiculosRepository = new Mock<IVeiculosRepository>();
        protected Mock<VeiculoService> veiculoService;
        protected IMapper mapper;
        private readonly VeiculoController veiculoController;

        public VeiculoControllersTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntitiesDTOMappingProfile());
            });
            mapper = config.CreateMapper();


            veiculoService = new Mock<VeiculoService>(veiculosRepository.Object, mapper);
            veiculoController = new VeiculoController(veiculoService.Object);
        }

        [Fact]
        public async Task BuscarTodosVeiculos_Success()
        {
            await using var application = new RentCarSysApplication();

            var veiculo1 = new Veiculo
            { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1" };
            var veiculo2 = new Veiculo
            { Id = 2, Status = VeiculoStatus.Online, Placa = "Placaa2", Marca = "Marca2", Modelo = "Modelo2", AnoFabricacao = "Ano2", KM = "KM2", QuantidadePortas = 4, Cor = "Cor2", Automatico = "Automatico2" };

            List<Veiculo> veiculos = new List<Veiculo>() { veiculo1, veiculo2 };

            await VeiculoMockData.CreateVeiculos(application, veiculos);

            var url = "veiculo/buscarTodos";
            var veiculo = application.CreateClient();

            var response = await veiculo.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<VeiculoDTOGetAll>>();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Placa == "Placaa1");
            Assert.Contains(result, c => c.Placa == "Placaa2");

            await VeiculoMockData.DeletarVeiculos(application, veiculos);
        }

        [Fact]
        public async Task BuscarVeiculoPorId_Fail()
        {
            await using var application = new RentCarSysApplication();

            var veiculo1 = new Veiculo
            { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1" };
            var veiculo2 = new Veiculo
            { Id = 2, Status = VeiculoStatus.Online, Placa = "Placaa2", Marca = "Marca2", Modelo = "Modelo2", AnoFabricacao = "Ano2", KM = "KM2", QuantidadePortas = 4, Cor = "Cor2", Automatico = "Automatico2" };

            List<Veiculo> veiculos = new List<Veiculo>() { veiculo1, veiculo2 };

            var url = "veiculo/buscarPorId/1";
            var veiculo = application.CreateClient();

            var response = await veiculo.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("Veiculo não encontrado, verifique se o veiculo já foi cadastrado!", errorResponse);
        }

        [Fact]
        public async Task BuscarVeiculoPorId_Sucess()
        {
            await using var application = new RentCarSysApplication();

            var veiculo1 = new Veiculo
            { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1" };
            var veiculo2 = new Veiculo
            { Id = 2, Status = VeiculoStatus.Online, Placa = "Placaa2", Marca = "Marca2", Modelo = "Modelo2", AnoFabricacao = "Ano2", KM = "KM2", QuantidadePortas = 4, Cor = "Cor2", Automatico = "Automatico2" };

            List<Veiculo> veiculos = new List<Veiculo>() { veiculo1, veiculo2 };

            await VeiculoMockData.CreateVeiculos(application, veiculos);

            var url = "veiculo/buscarPorId/1";
            var veiculo = application.CreateClient();

            var response = await veiculo.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<VeiculoDTO>();
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);

            await VeiculoMockData.DeletarVeiculos(application, veiculos);
        }

        [Fact]
        public async Task BuscarVeiculoPorPlaca_Fail()
        {
            await using var application = new RentCarSysApplication();

            var veiculo1 = new Veiculo
            { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1" };
            var veiculo2 = new Veiculo
            { Id = 2, Status = VeiculoStatus.Online, Placa = "Placaa2", Marca = "Marca2", Modelo = "Modelo2", AnoFabricacao = "Ano2", KM = "KM2", QuantidadePortas = 4, Cor = "Cor2", Automatico = "Automatico2" };

            List<Veiculo> veiculos = new List<Veiculo>() { veiculo1, veiculo2 };

            var url = "veiculo/buscarPorPlaca/Placaa1";
            var veiculo = application.CreateClient();

            var response = await veiculo.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("Veiculo não encontrado, verifique se a placa está correta!", errorResponse);
        }

        [Fact]
        public async Task BuscarVeiculoPorPlaca_Sucess()
        {
            await using var application = new RentCarSysApplication();

            var veiculo1 = new Veiculo
            { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1" };
            var veiculo2 = new Veiculo
            { Id = 2, Status = VeiculoStatus.Online, Placa = "Placaa2", Marca = "Marca2", Modelo = "Modelo2", AnoFabricacao = "Ano2", KM = "KM2", QuantidadePortas = 4, Cor = "Cor2", Automatico = "Automatico2" };

            List<Veiculo> veiculos = new List<Veiculo>() { veiculo1, veiculo2 };

            await VeiculoMockData.CreateVeiculos(application, veiculos);

            var url = "veiculo/buscarPorPlaca/Placaa1";
            var veiculo = application.CreateClient();

            var response = await veiculo.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<VeiculoDTO>();

            Assert.NotNull(result);
            Assert.Equal("Placaa1", result.Placa);

            await VeiculoMockData.DeletarVeiculos(application, veiculos);
        }

        [Fact]
        public async Task CriarVeiculo_Fail()
        {
            await using var application = new RentCarSysApplication();

            var veiculo1 = new Veiculo
            { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1" };
            var veiculo2 = new Veiculo
            { Id = 2, Status = VeiculoStatus.Online, Placa = "Placaa2", Marca = "Marca2", Modelo = "Modelo2", AnoFabricacao = "Ano2", KM = "KM2", QuantidadePortas = 4, Cor = "Cor2", Automatico = "Automatico2" };

            List<Veiculo> veiculos = new List<Veiculo>() { veiculo1, veiculo2 };

            var url = "veiculo/cadastrar";
            var veiculo = application.CreateClient();

            var veiculoCreateModel = new VeiculoDTOCreate
            {
                Placa = "",
                Marca = "",
                Modelo = "",
                AnoFabricacao = "",
                KM = "",
                QuantidadePortas = -1,
                Cor = "",
                Automatico = ""
            };

            var content = new StringContent(JsonConvert.SerializeObject(veiculoCreateModel), Encoding.UTF8, "application/json");

            var response = await veiculo.PostAsync(url, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("A Placa deve conter 4 letras e 3 números!", errorResponse);
            Assert.Contains("O modelo é obrigatório!", errorResponse);
            Assert.Contains("O ano de fabricação é obrigatório!", errorResponse);
            Assert.Contains("A quilometragem é obrigatório!", errorResponse);
            Assert.Contains("A cor é obrigatório!", errorResponse);
            Assert.Contains("O tipo é obrigatório!", errorResponse);
        }

        [Fact]
        public async Task CriarVeiculo_Sucess()
        {
            await using var application = new RentCarSysApplication();

            var veiculo1 = new Veiculo
            { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1" };
            var veiculo2 = new Veiculo
            { Id = 2, Status = VeiculoStatus.Online, Placa = "Placaa2", Marca = "Marca2", Modelo = "Modelo2", AnoFabricacao = "Ano2", KM = "KM2", QuantidadePortas = 4, Cor = "Cor2", Automatico = "Automatico2" };

            List<Veiculo> veiculos = new List<Veiculo>() { veiculo1, veiculo2 };

            await VeiculoMockData.CreateVeiculos(application, veiculos);

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

            await VeiculoMockData.DeletarVeiculos(application, veiculos);
        }



        [Fact]
        public async Task EditarVeiculo_Sucess()
        {
            await using var application = new RentCarSysApplication();

            var veiculo1 = new Veiculo
            { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1" };
            var veiculo2 = new Veiculo
            { Id = 2, Status = VeiculoStatus.Online, Placa = "Placaa2", Marca = "Marca2", Modelo = "Modelo2", AnoFabricacao = "Ano2", KM = "KM2", QuantidadePortas = 4, Cor = "Cor2", Automatico = "Automatico2" };

            List<Veiculo> veiculos = new List<Veiculo>() { veiculo1, veiculo2 };

            await VeiculoMockData.CreateVeiculos(application, veiculos);

            var url = "veiculo/alterar/1";
            var veiculo = application.CreateClient();

            var veiculoUpdateModel = new VeiculoDTOUpdate
            {
                Id = 1,
                Placa = "Placaa8",
                Marca = "Marca8",
                Modelo = "Modelo9",
                AnoFabricacao = "Ano8",
                KM = "KM8",
                QuantidadePortas = 4,
                Cor = "Cor8",
                Automatico = "Automatico8"
            };

            var content = new StringContent(JsonConvert.SerializeObject(veiculoUpdateModel), Encoding.UTF8, "application/json");

            var response = await veiculo.PutAsync(url, content);

            response.EnsureSuccessStatusCode();

            var updatedResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<VeiculoDTO>(updatedResponse);

            Assert.NotNull(result);
            Assert.Equal(veiculoUpdateModel.Placa, result.Placa);
            Assert.Equal(veiculoUpdateModel.Marca, result.Marca);
            Assert.Equal(veiculoUpdateModel.Modelo, result.Modelo);
            Assert.Equal(veiculoUpdateModel.KM, result.KM);
            Assert.Equal(veiculoUpdateModel.Cor, result.Cor);
            Assert.Equal(veiculoUpdateModel.Automatico, result.Automatico);

            await VeiculoMockData.DeletarVeiculos(application, veiculos);
        }

        [Fact]
        public async Task ExcluirVeiculos_Fail()
        {
            await using var application = new RentCarSysApplication();

            var veiculo1 = new Veiculo
            { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1" };
            var veiculo2 = new Veiculo
            { Id = 2, Status = VeiculoStatus.Online, Placa = "Placaa2", Marca = "Marca2", Modelo = "Modelo2", AnoFabricacao = "Ano2", KM = "KM2", QuantidadePortas = 4, Cor = "Cor2", Automatico = "Automatico2" };

            List<Veiculo> veiculos = new List<Veiculo>() { veiculo1, veiculo2 };

            await VeiculoMockData.CreateVeiculos(application, veiculos);

            var url = "veiculo/excluir/111";
            var veiculo = application.CreateClient();

            var response = await veiculo.DeleteAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("", errorResponse);
        }

        [Fact]
        public async Task ExcluirVeiculo_Success()
        {
            await using var application = new RentCarSysApplication();

            var veiculo1 = new Veiculo
            { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1" };
            var veiculo2 = new Veiculo
            { Id = 2, Status = VeiculoStatus.Online, Placa = "Placaa2", Marca = "Marca2", Modelo = "Modelo2", AnoFabricacao = "Ano2", KM = "KM2", QuantidadePortas = 4, Cor = "Cor2", Automatico = "Automatico2" };

            List<Veiculo> veiculos = new List<Veiculo>() { veiculo1, veiculo2 };

            await VeiculoMockData.CreateVeiculos(application, veiculos);

            var url = "veiculo/excluir/1";
            var veiculo = application.CreateClient();

            var response = await veiculo.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<VeiculoDTO>();

            Assert.NotNull(result);

            //await VeiculoMockData.DeletarVeiculos(application, veiculos);
        }
    }
}
