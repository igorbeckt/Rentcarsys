using RentCarSys.Application.DTOs.VeiculosDTO;
using RentCarSys.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarSys.Test.UnitTest.MockData
{
    public static class VeiculoMockData
    {
        public static List<Veiculo> VeiculosGetAll()
        {
            return new List<Veiculo>
            {
                new Veiculo { Id = 1, Placa = "placa1", Modelo = "modelo1'", Marca = "marca1" },
                new Veiculo { Id = 2, Placa = "placa2", Modelo = "modelo2'", Marca = "marca2" }
            };
        }

        public static Veiculo VeiculoGetById()
        {
            return new Veiculo
            {
                Id = 1,
                Placa = "placa1",
                Modelo = "modelo1'",
                Marca = "marca1"
            };
        }

        public static VeiculoDTOCreate VeiculoCreate()
        {
            return new VeiculoDTOCreate
            {
                Placa = "placa1",
                Marca = "Marca1",
                Modelo = "Modelo1",
                AnoFabricacao = "AnoFabricacao1",
                KM = "KM1",
                QuantidadePortas = 4,
                Cor = "Cor1",
                Automatico = "Automatico1"
            };
        }

        public static VeiculoDTOUpdate VeiculoUpdate()
        {
            return new VeiculoDTOUpdate
            {
                Placa = "placa1",
                Marca = "Marca1",
                Modelo = "Modelo1",
                AnoFabricacao = "AnoFabricacao1",
                KM = "KM1",
                QuantidadePortas = 4,
                Cor = "Cor1",
                Automatico = "Automatico1"
            };
        }
    }
}
