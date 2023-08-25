using Microsoft.Extensions.DependencyInjection;
using RentCarSys.Application.Models;
using RentCarSys.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarSys.Test.IntegrationTest.MockData
{
    public class VeiculoMockData
    {        
        public static async Task CreateVeiculos(RentCarSysApplication application, IEnumerable<Veiculo> veiculos)
        {
            using (var scope = application.Services.CreateAsyncScope())
            {
                var provider = scope.ServiceProvider;
                using (var clienteDbContext = provider.GetRequiredService<Contexto>())
                {
                    await clienteDbContext.Database.EnsureCreatedAsync();

                    clienteDbContext.Veiculos.AddRangeAsync(veiculos);
                    await clienteDbContext.SaveChangesAsync();
                }
            }
        }

        public static async Task DeletarVeiculos(RentCarSysApplication application, IEnumerable<Veiculo> veiculos)
        {
            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var clienteDbContext = provider.GetRequiredService<Contexto>())
                {
                    await clienteDbContext.Database.EnsureCreatedAsync();

                    clienteDbContext.Veiculos.RemoveRange(veiculos);
                    await clienteDbContext.SaveChangesAsync();
                }
            }
        }
    }
}
