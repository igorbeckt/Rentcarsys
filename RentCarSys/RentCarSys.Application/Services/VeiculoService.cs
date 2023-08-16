using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCarSys.Application.DTO.VeiculosDTOs;
using RentCarSys.Application.Interfaces;
using RentCarSys.Application.Models;
using RentCarSys.Application.Models.Enums;

namespace RentCarSys.Application.Services
{
    public class VeiculoService
    {
        private readonly IVeiculosRepository _repositorioVeiculos;
        private readonly IMapper _mapper;

        public VeiculoService(IVeiculosRepository repositorioVeiculos, IMapper mapper)
        {
            _repositorioVeiculos = repositorioVeiculos;
            _mapper = mapper;
        }

        public async Task<List<VeiculoDTOGetAll>> BuscarTodosVeiculos()
        {
            var veiculos = await _repositorioVeiculos.ObterTodosVeiculosAsync();
            var veiculoDto = _mapper.Map<List<VeiculoDTOGetAll>>(veiculos);
            return veiculoDto;
        }

        public async Task<VeiculoDTO> BuscarVeiculoPorId(int veiculoId)
        {
            var veiculo = await _repositorioVeiculos.ObterVeiculoPorIdAsync(veiculoId);
            if (veiculo == null)
            {
                throw new Exception("Veiculo não encontrado, verifique se o veiculo já foi cadastrado!");
            }

            var veiculoDto = _mapper.Map<VeiculoDTO>(veiculo);

            return veiculoDto;
        }

        public async Task<VeiculoDTO> BuscarVeiculoPorPlaca(string placa)
        {
            var veiculo = await _repositorioVeiculos.ObterVeiculoPorPlacaAsync(placa);
            if (veiculo == null)
            {
                throw new Exception("Veiculo não encontrado, verifique se a placa está correta!");
            }

            var veiculoDto = _mapper.Map<VeiculoDTO>(veiculo);
            return veiculoDto;
        }

        public async Task<VeiculoDTO> CriarVeiculo(VeiculoDTOCreate model)
        {
            var veiculo = new Veiculo
            {
                Status = VeiculoStatus.Online,
                Placa = model.Placa,
                Marca = model.Marca,
                Modelo = model.Modelo,
                AnoFabricacao = model.AnoFabricacao,
                KM = model.KM,
                QuantidadePortas = model.QuantidadePortas,
                Cor = model.Cor,
                Automatico = model.Automatico
            };

            await _repositorioVeiculos.AdicionarVeiculoAsync(veiculo);

            var veiculoDto = _mapper.Map<VeiculoDTO>(veiculo);

            return veiculoDto;
        }

        public async Task<VeiculoDTO> EditarVeiculo(int veiculoId, VeiculoDTOUpdate model)
        {
            var veiculo = await _repositorioVeiculos.ObterVeiculoPorIdAsync(veiculoId);
            if (veiculo == null)
            {
                throw new Exception("Veiculo não encontrado!");
            }

            if (veiculo.Status == VeiculoStatus.Running)
            {
                throw new Exception("Não foi possível alterar o veiculo, possui reserva em andamento");
            }

            veiculo.Placa = model.Placa;
            veiculo.Marca = model.Marca;
            veiculo.Modelo = model.Modelo;
            veiculo.AnoFabricacao = model.AnoFabricacao;
            veiculo.KM = model.KM;
            veiculo.QuantidadePortas = model.QuantidadePortas;
            veiculo.Cor = model.Cor;
            veiculo.Automatico = model.Automatico;

            await _repositorioVeiculos.AtualizarVeiculoAsync(veiculo);

            var veiculoDto = _mapper.Map<VeiculoDTO>(veiculo);

            return veiculoDto;
        }

        public async Task<VeiculoDTO> ExcluirVeiculo(int veiculoId)
        {
            var veiculo = await _repositorioVeiculos.ObterVeiculoPorIdAsync(veiculoId);
            if (veiculo == null)
            {
                throw new Exception("Veiculo não encontrado!");
            }

            if (veiculo.Status == VeiculoStatus.Running)
            {
                throw new Exception("Não foi possível excluir o veiculo, possui reserva em andamento");
            }

            await _repositorioVeiculos.ExcluirVeiculoAsync(veiculo);

            var veiculoDto = _mapper.Map<VeiculoDTO>(veiculo);

            return veiculoDto;
        }
    }

}
