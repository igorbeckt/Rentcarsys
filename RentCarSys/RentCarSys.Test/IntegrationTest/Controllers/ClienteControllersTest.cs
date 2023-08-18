using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RentCarSys.Application.DTO.ClientesDTOs;
using RentCarSys.Application.Models;
using RentCarSys.Application.Models.Enums;
using RentCarSys.Test.IntegrationTest.MockData;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace RentCarSys.Test.IntegrationTest.Controllers
{
    public class ClienteControllersTest
    {
        protected RentCarSysApplication application;

        public ClienteControllersTest()
        {
            application = new RentCarSysApplication();
        }

        [Fact]
        public async Task BuscarTodosClientes_Success()
        {
            var cliente1 = new Cliente
            { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };
            var cliente2 = new Cliente
            { Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678911, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };

            List<Cliente> clientes = new List<Cliente>() { cliente1, cliente2 };

            await ClienteMockData.CreateClientes(application, clientes);

            var url = "cliente/buscarTodos";
            var cliente = application.CreateClient();

            var response = await cliente.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<ClienteDTOGetAll>>();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.NomeCompleto == "Cliente 1");
            Assert.Contains(result, c => c.NomeCompleto == "Cliente 2");

            await ClienteMockData.DeletarClientes(application, clientes);
        }

        [Fact]
        public async Task BuscarClientePorId_Fail()
        {
            var cliente1 = new Cliente
            { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };
            var cliente2 = new Cliente
            { Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678911, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };

            List<Cliente> clientes = new List<Cliente>() { cliente1, cliente2 };

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
            var cliente1 = new Cliente
            { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };
            var cliente2 = new Cliente
            { Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678911, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };

            List<Cliente> clientes = new List<Cliente>() { cliente1, cliente2 };

            await ClienteMockData.CreateClientes(application, clientes);

            var url = "cliente/buscarPorId/1";
            var cliente = application.CreateClient();

            var response = await cliente.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ClienteDTO>();
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);

            await ClienteMockData.DeletarClientes(application, clientes);
        }

        [Fact]
        public async Task BuscarClientePorCpf_Fail()
        {
            var cliente1 = new Cliente
            { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };
            var cliente2 = new Cliente
            { Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678911, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };

            List<Cliente> clientes = new List<Cliente>() { cliente1, cliente2 };

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
            var cliente1 = new Cliente
            { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };
            var cliente2 = new Cliente
            { Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678911, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };

            List<Cliente> clientes = new List<Cliente>() { cliente1, cliente2 };

            await ClienteMockData.CreateClientes(application, clientes);

            var url = "cliente/buscarPorCpf/12345678912";
            var cliente = application.CreateClient();

            var response = await cliente.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ClienteDTO>();

            Assert.NotNull(result);
            Assert.Equal(12345678912, result.CPF);

            await ClienteMockData.DeletarClientes(application, clientes);
        }

        [Fact]
        public async Task CriarCliente_Fail()
        {
            var cliente1 = new Cliente
            { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };
            var cliente2 = new Cliente
            { Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678911, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };

            List<Cliente> clientes = new List<Cliente>() { cliente1, cliente2 };

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
            var cliente1 = new Cliente
            { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };
            var cliente2 = new Cliente
            { Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678911, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };

            List<Cliente> clientes = new List<Cliente>() { cliente1, cliente2 };

            await ClienteMockData.CreateClientes(application, clientes);

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

            await ClienteMockData.DeletarClientes(application, clientes);
        }

        [Fact]
        public async Task EditarCliente_Fail()
        {
            var cliente1 = new Cliente
            { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };
            var cliente2 = new Cliente
            { Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678911, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };

            List<Cliente> clientes = new List<Cliente>() { cliente1, cliente2 };

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
            var cliente1 = new Cliente
            { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };
            var cliente2 = new Cliente
            { Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678911, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };

            List<Cliente> clientes = new List<Cliente>() { cliente1, cliente2 };

            await ClienteMockData.CreateClientes(application, clientes);

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

            await ClienteMockData.DeletarClientes(application, clientes);
        }

        [Fact]
        public async Task ExcluirClientes_Fail()
        {
            var cliente1 = new Cliente
            { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };
            var cliente2 = new Cliente
            { Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678911, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };

            List<Cliente> clientes = new List<Cliente>() { cliente1, cliente2 };

            await ClienteMockData.CreateClientes(application, clientes);

            var url = "cliente/excluir/1333";
            var cliente = application.CreateClient();

            var response = await cliente.DeleteAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("", errorResponse);

            await ClienteMockData.DeletarClientes(application, clientes);
        }

        [Fact]
        public async Task ExcluirClientes_Success()
        {
            var cliente1 = new Cliente
            { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };
            var cliente2 = new Cliente
            { Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678911, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online };

            List<Cliente> clientes = new List<Cliente>() { cliente1, cliente2 };

            await ClienteMockData.CreateClientes(application, clientes);

            var url = "cliente/excluir/1";
            var cliente = application.CreateClient();

            var response = await cliente.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ClienteDTO>();

            Assert.NotNull(result);

            clientes.Remove(cliente1);
            await ClienteMockData.DeletarClientes(application, clientes);
        }
    }
}
