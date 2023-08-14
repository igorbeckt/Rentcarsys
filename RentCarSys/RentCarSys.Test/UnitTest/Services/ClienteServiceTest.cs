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
    public async void BuscarTodosClientes_Fail()
    {
        clientesRepository.Setup(repo => repo.ObterTodosClientesAsync())
                         .Throws(new Exception("Exceção simulada"));

        var result = await clienteService.BuscarTodosClientes();

        Assert.NotNull(result);
        Assert.NotEmpty(result.Erros);
    }

    [Fact]
    public async void BuscarTodosClientes_Success()
    {
        var mockClientes = MockData.ClienteMockData.ClientesGetAll();

        clientesRepository.Setup(repo => repo.ObterTodosClientesAsync())
                         .ReturnsAsync(mockClientes);

        var result = await clienteService.BuscarTodosClientes();


        Assert.NotNull(result);
        Assert.Empty(result.Erros);
        Assert.Equal(mockClientes.Count, result.Dados.Count);
    }

    [Fact]
    public async void BuscarClientePorId_Fail()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        int clienteId = mockCliente.Id;

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .Throws(new Exception("Exceção simulada"));


        var result = await clienteService.BuscarClientePorId(clienteId);


        Assert.NotNull(result);
        Assert.NotEmpty(result.Erros);
    }

    [Fact]
    public async void BuscarClientePorId_ClienteNulo()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        int clienteId = mockCliente.Id;

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync((Cliente)null);

        var result = await clienteService.BuscarClientePorId(clienteId);


        Assert.NotNull(result);
        Assert.NotEmpty(result.Erros);
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
        Assert.Empty(result.Erros);
        Assert.Equal(mockCliente.Id, result.Dados.Id);
    }

    [Fact]
    public async void BuscarClientePorCPF_Fail()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        var cpf = mockCliente.CPF;

        clientesRepository.Setup(repo => repo.ObterClientePorCPFAsync(cpf))
                         .Throws(new Exception("Exceção simulada"));

        var result = await clienteService.BuscarClientePorCPF(cpf);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Erros);
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
        Assert.Empty(result.Erros);
        Assert.Equal(mockCliente.Id, result.Dados.Id);
    }    

    [Fact]
    public async void CriarCliente_Fail()
    {
        var mockClienteDTOCreate = MockData.ClienteMockData.ClienteCreate();

        clientesRepository.Setup(repo => repo.AdicionarClienteAsync(It.IsAny<Cliente>()))
                         .Throws(new Exception("Exceção simulada"));

        var result = await clienteService.CriarCliente(mockClienteDTOCreate);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Erros);
    }

    [Fact]
    public async void CriarCliente_Success()
    {
        var mockClienteDTOCreate = MockData.ClienteMockData.ClienteCreate();

        var mockCliente = MockData.ClienteMockData.ClienteGetById();

        clientesRepository.Setup(repo => repo.AdicionarClienteAsync(It.IsAny<Cliente>()))
                         .Callback<Cliente>(cliente =>
                         {
                             cliente.Id = mockCliente.Id;
                         })
                         .Returns(Task.CompletedTask);

        var result = await clienteService.CriarCliente(mockClienteDTOCreate);

        Assert.NotNull(result);
        Assert.Empty(result.Erros);
        Assert.Equal(mockCliente.Id, result.Dados.Id);
    }

    [Fact]
    public async void EditarCliente_Fail()
    {
        var MockId = MockData.ClienteMockData.ClienteGetById();
        var clienteId = MockId.Id;
        var mockClienteDTOUpdate = MockData.ClienteMockData.ClienteDTOUpdate();

        var mockCliente = MockData.ClienteMockData.ClienteGetById();

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync(mockCliente);

        clientesRepository.Setup(repo => repo.AtualizarClienteAsync(It.IsAny<Cliente>()))
                         .Throws(new Exception("Exceção simulada"));

        var result = await clienteService.EditarCliente(clienteId, mockClienteDTOUpdate);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Erros);
    }

    [Fact]
    public async void EditarCliente_ClienteNaoEncontrado()
    {
        var MockId = MockData.ClienteMockData.ClienteGetById();
        var clienteId = MockId.Id;
        var mockClienteDTOUpdate = MockData.ClienteMockData.ClienteDTOUpdate();

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync((Cliente)null);

        var result = await clienteService.EditarCliente(clienteId, mockClienteDTOUpdate);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Erros);
    }

    [Fact]
    public async void EditarCliente_ClienteEmEstadoInvalido()
    {
        var MockId = MockData.ClienteMockData.ClienteGetById();
        var clienteId = MockId.Id;
        var mockClienteDTOUpdate = MockData.ClienteMockData.ClienteDTOUpdate();

        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        mockCliente.Status = ClienteStatus.Running;

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync(mockCliente);

        var result = await clienteService.EditarCliente(clienteId, mockClienteDTOUpdate);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Erros);
    }   

    [Fact]
    public async void EditarCliente_Success()
    {
        var MockId = MockData.ClienteMockData.ClienteGetById();
        var clienteId = MockId.Id;
        var mockClienteDTOUpdate = MockData.ClienteMockData.ClienteDTOUpdate();

        var mockCliente = MockData.ClienteMockData.ClienteGetById(); // Cliente mock após edição

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync(mockCliente);

        clientesRepository.Setup(repo => repo.AtualizarClienteAsync(It.IsAny<Cliente>()))
                         .Returns(Task.CompletedTask);

        var result = await clienteService.EditarCliente(clienteId, mockClienteDTOUpdate);

        Assert.NotNull(result);
        Assert.Empty(result.Erros);
        Assert.Equal(mockCliente.Id, result.Dados.Id);
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

        var result = await clienteService.ExcluirCliente(clienteId);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Erros);
    }


    [Fact]
    public async void ExcluirCliente_ClienteNaoEncontrado()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        var clienteId = mockCliente.Id;

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync((Cliente)null);

        var result = await clienteService.ExcluirCliente(clienteId);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Erros);
    }

    [Fact]
    public async void ExcluirCliente_ClienteEmEstadoInvalido()
    {
        var mockCliente = MockData.ClienteMockData.ClienteGetById();
        var clienteId = mockCliente.Id;

        mockCliente.Status = ClienteStatus.Running;

        clientesRepository.Setup(repo => repo.ObterClientePorIdAsync(clienteId))
                         .ReturnsAsync(mockCliente);

        var result = await clienteService.ExcluirCliente(clienteId);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Erros);
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
        Assert.Empty(result.Erros);
        Assert.Equal(mockCliente.Id, result.Dados.Id);
    }
}