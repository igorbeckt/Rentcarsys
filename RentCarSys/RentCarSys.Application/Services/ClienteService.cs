using AutoMapper;
using Localdorateste.Extensions;
using Localdorateste.Models;
using Microsoft.AspNetCore.Mvc;
using RentCarSys.Application.DTO.ClienteDTOs;
using RentCarSys.Application.DTO.ClientesDTOs;
using RentCarSys.Application.Extensions;
using RentCarSys.Application.Interfaces;
using RentCarSys.Application.Models.Enums;
using System.Web.Mvc;

namespace RentCarSys.Application.Services
{
    public class ClienteService
    {
        private readonly IClientesRepository _repositorioClientes;
        private readonly IMapper _mapper;

        public ClienteService(IClientesRepository repositorioClientes, IMapper mapper)
        {
            _repositorioClientes = repositorioClientes;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<List<ClienteDTOGetAll>>> BuscarTodosClientes()
        {
            try
            {
                var clientes = await _repositorioClientes.ObterTodosClientesAsync();

                var clienteDto = _mapper.Map<List<ClienteDTOGetAll>>(clientes);
                return new ResultViewModel<List<ClienteDTOGetAll>>(clienteDto);
            }
            catch
            {
                return new ResultViewModel<List<ClienteDTOGetAll>>(erro: "05X05 - Falha interna no servidor!");
            }
        }

        public async Task<ResultViewModel<ClienteDTO>> BuscarClientePorId(int clienteId)
        {
            try
            {
                var cliente = await _repositorioClientes.ObterClientePorIdAsync(clienteId);
                if (cliente == null)
                {
                    return new ResultViewModel<ClienteDTO>(erro: "Cliente não encontrado, verifique se o cliente já foi cadastrado!");
                }

                var clienteDto = _mapper.Map<ClienteDTO>(cliente);

                return new ResultViewModel<ClienteDTO>(clienteDto);
            }
            catch
            {
                return new ResultViewModel<ClienteDTO>(erro: "Falha interna no servidor!");
            }
        }

        public async Task<ResultViewModel<ClienteDTO>> BuscarClientePorCPF(long cpf)
        {
            try
            {
                var cliente = await _repositorioClientes.ObterClientePorCPFAsync(cpf);
                if (cliente == null)
                {
                    return new ResultViewModel<ClienteDTO>("Cliente não encontrado, verifique se o CPF está correto!");
                }

                var clienteDto = _mapper.Map<ClienteDTO>(cliente);

                return new ResultViewModel<ClienteDTO>(clienteDto);
            }
            catch
            {
                return new ResultViewModel<ClienteDTO>("Falha interna no servidor!");
            }
        }

        public async Task<ResultViewModel<ClienteDTO>> CriarCliente(ClienteDTOCreate model)
        {
            try
            {
                var cliente = new Cliente
                {
                    Status = ClienteStatus.Online,
                    NomeCompleto = model.NomeCompleto,
                    Email = model.Email,
                    RG = model.RG,
                    CPF = model.CPF,
                };

                await _repositorioClientes.AdicionarClienteAsync(cliente);

                var clienteDto = _mapper.Map<ClienteDTO>(cliente);
                return new ResultViewModel<ClienteDTO>(clienteDto);
            }
            catch
            {
                return new ResultViewModel<ClienteDTO>("05X10 - Falha interna no servidor!");
            }
        }

        public async Task<ResultViewModel<ClienteDTO>> EditarCliente(int clienteId, ClienteDTOUpdate model)
        {

            try
            {
                var cliente = await _repositorioClientes.ObterClientePorIdAsync(clienteId);
                if (cliente == null)
                {
                    return new ResultViewModel<ClienteDTO>("Cliente não encontrado!");
                }

                if (cliente.Status == ClienteStatus.Running)
                {
                    return new ResultViewModel<ClienteDTO>("Não foi possível alterar o cliente, possui reserva em andamento");
                }

                cliente.NomeCompleto = model.NomeCompleto;
                cliente.Email = model.Email;
                cliente.RG = model.RG;
                cliente.CPF = model.CPF;

                var clienteDto = _mapper.Map<ClienteDTO>(cliente);

                await _repositorioClientes.AtualizarClienteAsync(cliente);

                
                return new ResultViewModel<ClienteDTO>(clienteDto);
            }
            catch
            {
                return new ResultViewModel<ClienteDTO>("05X11 - Falha interna no servidor!");
            }
        }

        public async Task<ResultViewModel<ClienteDTO>> ExcluirCliente(int clienteId)
        {
            try
            {
                var cliente = await _repositorioClientes.ObterClientePorIdAsync(clienteId);
                if (cliente == null)
                {
                    return new ResultViewModel<ClienteDTO>("Cliente não encontrado!");
                }

                if (cliente.Status == ClienteStatus.Running)
                {
                    return new ResultViewModel<ClienteDTO>("Não foi possível excluir o cliente, possui reserva em andamento");
                }

                var clienteDto = _mapper.Map<ClienteDTO>(cliente);

                await _repositorioClientes.ExcluirClienteAsync(cliente);
                
                return new ResultViewModel<ClienteDTO>(clienteDto);
            }
            catch
            {
                return new ResultViewModel<ClienteDTO>("05X12 - Falha interna no servidor!");
            }
        }
    }
}
