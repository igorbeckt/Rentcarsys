using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using RentCarSys.Application.DTO.AutoMapper;
using RentCarSys.Application.DTO.ClientesDTOs;
using RentCarSys.Application.Interfaces;
using RentCarSys.Application.Models;
using RentCarSys.Application.Models.Enums;
using RentCarSys.Application.Services;
using RentCarSys.Application.Services.RentCarSys.Application.Services;
using Xunit;

namespace RentCarSys.Test.UnitTest.Services;
public class ClienteServiceTest
{
    protected Mock<IClientesRepository> clientesRepository = new Mock<IClientesRepository>();
    protected IMapper mapper;
    private readonly ClienteService clienteService;

    public ClienteServiceTest()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new EntitiesDTOMappingProfile());
        });
        mapper = config.CreateMapper();
        clienteService = new ClienteService(clientesRepository.Object, mapper);
    }

    [Fact]
    public async Task BuscarTodosClientes_Fail()
    {
        clientesRepository.Setup(repo => repo.ObterTodosClientesAsync())
                         .Throws(new Exception("Exceção simulada"));
        
        await Assert.ThrowsAsync<Exception>(async () => await clienteService.BuscarTodosClientes());
    }

    [Fact]
    public async Task BuscarTodosClientes_Success()
    {
        var mockClientes = MockData.ClienteMockData.ClientesGetAll();

        clientesRepository.Setup(repo => repo.ObterTodosClientesAsync())
                         .ReturnsAsync(mockClientes);

        var result = await clienteService.BuscarTodosClientes();
        
        Assert.NotNull(result);        
        Assert.IsType<List<ClienteDTOGetAll>>(result);        
        Assert.Equal(mockClientes.Count, result.Count);
    }


    [Fact]
    public async Task BuscarClientePorId_Fail()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        int clienteId = mockCliente.Id;

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .Throws(new Exception("Exceção simulada"));

        await Assert.ThrowsAsync<Exception>(async () => await clienteService.BuscarClientePorId(clienteId));
    }

    [Fact]
    public async void BuscarClientePorId_ClienteNulo()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        int clienteId = mockCliente.Id;

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync((Cliente)null);

        await Assert.ThrowsAsync<Exception>(async () => await clienteService.BuscarClientePorId(clienteId));
    }

    [Fact]
    public async void BuscarClientePorId_Success()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        int clienteId = mockCliente.Id;

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync(mockCliente);

        var result = await clienteService.BuscarClientePorId(clienteId);

        Assert.NotNull(result);
        Assert.IsType<ClienteDTO>(result);
    }

    [Fact]
    public async void BuscarClientePorCPF_Fail()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        var cpf = mockCliente.CPF;

        clientesRepository.Setup(repo => repo.ObterClientePorCPFAsync(cpf))
                         .Throws(new Exception("Exceção simulada"));        

        await Assert.ThrowsAsync<Exception>(async () => await clienteService.BuscarClientePorCPF(cpf));
    }

    [Fact]
    public async void BuscarClientePorCPF_ClienteNulo()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        var clienteCpf = mockCliente.CPF;

        clientesRepository.Setup(repo => repo.ObterClientePorCPFAsync(clienteCpf))
                         .ReturnsAsync((Cliente)null);

        await Assert.ThrowsAsync<Exception>(async () => await clienteService.BuscarClientePorCPF(clienteCpf));
    }

    [Fact]
    public async void BuscarClientePorCPF_Success()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        var cpf = mockCliente.CPF;

        clientesRepository.Setup(repo => repo.ObterClientePorCPFAsync(cpf))
                         .ReturnsAsync(mockCliente);

        var result = await clienteService.BuscarClientePorCPF(cpf);

        Assert.NotNull(result);
        Assert.IsType<ClienteDTO>(result);
    }    

    [Fact]
    public async void CriarClienteFail_Exception()
    {
        var mockClienteCreate = MockData.ClienteMockData.ClienteCreate();

        clientesRepository.Setup(repo => repo.AdicionarClienteAsync(It.IsAny<Cliente>()))
                         .Throws(new Exception("Exceção simulada"));        

        await Assert.ThrowsAsync<Exception>(async () => await clienteService.CriarCliente(mockClienteCreate));
    }

    [Fact]
    public async void CriarCliente_Success()
    {
        var mockClienteCreate = MockData.ClienteMockData.ClienteCreate();

        var mockCliente = MockData.ClienteMockData.ClienteGetById();

        clientesRepository.Setup(repo => repo.AdicionarClienteAsync(It.IsAny<Cliente>()))
                         .Callback<Cliente>(cliente =>
                         {
                             cliente.Id = mockCliente.Id;
                         })
                         .Returns(Task.CompletedTask);

        var result = await clienteService.CriarCliente(mockClienteCreate);

        Assert.NotNull(result);
        Assert.Equal(mockCliente.Id, result.Id);
    }

    [Fact]
    public async void EditarCliente_Fail()
    {
        var MockId = MockData.ClienteMockData.ClienteGetById();
        var clienteId = MockId.Id;
        var mockClienteUpdate = MockData.ClienteMockData.ClienteDTOUpdate();

        var mockCliente = MockData.ClienteMockData.ClienteGetById();

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync(mockCliente);

        clientesRepository.Setup(repo => repo.AtualizarClienteAsync(It.IsAny<Cliente>()))
                         .Throws(new Exception("Exceção simulada"));

        await Assert.ThrowsAsync<Exception>(async () => await clienteService.EditarCliente(clienteId, mockClienteUpdate));
    }

    [Fact]
    public async void EditarCliente_ClienteNaoEncontrado()
    {
        var mockClienteUpdate = MockData.ClienteMockData.ClienteDTOUpdate();
        var clienteId = mockClienteUpdate.Id;
        

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync((Cliente)null);

        await Assert.ThrowsAsync<Exception>(async () => await clienteService.EditarCliente(clienteId, mockClienteUpdate));
    }

    [Fact]
    public async void EditarCliente_ClienteEmEstadoInvalido()
    {
        var mockClienteUpdate = MockData.ClienteMockData.ClienteDTOUpdate();
        var clienteId = mockClienteUpdate.Id;
        var mockStatus = new Cliente { Status = ClienteStatus.Running };

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync(mockStatus);

        await Assert.ThrowsAsync<Exception>(async () => await clienteService.EditarCliente(clienteId, mockClienteUpdate));
    }   

    [Fact]
    public async void EditarCliente_Success()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        var clienteId = mockCliente.Id;
        var mockClienteUpdate = MockData.ClienteMockData.ClienteDTOUpdate();
        

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync(mockCliente);

        clientesRepository.Setup(repo => repo.AtualizarClienteAsync(It.IsAny<Cliente>()))
                         .Returns(Task.CompletedTask);

        var result = await clienteService.EditarCliente(clienteId, mockClienteUpdate);

        Assert.NotNull(result);
        Assert.Equal(mockCliente.Id, result.Id);
    }

    [Fact]
    public async void ExcluirCliente_Fail()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        var clienteId = mockCliente.Id;

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync(mockCliente);

        clientesRepository.Setup(repo => repo.ExcluirClienteAsync(It.IsAny<Cliente>()))
                         .Throws(new Exception("Exceção simulada"));       

        await Assert.ThrowsAsync<Exception>(async () => await clienteService.ExcluirCliente(clienteId));
    }


    [Fact]
    public async void ExcluirCliente_ClienteNaoEncontrado()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        var clienteId = mockCliente.Id;

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync((Cliente)null);

        await Assert.ThrowsAsync<Exception>(async () => await clienteService.ExcluirCliente(clienteId));
    }

    [Fact]
    public async void ExcluirCliente_ClienteEmEstadoInvalido()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        var clienteId = mockCliente.Id;
        mockCliente.Status = ClienteStatus.Running;

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync(mockCliente);

        await Assert.ThrowsAsync<Exception>(async () => await clienteService.ExcluirCliente(clienteId));
    }

    [Fact]
    public async void ExcluirCliente_Success()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        var clienteId = mockCliente.Id;

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync(mockCliente);

        clientesRepository.Setup(repo => repo.ExcluirClienteAsync(It.IsAny<Cliente>()))
                         .Returns(Task.CompletedTask);

        var result = await clienteService.ExcluirCliente(clienteId);

        Assert.NotNull(result);
        Assert.Equal(mockCliente.Id, result.Id);
    }
}