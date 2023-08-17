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
    public class VeiculoMockData
    {
        public static async Task CreateVeiculos(RentCarSysApplication application, bool criar)
        {
            using (var scope = application.Services.CreateAsyncScope())
            {
                var provider = scope.ServiceProvider;
                using (var veiculoDbContext = provider.GetRequiredService<Contexto>())
                {
                    await veiculoDbContext.Database.EnsureCreatedAsync();

                    if (criar)
                    {
                        await veiculoDbContext.Veiculos.AddAsync(new Veiculo
                        { Id = 1, Status = VeiculoStatus.Online, Placa = "Placaa1", Marca = "Marca1", Modelo = "Modelo1", AnoFabricacao = "Ano1", KM = "KM1", QuantidadePortas = 2, Cor = "Cor1", Automatico = "Automatico1" });

                        await veiculoDbContext.Veiculos.AddAsync(new Veiculo
                        { Id = 2, Status = VeiculoStatus.Online, Placa = "Placaa2", Marca = "Marca2", Modelo = "Modelo2", AnoFabricacao = "Ano2", KM = "KM2", QuantidadePortas = 4, Cor = "Cor2", Automatico = "Automatico2" });

                        await veiculoDbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
