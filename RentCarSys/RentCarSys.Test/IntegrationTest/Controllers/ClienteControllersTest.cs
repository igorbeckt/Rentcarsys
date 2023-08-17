using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Moq;
using Newtonsoft.Json;
using RentCarSys.Application.Controllers;
using RentCarSys.Application.DTO.AutoMapper;
using RentCarSys.Application.DTO.ClientesDTOs;
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
    public class ClienteControllersTest
    {
        protected Mock<IClientesRepository> clientesRepository = new Mock<IClientesRepository>();
        protected Mock<ClienteService> clienteService;
        protected IMapper mapper;
        public readonly ClienteController clienteController;

        public ClienteControllersTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntitiesDTOMappingProfile());
            });
            mapper = config.CreateMapper();

            clienteService = new Mock<ClienteService>(clientesRepository.Object, mapper);
            clienteController = new ClienteController(clienteService.Object);
        }

        [Fact]
        public async Task BuscarTodosClientes_Fail()
        {
            await using var application = new RentCarSysApplication();
            await ClienteMockData.CreateClientes(application, true);

            var url = "cliente/buscarClientes";
            var cliente = application.CreateClient();

            var response = await cliente.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task BuscarTodosClientes_Success()
        {
            await using var application = new RentCarSysApplication();
            await ClienteMockData.CreateClientes(application, true);

            var url = "cliente/buscarTodos";
            var cliente = application.CreateClient();

            var response = await cliente.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<ClienteDTOGetAll>>();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.NomeCompleto == "Cliente 1");
            Assert.Contains(result, c => c.NomeCompleto == "Cliente 2");
        }

        [Fact]
        public async Task BuscarClientePorId_Fail()
        {
            await using var application = new RentCarSysApplication();

            var url = "cliente/buscarPorId/1";
            var cliente = application.CreateClient();

            var response = await cliente.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("Cliente não encontrado, verifique se o cliente já foi cadastrado!", errorResponse);
        }

        [Fact]
        public async Task BuscarClientePorId_Sucess()
        {
            await using var application = new RentCarSysApplication();
            await ClienteMockData.CreateClientes(application, true);

            var url = "cliente/buscarPorId/1";
            var cliente = application.CreateClient();

            var response = await cliente.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ClienteDTO>();
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task BuscarClientePorCpf_Fail()
        {
            await using var application = new RentCarSysApplication();
            await ClienteMockData.CreateClientes(application, true);

            var url = "cliente/buscarPorCpf/98765432109";
            var cliente = application.CreateClient();

            var response = await cliente.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("Cliente não encontrado", errorResponse);
        }

        [Fact]
        public async Task BuscarClientePorCpf_Sucess()
        {
            await using var application = new RentCarSysApplication();
            await ClienteMockData.CreateClientes(application, true);

            var url = "cliente/buscarPorCpf/12345678912";
            var cliente = application.CreateClient();

            var response = await cliente.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ClienteDTO>();

            Assert.NotNull(result);
            Assert.Equal(12345678912, result.CPF);
        }

        [Fact]
        public async Task CriarCliente_Fail()
        {
            await using var application = new RentCarSysApplication();
            var url = "cliente/cadastrar";
            var cliente = application.CreateClient();

            var clienteCreateModel = new ClienteDTOCreate
            {
                NomeCompleto = "",
                CPF = 987654921091,
                RG = 98765844,
                Email = ""
            };

            var content = new StringContent(JsonConvert.SerializeObject(clienteCreateModel), Encoding.UTF8, "application/json");
      
            var response = await cliente.PostAsync(url, content);
  
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); 

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("O nome é obrigatório!", errorResponse); 
            Assert.Contains("Insira um e-mail válido", errorResponse);             
            Assert.Contains("O RG deve conter 7 dígitos!", errorResponse);             
            Assert.Contains("O CPF deve conter 11 dígitos!", errorResponse); 
        }     

        [Fact]
        public async Task CriarCliente_Sucess()
        {
            await using var application = new RentCarSysApplication();
            var url = "cliente/cadastrar";
            var cliente = application.CreateClient();

            var clienteCreateModel = new ClienteDTOCreate
            {
                NomeCompleto = "Novo Cliente",
                CPF = 98765492109,
                RG = 9876583,
                Email = "novo_cliente@example.com"
            };

            var content = new StringContent(JsonConvert.SerializeObject(clienteCreateModel), Encoding.UTF8, "application/json");

            var response = await cliente.PostAsync(url, content);

            response.EnsureSuccessStatusCode();

            var createdResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ClienteDTO>(createdResponse);

            Assert.NotNull(result);
            Assert.Equal(clienteCreateModel.NomeCompleto, result.NomeCompleto);
            Assert.Equal(clienteCreateModel.Email, result.Email);
            Assert.Equal(clienteCreateModel.RG, result.RG);
            Assert.Equal(clienteCreateModel.CPF, result.CPF);
        }

        [Fact]
        public async Task EditarCliente_Fail()
        {
            await using var application = new RentCarSysApplication();

            var url = "cliente/alterar/1";
            var cliente = application.CreateClient();

            var clienteUpdateModel = new ClienteDTOUpdate
            {
                Id = 1,
                NomeCompleto = "Cliente Editado",
                Email = "cliente_editado@example.com",
                RG = 1234567,
                CPF = 12345678911
            };

            var content = new StringContent(JsonConvert.SerializeObject(clienteUpdateModel), Encoding.UTF8, "application/json");

            var response = await cliente.PutAsync(url, content);
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("", errorResponse);
        }

        [Fact]
        public async Task EditarCliente_Sucess()
        {
            await using var application = new RentCarSysApplication();
            await ClienteMockData.CreateClientes(application, true);
            var url = "cliente/alterar/1"; 
            var cliente = application.CreateClient();

            var clienteUpdateModel = new ClienteDTOUpdate
            {
                Id = 1,
                NomeCompleto = "Cliente Editado",
                Email = "cliente_editado@example.com",
                RG = 1234567,
                CPF = 12345678911              
            };            

            var content = new StringContent(JsonConvert.SerializeObject(clienteUpdateModel), Encoding.UTF8, "application/json");

            
            var response = await cliente.PutAsync(url, content);
            
            response.EnsureSuccessStatusCode();

            var updatedResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ClienteDTO>(updatedResponse);

            Assert.NotNull(result);
            Assert.Equal(clienteUpdateModel.NomeCompleto, result.NomeCompleto);
            Assert.Equal(clienteUpdateModel.Email, result.Email);
            Assert.Equal(clienteUpdateModel.RG, result.RG);
            Assert.Equal(clienteUpdateModel.CPF, result.CPF);
        }

        [Fact]
        public async Task ExcluirClientes_Fail()
        {
            await using var application = new RentCarSysApplication();
            await ClienteMockData.CreateClientes(application, true);

            var url = "cliente/excluir/1333";
            var cliente = application.CreateClient();

            var response = await cliente.DeleteAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("", errorResponse);
        }

        [Fact]
        public async Task ExcluirClientes_Success()
        {
            await using var application = new RentCarSysApplication();
            await ClienteMockData.CreateClientes(application, true);

            var url = "cliente/excluir/1";
            var cliente = application.CreateClient();
            
            var response = await cliente.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ClienteDTO>();

            Assert.NotNull(result);
        }          
    }
}
