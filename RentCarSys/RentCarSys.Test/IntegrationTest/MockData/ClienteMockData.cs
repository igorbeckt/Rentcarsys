using RentCarSys.Application.Data;
using RentCarSys.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RentCarSys.Infra.Data.Context;

namespace RentCarSys.Test.IntegrationTest.MockData
{
    public class ClienteMockData
    {
        public static async Task CreateClientes(RentCarSysApplication application, IEnumerable<Cliente> clientes)
        {
            using (var scope = application.Services.CreateAsyncScope())
            {
                var provider = scope.ServiceProvider;
                using (var clienteDbContext = provider.GetRequiredService<Contexto>())
                {
                    await clienteDbContext.Database.EnsureCreatedAsync();

                    clienteDbContext.Clientes.AddRangeAsync(clientes);
                    await clienteDbContext.SaveChangesAsync();
                }
            }
        }

        public static async Task DeletarClientes(RentCarSysApplication application, IEnumerable<Cliente> clientes)
        {
            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var clienteDbContext = provider.GetRequiredService<Contexto>())
                {
                    await clienteDbContext.Database.EnsureCreatedAsync();

                    clienteDbContext.Clientes.RemoveRange(clientes);
                    await clienteDbContext.SaveChangesAsync();
                }
            }
        }
    }
}
