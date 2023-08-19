using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RentCarSys.Application.DTO.ClientesDTOs;
using RentCarSys.Application.Interfaces;
using RentCarSys.Application.Models;
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

        public async Task<List<ClienteDTOGetAll>> BuscarTodosClientes()
        {
            var clientes = await _repositorioClientes.ObterTodosClientesAsync();

            var clienteDto = _mapper.Map<List<ClienteDTOGetAll>>(clientes);
            return clienteDto;
        }

        public async Task<ClienteDTO> BuscarClientePorId(int clienteId)
        {
            var cliente = await _repositorioClientes.ObterClientePorIdAsync(clienteId);
            if (cliente == null)
            {
                throw new Exception("Cliente não encontrado, verifique se o cliente já foi cadastrado!");
            }

            var clienteDto = _mapper.Map<ClienteDTO>(cliente);

            return clienteDto;
        }

        public async Task<ClienteDTO> BuscarClientePorCPF(long cpf)
        {
            var cliente = await _repositorioClientes.ObterClientePorCPFAsync(cpf);
            if (cliente == null)
            {
                throw new Exception("Cliente não encontrado, verifique se o CPF está correto!");
            }

            var clienteDto = _mapper.Map<ClienteDTO>(cliente);

            return clienteDto;
        }

        public async Task<ClienteDTO> CriarCliente(ClienteDTOCreate model)
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
            return clienteDto;
        }

        public async Task<ClienteDTO> EditarCliente(int clienteId, ClienteDTOUpdate model)
        {
            var cliente = await _repositorioClientes.ObterClientePorIdAsync(clienteId);
            if (cliente == null)
            {
                throw new Exception("Cliente não encontrado!");
            }

            if (cliente.Status == ClienteStatus.Running)
            {
                throw new Exception("Não foi possível alterar o cliente, possui reserva em andamento");
            }

            cliente.NomeCompleto = model.NomeCompleto;
            cliente.Email = model.Email;
            cliente.RG = model.RG;
            cliente.CPF = model.CPF;

            var clienteDto = _mapper.Map<ClienteDTO>(cliente);

            await _repositorioClientes.AtualizarClienteAsync(cliente);

            return clienteDto;
        }

        public async Task<ClienteDTO> ExcluirCliente(int clienteId)
        {
            var cliente = await _repositorioClientes.ObterClientePorIdAsync(clienteId);
            if (cliente == null)
            {
                throw new Exception("Cliente não encontrado!");
            }

            if (cliente.Status == ClienteStatus.Running)
            {
                throw new Exception("Não foi possível excluir o cliente, possui reserva em andamento");
            }

            var clienteDto = _mapper.Map<ClienteDTO>(cliente);

            await _repositorioClientes.ExcluirClienteAsync(cliente);

            return clienteDto;
        }
    }
}
