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
        public static async Task CreateReservas(RentCarSysApplication application, IEnumerable<Reserva> reservas)
        {
            using (var scope = application.Services.CreateAsyncScope())
            {
                var provider = scope.ServiceProvider;
                using (var clienteDbContext = provider.GetRequiredService<Contexto>())
                {
                    await clienteDbContext.Database.EnsureCreatedAsync();

                    clienteDbContext.Reservas.AddRangeAsync(reservas);
                    await clienteDbContext.SaveChangesAsync();
                }
            }
        }

        public static async Task DeletarReservas(RentCarSysApplication application, IEnumerable<Reserva> reservas)
        {
            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var clienteDbContext = provider.GetRequiredService<Contexto>())
                {
                    await clienteDbContext.Database.EnsureCreatedAsync();

                    clienteDbContext.Reservas.RemoveRange(reservas);
                    await clienteDbContext.SaveChangesAsync();
                }
            }
        }
    }
}
