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
    public static class ClienteMockData 
    {   
        public static async Task CreateClientes(Contexto contexto, IEnumerable<Cliente> clientes)
        {
            await contexto.Database.EnsureCreatedAsync();

            await contexto.Clientes.AddRangeAsync(clientes);
            await contexto.SaveChangesAsync();
        }

        public static async Task DeletarClientes(Contexto contexto, IEnumerable<Cliente> clientes)
        {
            await contexto.Database.EnsureCreatedAsync();

            contexto.Clientes.RemoveRange(clientes);
            await contexto.SaveChangesAsync();
        }
        
    }
}
