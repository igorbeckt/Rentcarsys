using RentCarSys.Application.Data;
using RentCarSys.Application.Models.Enums;
using RentCarSys.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace RentCarSys.Test.IntegrationTest.MockData
{
    public class ClienteMockData
    {
        public static async Task CreateClientes(RentCarSysApplication application, bool criar)
        {
            using (var scope = application.Services.CreateAsyncScope())
            {
                var provider = scope.ServiceProvider;
                using (var clienteDbContext = provider.GetRequiredService<Contexto>())
                {
                    await clienteDbContext.Database.EnsureCreatedAsync();

                    if (criar)
                    {
                        await clienteDbContext.Clientes.AddAsync(new Cliente
                        { Id = 1, NomeCompleto = "Cliente 1", CPF = 12345678912, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online });

                        await clienteDbContext.Clientes.AddAsync(new Cliente
                        { Id = 2, NomeCompleto = "Cliente 2", CPF = 12345678911, RG = 12345678911, Email = "aa", Status = ClienteStatus.Online });

                        await clienteDbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
