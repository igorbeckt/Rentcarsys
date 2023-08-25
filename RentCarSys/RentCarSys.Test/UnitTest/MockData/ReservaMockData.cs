using RentCarSys.Application.DTOs.ReservasDTO;
using RentCarSys.Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace RentCarSys.Test.UnitTest.MockData
{
    public static class ReservaMockData
    {
        public static List<Reserva> ReservasGetAll()
        {
            return new List<Reserva>
            {
                new Reserva { Id = 1,
                    DataReserva = "DataReserva",
                    ValorLocacao = 1,
                    DataRetirada = "DataRetirada1",
                    DataEntrega = "DataEntrega1",
                    Cliente = new List<Cliente>
                    {
                        new Cliente
                        {
                             Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912
                        }
                    },
                    Veiculo = new List<Veiculo>
                    {
                        new Veiculo
                        {
                            Id = 1, Placa = "placa1", Modelo = "modelo1'", Marca = "marca1"
                        }
                    }
                }, 
                new Reserva{ Id = 2,
                    DataReserva = "DataReserva2",
                    ValorLocacao = 2,
                    DataRetirada = "DataRetirada2",
                    DataEntrega = "DataEntrega2",
                    Cliente = new List<Cliente>
                    {
                        new Cliente
                        {
                             Id = 2, NomeCompleto = "Cliente2", CPF = 12345678911
                        }
                    },
                    Veiculo = new List<Veiculo>
                    {
                        new Veiculo
                        {
                            Id = 2, Placa = "placa2", Modelo = "modelo2'", Marca = "marca2"
                        }
                    }
                }
            };
        }

        public static Reserva ReservaById() 
        {
            return new Reserva
            {
                Id = 1,
                DataReserva = "DataReserva",
                ValorLocacao = 1,
                DataRetirada = "DataRetirada1",
                DataEntrega = "DataEntrega1",
                Cliente = new List<Cliente>
                {
                        new Cliente
                        {
                             Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912
                        }
                },
                Veiculo = new List<Veiculo>
                {
                        new Veiculo
                        {
                            Id = 1, Placa = "placa1", Modelo = "modelo1'", Marca = "marca1"
                        }
                }

            };
        }

        public static ReservaDTOCreate ReservaCreate()
        {
            return new ReservaDTOCreate
            {
                ClienteId = 1,
                VeiculoId = 1,
                Id = 1,
                DataReserva = "DataReserva1",
                ValorLocacao = 1,
                DataRetirada = "DataRetirada1",
                DataEntrega = "DataEntrega1"
            };
        }

        public static ReservaDTOUpdate ReservaUpdate()
        {
            return new ReservaDTOUpdate
            {               
                Id = 1,
                DataReserva = "DataReserva1",
                ValorLocacao = 1,
                DataRetirada = "DataRetirada1",
                DataEntrega = "DataEntrega1"
            };
        }
    }
}
