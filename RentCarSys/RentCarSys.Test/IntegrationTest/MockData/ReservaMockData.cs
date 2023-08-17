using Microsoft.Extensions.DependencyInjection;
using RentCarSys.Application.Data;
using RentCarSys.Application.Models.Enums;
using RentCarSys.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarSys.Test.IntegrationTest.MockData
{
    public class ReservaMockData
    {
        public static async Task CreateReservas(RentCarSysApplication application, bool criar)
        {
            using (var scope = application.Services.CreateAsyncScope())
            {
                var provider = scope.ServiceProvider;
                using (var reservaDbContext = provider.GetRequiredService<Contexto>())
                {
                    await reservaDbContext.Database.EnsureCreatedAsync();

                    if (criar)
                    {
                        await reservaDbContext.Reservas.AddAsync(new Reserva
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
                                    Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678911
                                }
                            },
                            Veiculo = new List<Veiculo>
                            {
                                new Veiculo
                                {
                                    Id = 1, Placa = "placa1", Modelo = "modelo1'", Marca = "marca1"
                                }
                        }   });

                        await reservaDbContext.Reservas.AddAsync(new Reserva
                        {
                            Id = 2,
                            DataReserva = "DataReserva2",
                            ValorLocacao = 2,
                            DataRetirada = "DataRetirada2",
                            DataEntrega = "DataEntrega2",
                            Cliente = new List<Cliente>
                            {
                                new Cliente
                                {
                                    Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678912
                                }
                            },
                            Veiculo = new List<Veiculo>
                            {
                                new Veiculo
                                {
                                    Id = 2, Placa = "placa2", Modelo = "modelo2'", Marca = "marca2"
                                }
                            }
                        });
                    }
                }
            }
        }
    }
}
