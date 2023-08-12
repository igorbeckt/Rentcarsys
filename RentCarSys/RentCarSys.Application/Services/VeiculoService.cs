using AutoMapper;
using Localdorateste.Data;
using Localdorateste.Extensions;
using Localdorateste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCarSys.Application.DTO;
using RentCarSys.Application.Extensions;
using RentCarSys.Application.Interfaces;
using RentCarSys.Enums;

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

        public async Task<ResultViewModel<List<VeiculoDTOGetAll>>> BuscarTodosVeiculos()
        {
            try
            {
                var veiculos = await _repositorioVeiculos.ObterTodosVeiculosAsync();

                var veiculoDto = _mapper.Map<List<VeiculoDTOGetAll>>(veiculos);
                return new ResultViewModel<List<VeiculoDTOGetAll>>(veiculoDto);
            }
            catch
            {
                return new ResultViewModel<List<VeiculoDTOGetAll>>(erro: "05X05 - Falha interna no servidor!");
            }
        }

        public async Task<ResultViewModel<VeiculoDTO>> BuscarVeiculoPorId(int veiculoId)
        {
            try
            {
                var veiculo = await _repositorioVeiculos.ObterVeiculoPorIdAsync(veiculoId);
                if (veiculo == null)
                {
                    return new ResultViewModel<VeiculoDTO>(erro: "Veiculo não encontrado, verifique se o veiculo já foi cadastrado!");
                }

                var veiculoDto = _mapper.Map<VeiculoDTO>(veiculo);

                return new ResultViewModel<VeiculoDTO>(veiculoDto);
            }
            catch
            {
                return new ResultViewModel<VeiculoDTO>(erro: "Falha interna no servidor!");
            }
        }

        public async Task<ResultViewModel<VeiculoDTO>> BuscarVeiculoPorPlaca(string placa)
        {
            try
            {
                var veiculo = await _repositorioVeiculos.ObterVeiculoPorPlacaAsync(placa);
                if (veiculo == null)
                {
                    return new ResultViewModel<VeiculoDTO>("Veiculo não encontrado, verifique se a placa está correta!");
                }

                var veiculoDto = _mapper.Map<VeiculoDTO>(veiculo);
                return new ResultViewModel<VeiculoDTO>(veiculoDto);
            }
            catch
            {
                return new ResultViewModel<VeiculoDTO>("Falha interna no servidor!");
            }
        }

        public async Task<ResultViewModel<VeiculoDTOCREATE>> CriarVeiculo(VeiculoDTOCREATE model)
        {
            try
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

                var veiculoDto = _mapper.Map<VeiculoDTOCREATE>(veiculo);

                return new ResultViewModel<VeiculoDTOCREATE>(veiculoDto);
            }
            catch
            {
                return new ResultViewModel<VeiculoDTOCREATE>("05X10 - Falha interna no servidor!");
            }
        }

        public async Task<ResultViewModel<VeiculoDTO>> EditarVeiculo(int veiculoId, VeiculoDTO model)
        {

            try
            {
                var veiculo = await _repositorioVeiculos.ObterVeiculoPorIdAsync(veiculoId);
                if (veiculo == null)
                {
                    return new ResultViewModel<VeiculoDTO>("Veiculo não encontrado!");
                }

                if (veiculo.Status == VeiculoStatus.Running)
                {
                    return new ResultViewModel<VeiculoDTO>("Não foi possível alterar o veiculo, possui reserva em andamento");
                }

                veiculo.Placa = model.Placa;
                veiculo.Marca = model.Marca;
                veiculo.Modelo = model.Modelo;
                veiculo.AnoFabricacao = model.AnoFabricacao;
                veiculo.KM = model.KM;
                veiculo.QuantidadePortas = model.QuantidadePortas;
                veiculo.Cor = model.Cor;
                veiculo.Automatico = model.Automatico;

                var veiculoDto = _mapper.Map<VeiculoDTO>(veiculo);

                await _repositorioVeiculos.AtualizarVeiculoAsync(veiculo);

                return new ResultViewModel<VeiculoDTO>(veiculoDto);
            }
            catch
            {
                return new ResultViewModel<VeiculoDTO>("05X11 - Falha interna no servidor!");
            }
        }

        public async Task<ResultViewModel<VeiculoDTO>> ExcluirVeiculo(int veiculoId)
        {
            try
            {
                var veiculo = await _repositorioVeiculos.ObterVeiculoPorIdAsync(veiculoId);
                if (veiculo == null)
                {
                    return new ResultViewModel<VeiculoDTO>("Veiculo não encontrado!");
                }

                if (veiculo.Status == VeiculoStatus.Running)
                {
                    return new ResultViewModel<VeiculoDTO>("Não foi possível excluir o veiculo, possui reserva em andamento");
                }

                var veiculoDto = _mapper.Map<VeiculoDTO>(veiculo);

                await _repositorioVeiculos.ExcluirVeiculoAsync(veiculo);

                return new ResultViewModel<VeiculoDTO>(veiculoDto);
            }
            catch
            {
                return new ResultViewModel<VeiculoDTO>("05X12 - Falha interna no servidor!");
            }
        }
    }
}
